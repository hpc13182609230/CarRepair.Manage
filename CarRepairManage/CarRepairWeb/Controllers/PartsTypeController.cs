using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.CarRepair;
using Service;

namespace CarRepairWeb.Controllers
{
    public class PartsTypeController : Controller
    {
        // GET: PartsType
        public ActionResult PartsClassify(long OptionID,string name)
        {
            PartsClassifyService service = new PartsClassifyService();
            List<PartsClassifyModel> _PartsClassifyModels = service.GetByParentID(OptionID);
            ViewBag.PartsClassify = _PartsClassifyModels;
            ViewBag.name = name;
            ViewBag.OptionID = OptionID;
            return View();
        }

        public ActionResult AddPartsClassify(PartsClassifyModel model)
        {
            PartsClassifyService service = new PartsClassifyService();
            long id= service.Save(model);
            return Json(id,JsonRequestBehavior.AllowGet);
        }


        public ActionResult PartsClassifyCompany(int PartsClassifyID,string Content)
        {
            //PartsClassifyService service = new PartsClassifyService();
            //List<PartsClassifyModel> _PartsClassifyModels = service.GetByParentID(OptionID);
            //ViewBag.PartsClassify = _PartsClassifyModels;
            ViewBag.PartsClassifyID = PartsClassifyID;
            ViewBag.Content = Content;
            return View();
        }
    }
}