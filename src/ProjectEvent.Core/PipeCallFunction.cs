using Newtonsoft.Json;
using ProjectEvent.Core.Net;
using ProjectEvent.Core.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEvent.Core
{
    public class PipeCallFunction
    {
        static object connectLock = new object();
        static PipeNet pipeNet;
        static bool isConnected = false;
        public delegate void PipeCallFeedbackEventHandler(PipeCallFunctionFeedbackStruct fb);
        public static event PipeCallFeedbackEventHandler OnCallFeedback;
        public static void Connect()
        {
            lock (connectLock)
            {
                if (!isConnected)
                {
                    isConnected = true;
                    pipeNet = new PipeNet("callfunction");
                    pipeNet.OnReceiveMsg += PipeNet_OnReceiveMsg;
                    pipeNet.Start();
                }
            }
        }

        private static void PipeNet_OnReceiveMsg(PipeNet sender, string msg)
        {
            var fb = JsonConvert.DeserializeObject<PipeCallFunctionFeedbackStruct>(msg);
            OnCallFeedback?.Invoke(fb);
        }

        public static void Close()
        {
            if (isConnected)
            {

                isConnected = false;
            }
        }


        public static void Call(PipeCallFunctionStruct data)
        {
            Connect();
            pipeNet.Send(JsonConvert.SerializeObject(data));
        }
    }
}
