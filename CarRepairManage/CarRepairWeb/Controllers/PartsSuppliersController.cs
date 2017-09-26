using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service;
using ViewModels;
using ViewModels.CarRepair;
using System.IO;

namespace CarRepairWeb.Controllers
{
    //配件商 
    public class PartsSuppliersController : Controller
    {
        //配件商 列表
        public ActionResult PartsList()
        {
            BaseOptionsService _BaseOptionsService = new BaseOptionsService();
            List<BaseOptionsModel> options = _BaseOptionsService.GetByParentID(1);
            PartsCompanyService service = new PartsCompanyService();
            PageInfoModel page = new PageInfoModel();
            List<PartsCompanyModel> partsCompanys = service.GetListByPage(page);
            ViewBag.options = options;
            ViewBag.partsCompanys = partsCompanys;
            return View();
        }

        //配件商 单个详情
        public ActionResult PartDetail(int id=0)
        {
            PartsCompanyService service = new PartsCompanyService();
            PartsCompanyModel model =id==0?new PartsCompanyModel() : service.GetByID(id);
            ViewBag.PartsCompany = model;
            return View();
        }

        //配件商 单个保存
        public ActionResult PartsCompanySave(PartsCompanyModel model)
        {
            PartsCompanyService service = new PartsCompanyService();
            var id =  service.SavePartsCompany(model);
            return Json(id,JsonRequestBehavior.AllowGet);
        }

        //配件商的分类 获取 初始化
        public ActionResult PartsCompanyClassifyGets(long PartsCompanyID)
        {
            PartsClassifyCompanyService service = new PartsClassifyCompanyService();
            var model = service.GetByPartsCompanyID(PartsCompanyID);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //配件商的分类  保存
        public ActionResult PartsCompanyClassifySave(PartsClassifyCompanyModel model)
        {
            PartsClassifyCompanyService service = new PartsClassifyCompanyService();
            var id = service.Save(model);
            return Json(id, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upImg)
        {
            string fileName = System.IO.Path.GetFileName(upImg.FileName);
            string Url_Show = UpLoad_Image(upImg);
            return Json(Url_Show, JsonRequestBehavior.AllowGet);
        }

        //将File文件保存到本地，返回物理的相对地址
        private string UpLoad_Image(HttpPostedFileBase file)
        {
            string rootPath = HttpContext.Request.PhysicalApplicationPath;
            DateTime date = DateTime.Now;
            string directory = rootPath + "Image\\" + date.ToString("yyyyMM");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string filename = directory + "\\" + date.ToString("yyyyMMddHHmmssffff") + ".jpg";
            file.SaveAs(filename);
            string Url_Show = "\\" + filename.Replace(rootPath, "");
            return Url_Show;
        }


    }
}