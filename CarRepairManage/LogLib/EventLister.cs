using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogLib
{
    public class EventLister
    {
        string _timeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        string _body;
        /// <summary>
        /// 获取或设置消息体
        /// </summary>
        public string Body
        {
            get { return _body; }
            set { _body = value.Trim(); }
        }

        MessageType _eventLevel = MessageType.Information;
        /// <summary>
        /// 获取或设置事件等级
        /// </summary>
        public MessageType EventLevel
        {
            get { return _eventLevel; }
            set { _eventLevel = value; }
        }

        /// <summary>
        /// 创建输出消息文字
        /// </summary>
        public string GetMessage()
        {
            StringBuilder sb = new StringBuilder();

            switch (this.EventLevel)
            {
                case MessageType.Information: sb.Append("~Info"); break;
                case MessageType.Warning: sb.Append("!Warning"); break;
                case MessageType.Error: sb.Append("^Error"); break;
                case MessageType.WriteInfomation: sb.Append("~Info"); break;
            }

            sb.AppendFormat("[{0}] ", DateTime.Now.ToString(_timeFormat));

            sb.AppendFormat("{0}", this.Body.Trim());
            return sb.ToString();
        }

    }
}
