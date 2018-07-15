using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels;
using ViewModels.CarRepair;
using WechatAppLib;
using static WechatAppLib.WeChatServiceHelper;

namespace CarRepairWeb.Controllers
{
    /// <summary>
    /// 用户 相关
    /// </summary>
    public class UserController : BaseController
    {

        WXUserService _WXUserService = new WXUserService();
        WechatUserService _WechatUserService = new WechatUserService();

        // GET: User
        #region 小程序 用户
        public ActionResult WechatUserList(string keyword, DateTime startTime, DateTime? endTime, int pageIndex = 1, int pageSize = 10)
        {
            PageInfoModel page = new PageInfoModel() { PageIndex = pageIndex, PageSize = pageSize };
            endTime = endTime ?? DateTime.Now.Date;

            List<WechatUserModel> models = _WechatUserService.GetListByPage(keyword, startTime, Convert.ToDateTime(endTime).AddDays(1), ref page);
            ViewBag.models = models;

            ViewBag.keyword = keyword;
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime;
            ViewBag.Page = page;

            return View();
        }
        #endregion


        #region 服务号 用户
        public ActionResult WXUserList(string keyword, DateTime startTime, DateTime? endTime, int pageIndex = 1, int pageSize = 10)
        {
            PageInfoModel page = new PageInfoModel() { PageIndex = pageIndex, PageSize = pageSize };
            endTime = endTime ?? DateTime.Now.Date;

            List<WXUserModel> models = _WXUserService.GetListByPage(keyword, startTime, Convert.ToDateTime(endTime).AddDays(1), ref page);
            ViewBag.models = models;

            ViewBag.keyword = keyword;
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime;
            ViewBag.Page = page;

            return View();
        }

        //
        public ActionResult Sync_WXUser()
        {

            List<string> openids = WeChatServiceHelper.WX_API_User_GetAll();
            foreach (var item in openids)
            {
                WX_BaseModel_UserInfo userInfo = WeChatServiceHelper.WX_API_GetUserInfo_ByOpenID(item);//根据openid获取用户的基本信息
                #region 用户信息验证 [获取M站的登录token,更新用户的信息]

                //获取数据库 用户信息
                WXUserModel _WXUserModel = GetUserinfo(userInfo);

                //用户登录信息 存储和更新
                long id = _WXUserService.Save(_WXUserModel);
                #endregion
            }
            return Json(null,JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region private
        private WXUserModel GetUserinfo(WX_BaseModel_UserInfo userInfo)
        {
            WXUserModel model = new WXUserModel();
            model.Openid = userInfo.Openid;
            model.Unionid = userInfo.Unionid;
            model.NickName = userInfo.NickName;
            model.Sex = userInfo.Sex;
            model.Language = userInfo.Language;
            model.City = userInfo.City;
            model.Country = userInfo.Country;
            model.Province = userInfo.Province;
            model.Headimgurl = userInfo.Headimgurl;
            model.Subscribe_time = userInfo.Subscribe_time;
            model.Groupid = userInfo.Groupid;
            model.Tagid_list = userInfo.Tagid_list;
            model.LastActiveTime = DateTime.Now;
            return model;
        }
        #endregion
    }
}