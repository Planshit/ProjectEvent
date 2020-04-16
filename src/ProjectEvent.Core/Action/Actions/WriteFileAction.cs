using ProjectEvent.Core.Action.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEvent.Core.Action.Actions
{
    public class WriteFileAction : IAction
    {
        public System.Action GetAction(string[] args)
        {
            return () =>
            {
                Debug.WriteLine("write file:" + args[0]);
                File.WriteAllText(args[0], args[1]);
            };
        }
    }
}
