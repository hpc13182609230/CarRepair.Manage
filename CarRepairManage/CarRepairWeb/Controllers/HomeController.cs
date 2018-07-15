using HelperLib;
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
    public class HomeController :BaseController
    {

        public ActionResult Index()
        {
            return View();
        }
    }
}
