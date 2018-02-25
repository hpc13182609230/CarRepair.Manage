using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels;
using ViewModels.CarRepair;

namespace CarRepairWeb.Controllers
{
    public class UserController : Controller
    {

        WXUserService _WXUserService = new WXUserService();
        WechatUserService _WechatUserService = new WechatUserService();

        // GET: User
        #region 小程序 用户
        public ActionResult WechatUserList(string keyword, DateTime startTime, DateTime endTime, int pageIndex = 1, int pageSize = 10)
        {
            PageInfoModel page = new PageInfoModel() { PageIndex = pageIndex, PageSize = pageSize };

            List<WechatUserModel> models = _WechatUserService.GetListByPage(keyword, startTime, endTime.AddDays(1), ref page);
            ViewBag.models = models;

            ViewBag.keyword = keyword;
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime;
            ViewBag.Page = page;

            return View();
        }
        #endregion


        #region 服务号 用户
        public ActionResult WXUserList(string keyword, DateTime startTime, DateTime endTime, int pageIndex = 1, int pageSize = 10)
        {
            PageInfoModel page = new PageInfoModel() { PageIndex = pageIndex, PageSize = pageSize };

            List<WXUserModel> models = _WXUserService.GetListByPage(keyword, startTime, endTime.AddDays(1), ref page);
            ViewBag.models = models;

            ViewBag.keyword = keyword;
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime;
            ViewBag.Page = page;

            return View();
        }
        #endregion
    }
}