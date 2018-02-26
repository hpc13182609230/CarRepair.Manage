using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModels;
using ViewModels.CarRepair;
using AutoMapperLib;
using Repository;
using ViewModels;
using WechatAppLib;
using HelperLib;

namespace Service
{
    public class WXMessageTemplateService
    {

        WechatUserService _WechatUserService = new WechatUserService();
        PartsCompanyService _PartsCompanyService = new PartsCompanyService();

        public bool WX_Message_Template_Send(string touser, string template_id, string data)
        {
            string msgid = WeChatServiceHelper.WX_Message_Template_Send(touser, template_id, data);
            return string.IsNullOrWhiteSpace(msgid) ? false : true;
        }

        /// <summary>
        /// 服务号 电话 推送
        /// </summary>
        /// <param name="wechatID">小程序 用户 id</param>
        /// <param name="partsCompanyID">配件商 id</param>
        /// <param name="touser">服务号 用户 openid</param>
        /// <param name="template_id">推送的模版消息id</param>
        /// <returns></returns>
        public bool WX_MessageTemplate_Tel_Push(long wechatID , long partsCompanyID , string touser , string template_id= "CwhLNFzWeJ7qmtxOH_Jz2QW1kHA_Yq_q7U1ME26vxrk")
        {
            WechatUserModel wechatUser = _WechatUserService.GetByID(wechatID);
            PartsCompanyModel _PartsCompanyModel = _PartsCompanyService.GetByID(partsCompanyID);
            object dataObj = new
            {
                first = new { value = "尊敬的修车必备认证商户，您即将接到来自平台修理厂用户的电话！", color = "#FF0000" },
                keyword1 = new { value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), color = "#173177" },
                keyword2 = new { value = "【"+wechatUser.NickName+"】" + "正在拨打您的电话,，请保持电话畅通，并提供专业、高效、优质的服务，祝生意兴隆！【" + _PartsCompanyModel.Name + "】", color = "#173177" },
                remark = new { value = "此功能仅限平台来电，不包含广告书和其他渠道！", color = "#173177" }
            };
            string data = TransformHelper.SerializeObject(dataObj);
            string msgid = WeChatServiceHelper.WX_Message_Template_Send(touser, template_id, data);
            return string.IsNullOrWhiteSpace(msgid) ? false : true;
        }

        /// <summary>
        /// 服务号  绑定会员推送
        /// </summary>
        /// <param name="wechatID">小程序 用户 id</param>
        /// <param name="touser">服务号 用户 openid</param>
        /// <param name="template_id">推送的模版消息id</param>
        /// <returns></returns>
        public bool WX_MessageTemplate_BindMember_Push(long wechatID, string touser, string template_id= "GCFutkEI-SDZWYu7gNNNX1bGHw93EW_yjMr7iCEaJCs")
        {
            WechatUserModel wechatUser = _WechatUserService.GetByID(wechatID);
            object dataObj = new
            {
                first = new { value = "将您的页面推荐给修理厂，系统会自动绑定您与修理厂用户之间的业务关系，从而优先显示您的信息！", color = "#1C86EE" },
                keyword1 = new { value = wechatUser.NickName, color = "#173177" },
                keyword2 = new { value = "绑定成功！", color = "#173177" },
                remark = new { value = "修车必备独有的[金矿系统]能源源不断的帮您开发新客户，稳固老客户！→详情请点击←", color = "#173177" }
            };
            string data = TransformHelper.SerializeObject(dataObj);
            string msgid = WeChatServiceHelper.WX_Message_Template_Send(touser, template_id, data);
            return string.IsNullOrWhiteSpace(msgid) ? false : true;
        }

    }
}
