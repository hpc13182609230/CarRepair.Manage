using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRepairAPI.Models
{
    public class APIRequestModels_Base
    {
        public string ApiKey { get; set; }

        public string ApiSecret { get; set; }

        public string Date { get; set; }

        public string Sign { get; set; }
    }


    public class APIRequestModels_Call
    {
        /// <summary>
        /// 通话的 唯一标识 
        /// </summary>
        public long CallID { get; set; }

        /// <summary>
        /// 配件商 客户端  的 session
        /// </summary>
        public string LoginToken { get; set; }

        /// <summary>
        /// 来电 号码
        /// </summary>
        public string Number { get; set; }
    }

    public class APIRequestModels_Login
    {
        /// <summary>
        /// 登录账户
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登录 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 登录 token
        /// </summary>
        public string LoginToken { get; set; }
    }
}