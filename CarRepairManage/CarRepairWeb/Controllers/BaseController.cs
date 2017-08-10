using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRepairWeb.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //验证session
            CheckLogin(filterContext);
            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        private void CheckLogin(ActionExecutingContext filterContext)
        {
            int id = Utility.UserUtility.Current.AccountID;
            if (id == 0)
            {
                filterContext.Result = RedirectToRoute("Default", new { Controller = "Account", Action = "Login" });
            }

        }
    }
}