using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLib
{
    /// <summary>
    /// 配置文件Helper
    /// </summary>
    public class ConfigureHelper
    {
        /// <summary>
        ///  获取节点内容
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key)
        {
            string value = (ConfigurationManager.AppSettings[key] ?? "").Trim();
            return value;
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConnectionString(string key)
        {
            string value = (ConfigurationManager.ConnectionStrings[key].ConnectionString ?? "").Trim();
            return value;
        }
    }
}
