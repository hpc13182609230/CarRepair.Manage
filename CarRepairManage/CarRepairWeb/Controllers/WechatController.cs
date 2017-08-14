using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRepairWeb.Controllers
{
    public class WechatController : Controller
    {
        // GET: Wechat
        public ActionResult Menu()
        {
            return View();
        }
    }
}