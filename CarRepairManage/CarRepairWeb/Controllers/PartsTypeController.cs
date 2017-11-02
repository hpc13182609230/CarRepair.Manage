using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.CarRepair;
using Service;
using ViewModels;

namespace CarRepairWeb.Controllers
{
    public class PartsTypeController : Controller
    {
        // GET: PartsType
        //子分类列表
        public ActionResult PartsClassify(long OptionID,string name, string keyword, DateTime startTime, DateTime endTime, int pageIndex = 1, int pageSize = 10)
        {
            PageInfoModel page = new PageInfoModel() { PageIndex = pageIndex, PageSize = pageSize };

            PartsClassifyService service = new PartsClassifyService();
            List<PartsClassifyModel> _PartsClassifyModels = service.GetByParentIDPage(OptionID,keyword,startTime, endTime.AddDays(1), ref page);

            ViewBag.PartsClassify = _PartsClassifyModels;
            ViewBag.name = name;
            ViewBag.OptionID = OptionID;

            ViewBag.page = page;
            ViewBag.keyword = keyword;
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime;

            return View();
        }

        //子分类 详情
        //public ActionResult PartsClassify(long PartsClassify)
        //{
        //    PartsClassifyService service = new PartsClassifyService();
        //    List<PartsClassifyModel> _PartsClassifyModels = service.GetByParentID(OptionID);
        //    ViewBag.PartsClassify = _PartsClassifyModels;
        //    ViewBag.name = name;
        //    ViewBag.OptionID = OptionID;
        //    return View();
        //}


        public ActionResult AddPartsClassify(PartsClassifyModel model)
        {
            PartsClassifyService service = new PartsClassifyService();
            long id= service.Save(model);
            return Json(id,JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeletePartsClassify(long PartsClassifyID)
        {
            PartsClassifyService service = new PartsClassifyService();
            int flag = service.DeleteByID(PartsClassifyID);
            return Json(PartsClassifyID, JsonRequestBehavior.AllowGet);
        }


        //public ActionResult PartsClassifyCompany(long PartsClassifyID,string Content)
        //{

        //    PartsClassifyCompanyService service = new PartsClassifyCompanyService();
   
        //    List<PartsCompanyModel> partsCompanys = service.GetForAPI(PartsClassifyID);
        //    ViewBag.partsCompanys = partsCompanys;
        //    ViewBag.PartsClassifyID = PartsClassifyID;
        //    ViewBag.Content = Content;

        //    BaseOptionsService _BaseOptionsService = new BaseOptionsService();
        //    List<BaseOptionsModel> options = _BaseOptionsService.GetByParentID(1);
        //    ViewBag.options = options;

        //    return View();
        //}
    }
}