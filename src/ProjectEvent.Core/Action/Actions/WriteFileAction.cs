using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectEvent.Core.Action.Actions
{
    public class WriteFileAction : IAction
    {
        public Task<object> GenerateAction(int taskID, int actionID, object[] args)
        {
            var task = new Task<object>(() =>
            {
                Debug.WriteLine("write file:" + args[0]);
                Thread.Sleep(5000);
                try
                {
                    File.WriteAllText(args[0].ToString(), args[1].ToString());
                    return true;
                }
                catch
                {
                    return false;
                }
            });
            return task;
        }
    }
}
