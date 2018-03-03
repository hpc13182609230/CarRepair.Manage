using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogLib
{
    public enum MessageType
    {
        /// <summary>
        /// 信息
        /// </summary>
        Information = 0,
        /// <summary>
        /// 警告
        /// </summary>
        Warning = 1,
        /// <summary>
        /// 错误
        /// </summary>
        Error = 2,
        /// <summary>
        /// 需要记录的信息
        /// </summary>
        WriteInfomation = 4,

        Fatal=5,
    }
}
