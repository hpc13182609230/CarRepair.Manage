using Em.Future._2017.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class WechatService
    {
        public static string GetThirdSession(string code)
        {
            WeChatAppDecrypt _WeChatAppDecrypt = new WeChatAppDecrypt() ;
            OpenIdAndSessionKey _OpenIdAndSessionKey = _WeChatAppDecrypt.DecodeOpenIdAndSessionKey(code);
            string thirdSession = _OpenIdAndSessionKey.session_key;//Guid.NewGuid().ToString();
            //此处绑定 thirdSession 和 OpenIdAndSessionKey 的关系到数据库
            return thirdSession;
        }


        public static WechatUserInfo GetUserInfo(string encryptedData, string iv, string thirdSession)
        {
            WeChatAppDecrypt _WeChatAppDecrypt = new WeChatAppDecrypt() ;

            //根据thirdSession 获取 sessionKey
            string sessionKey = thirdSession;

            WechatUserInfo _WechatUserInfo = _WeChatAppDecrypt.Decrypt(encryptedData, iv, sessionKey);
            return _WechatUserInfo;
        }
    }
}
