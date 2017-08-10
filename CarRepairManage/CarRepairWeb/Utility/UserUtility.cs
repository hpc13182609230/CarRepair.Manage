using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRepairWeb.Utility
{
    public class UserUtility
    {
        private UserUtility() { }
        private static UserUtility _instance = new UserUtility();
        /// <summary>
        /// 获取当前用户实例
        /// </summary>
        public static UserUtility Current
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// 帐号ID
        /// </summary>
        public int AccountID
        {
            get
            {
                try { return (int)HttpContext.Current.Session["AccountID"]; }
                catch { return 0; }
            }
            private set
            {
                try { HttpContext.Current.Session["AccountID"] = value; }
                catch { }
            }
        }


        public bool IsLogin
        {
            get
            {
                return AccountID != 0;
            }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get
            {
                try { return (string)HttpContext.Current.Session["UserName"]; }
                catch { return ""; }
            }
            private set
            {
                try { HttpContext.Current.Session["UserName"] = value; }
                catch { }
            }
        }

        public void SetLogin(int accountID, string username)
        {
            AccountID = accountID;
            UserName = username;
        }

        public void Logoff()
        {
            AccountID = 0;
            UserName = "";
        }
    }
}