using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Checks
{
    public interface ICheck
    {

        /// <summary>
        /// 检查操作是否可执行
        /// </summary>
        /// <returns></returns>
        bool IsCheck();
    }
}
