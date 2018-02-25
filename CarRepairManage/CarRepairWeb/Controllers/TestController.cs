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
        // GET: Test
        public ActionResult Index()
        {
            ZTestModel model= _ZTestService.GetByID(1);
            if (model!=null)
            {
                model.ParentID = 1;
                _ZTestService.Save(model);
            }
            return View();
        }
    }
}