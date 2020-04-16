using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Checks
{
    public class WriteFileCheck : ICheck
    {
        private readonly string[] args;
        public WriteFileCheck(string[] args)
        {
            this.args = args;
        }
        public bool IsCheck()
        {
            return args.Length == 2;
        }
    }
}
