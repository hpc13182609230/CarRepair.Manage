using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogLib
{
    public class Tracer
    {
        private static string _filepath = System.AppDomain.CurrentDomain.BaseDirectory + (ConfigurationManager.AppSettings["LogPath"] ?? "").Trim();
        private static bool IsInitMessageLevel = false;
        private static MessageLevel messageLevel = MessageLevel.DEBUG;
        private static object lockObj = new object();


        private static void InitMessageLevel()
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MessageLevel"] ?? ""))
            {
                string _messageLevel = ConfigurationManager.AppSettings["MessageLevel"] ?? "";

                if (_messageLevel.ToUpper() == MessageLevel.RUNTIME.ToString())
                    messageLevel = MessageLevel.RUNTIME;
                else if (_messageLevel.ToUpper() == MessageLevel.DEBUG.ToString())
                    messageLevel = MessageLevel.DEBUG;
                else
                    messageLevel = MessageLevel.DEBUG;
            }
            else
                messageLevel = MessageLevel.DEBUG;

        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="mt"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int RunLog(MessageType mt, string path, string fileName = "", string msg = "")
        {
            Monitor.Enter(lockObj);//在指定的对象上获取排它锁
            if (!IsInitMessageLevel) InitMessageLevel();
            if (messageLevel == MessageLevel.RUNTIME && mt == MessageType.Information) return 1;
            try
            {
                EventLister el = new EventLister();
                el.EventLevel = mt;
                el.Body = msg;
                System.IO.File.AppendAllText(CreatePath(path, fileName), el.GetMessage() + "\r\n");
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                Monitor.Exit(lockObj);
            }
        }

        /// <summary>
        /// 拼接要输出的文本的绝对路径+后缀名
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string CreatePath(string path, string fileName)
        {
            string FilePath = _filepath;
            if (!string.IsNullOrEmpty(path))
            {
                FilePath += path + "\\";
            }

            FilePath += DateTime.Now.Year;
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }

            FilePath = FilePath + "\\" + DateTime.Now.Month;
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }

            FilePath += "\\" + fileName + DateTime.Now.Day + ".txt";
            return FilePath;
        }
    }
}
