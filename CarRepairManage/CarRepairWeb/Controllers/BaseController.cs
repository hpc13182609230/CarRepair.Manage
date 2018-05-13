using HelperLib;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using ViewModels;
using ViewModels.CarRepair;


namespace CarRepairWeb.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        ManageMenuService _ManageMenuService = new ManageMenuService();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //验证session
            CheckLogin(filterContext);
            //获取  用户的菜单 权限
            List<ManageMenuModel> menus = _ManageMenuService.GetAll();
            ViewBag.menus = menus;
            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        private void CheckLogin(ActionExecutingContext filterContext)
        {
            long id = Utility.UserUtility.Current.AccountID;
            if (id == 0)
            {
                filterContext.Result = RedirectToRoute("Default", new { Controller = "Account", Action = "Login" });
            }

        }
    }
}