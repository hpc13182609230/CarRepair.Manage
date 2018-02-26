using HelperLib;
using LogLib;
using Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ViewModels.CarRepair;
using WechatAppLib;
using static WechatAppLib.WeChatServiceHelper;
namespace CarRepairWeb.Controllers
{
    public class TestController : Controller
    {
        ZTestService _ZTestService = new ZTestService();
        WXUserService _WXUserService = new WXUserService();
        WXMessageTemplateService _WXMessageTemplateService = new WXMessageTemplateService();
        WechatUserService _WechatUserService = new WechatUserService();
        PartsCompanyService _PartsCompanyService = new PartsCompanyService();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wechatID"> 小程序的 用户id</param>
        /// <param name="partsCompanyID">汽修厂的 id</param>
        /// <param name="touser">接受的 服务号的 用户  openid</param>
        /// <returns></returns>
        public ActionResult Index(long wechatID = 64, long partsCompanyID = 9, string touser = "oqtR-w03IY8eE1Nym5QtTaDQ--bs")
        {

            WechatUserModel wechatUser = _WechatUserService.GetByID(wechatID);
            PartsCompanyModel _PartsCompanyModel = _PartsCompanyService.GetByID(partsCompanyID);
            string template_id= "Y-ymHEfN0Iu2XjzvMTngn100CmtSMgFUg4OVlcgTuns";
            object dataObj = new {
                first = new { value = "电话预约成功!", color = "#173177" },
                productType = new { value = _PartsCompanyModel.Name, color = "#173177" },
                name = new { value = wechatUser.NickName, color = "#173177" },               
                time = new { value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), color = "#173177" },
                result = new { value = "已预约", color = "#173177" },
                remark = new { value = "欢迎您再次电话预约！", color = "#173177" }
            };
            string data = TransformHelper.SerializeObject(dataObj);
            _WXMessageTemplateService.WX_Message_Template_Send(touser, template_id,data);
            //var s =  _WXUserService.GetByUnionID("o8rVE1CllTtErR1YFsFRq7nITsrM");

            //ZTestModel model= _ZTestService.GetByID(1);
            //if (model!=null)
            //{
            //    model.ParentID = 1;
            //    _ZTestService.Save(model);
            //}
            return Json("",JsonRequestBehavior.AllowGet);
        }
    }
}