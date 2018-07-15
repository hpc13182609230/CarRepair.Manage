using Em.Future._2017.Common;
using HelperLib;
using Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ViewModels;
using ViewModels.CarRepair;

namespace CarRepairAPI.Controllers
{
    [RoutePrefix("api/Wechat")]
    public class WechatController : ApiController
    {
        #region 基本类 实例化
        WXMessageTemplateService _WXMessageTemplateService = new WXMessageTemplateService();

        #endregion
       
        #region 服务号 推送
        /// <summary>
        /// 修理厂 给 配件商 电话 推送
        /// </summary>
        /// <param name="wechatID">小程序 用户 id</param>
        /// <param name="partsCompanyID">配件商 id</param>
        /// <returns></returns>
        [Route("MessageTemplate_Tel_Push")]
        [HttpGet]
        public DataResultModel MessageTemplate_Tel_Push(long wechatID, long partsCompanyID)
        {
            DataResultModel result = new DataResultModel();
            try
            {
                result.data = _WXMessageTemplateService.WX_MessageTemplate_Tel_Push(wechatID, partsCompanyID);
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 服务号  绑定会员推送
        /// </summary>
        /// <param name="wechatID">小程序 用户 id</param>
        /// <param name="touser">服务号 用户 openid</param>
        /// <returns></returns>
        [Route("MessageTemplate_BindMember_Push")]
        [HttpGet]
        public DataResultModel MessageTemplate_BindMember_Push(long wechatID, long partsCompanyID)
        {
            DataResultModel result = new DataResultModel();
            try
            {
                result.data = _WXMessageTemplateService.WX_MessageTemplate_BindMember_Push(wechatID, partsCompanyID);
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }

        #endregion



    }
}
