using Em.Future._2017.Common;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ViewModels;
namespace CarRepairAPI.Controllers
{
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {
        /// <summary>
        /// 根据code 获取 thirdSession
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("GetThirdSession")]
        [HttpGet]
        public DataResultModel GetThirdSession(string code="")
        {
            DataResultModel result = new DataResultModel();
            try
            {
                result.data = WechatService.GetThirdSession(code);
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 获取用户的信息
        /// </summary>
        /// <param name="encryptedData">包括敏感数据在内的完整用户信息的加密数据</param>
        /// <param name="iv">加密算法的初始向量</param>
        /// <param name="thirdSession"> 获取 sessionKey</param>
        /// <returns></returns>
        [Route("GetUserInfo")]
        [HttpGet]
        public DataResultModel GetUserInfo(string encryptedData, string iv, string thirdSession)
        {
            //Convert.ToBase64String()转换过的参数发现，+都变成了空格
            encryptedData = encryptedData.Replace(" ", "+");
            iv = iv.Replace(" ", "+");
            thirdSession= thirdSession.Replace(" ", "+");
            DataResultModel result = new DataResultModel();
            try
            {
                WechatUserInfo _WechatUserInfo = WechatService.GetUserInfo(encryptedData,iv,thirdSession);
                result.data = _WechatUserInfo;
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }
    }
}