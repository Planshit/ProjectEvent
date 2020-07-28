using Newtonsoft.Json;
using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Net;
using ProjectEvent.Core.Structs;
using ProjectEvent.UI.Controls.Window;
using ProjectEvent.UI.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectEvent.UI.Services
{
    public class CallFunctionPipes : ICallFunctionPipes
    {
        private PipeNet pipeNet;
        private bool isRuning = false;
        public CallFunctionPipes()
        {
            pipeNet = new PipeNet("callfunction");
            pipeNet.OnReceiveMsg += PipeNet_OnReceiveMsg;
        }

        private void PipeNet_OnReceiveMsg(PipeNet sender, string msg)
        {
            var call = JsonConvert.DeserializeObject<PipeCallFunctionStruct>(msg);
            switch (call.CallFunctionType)
            {
                case PipeCallFunctionType.Dialog:
                    CallDialog(call);
                    break;
            }
        }

        public void StartServer()
        {
            if (!isRuning)
            {
                pipeNet.StartMain();
                isRuning = true;
            }
        }

        private void CallDialog(PipeCallFunctionStruct call)
        {
            var data = ObjectConvert.Get<DialogActionParamsModel>(call.Data);
            Application.Current.Dispatcher.Invoke(() =>
            {
                var dialog = new DialogWindow(data.Title, data.Content, data.Image, data.Buttons);
                dialog.OnWindowClosedEvent += (sender, value) =>
                {
                    var feedback = new PipeCallFunctionFeedbackStruct();
                    feedback.ID = call.ID;
                    feedback.CallFunctionType = call.CallFunctionType;
                    feedback.FeedbackData = value;
                    pipeNet.Send(JsonConvert.SerializeObject(feedback));
                };
                dialog.Show();
            });
        }

        public void CloseServer()
        {
            if (isRuning)
            {
                isRuning = false;
                pipeNet.Close();
            }
        }
    }
}
