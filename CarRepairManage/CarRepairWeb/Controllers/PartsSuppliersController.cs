using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service;
using ViewModels;
using ViewModels.CarRepair;
using System.IO;
using HelperLib;
using LogLib;

namespace CarRepairWeb.Controllers
{
    //配件商 
    public class PartsSuppliersController : BaseController
    {
        //配件商 列表
        public ActionResult PartsList(string keyword,DateTime startTime,DateTime endTime, int pageIndex = 1, int pageSize = 10)
        {
            PageInfoModel page = new PageInfoModel() { PageIndex = pageIndex, PageSize = pageSize };

            BaseOptionsService _BaseOptionsService = new BaseOptionsService();
            List<BaseOptions> options = _BaseOptionsService.GetByParentID(1);
            PartsCompanyService service = new PartsCompanyService();
            List<PartsCompany> partsCompanys = service.GetListByPage(keyword,startTime, endTime.AddDays(1), ref page);

            ViewBag.page = page;
            ViewBag.keyword = keyword;
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime;
            ViewBag.options = options;
            ViewBag.partsCompanys = partsCompanys;

            return View();
        }

        //配件商 单个详情
        public ActionResult PartDetail(int id=0)
        {
            PartsCompanyService service = new PartsCompanyService();
            PartsCompany model =id==0?new PartsCompany() : service.GetByID(id);

            //分类相关
            BaseOptionsService _BaseOptionsService = new BaseOptionsService();
            List<BaseOptions> options = _BaseOptionsService.GetByParentID(1);
            //PartsClassifyCompanyService _PartsClassifyCompanyService = new PartsClassifyCompanyService();
            //PartsClassifyCompanyModel PartsClassifyCompany = _PartsClassifyCompanyService.GetByPartsCompanyID(id);

            ViewBag.options = options;
            //ViewBag.PartsClassifyCompany = PartsClassifyCompany;

            ViewBag.PartsCompany = model;
            return View();
        }

        //配件商 单个保存
        public ActionResult PartsCompanySave(PartsCompany model)
        {
            model.Content = EncryptHelper.UrlDecode(model.Content);

            PartsCompanyService service = new PartsCompanyService();
            var id =  service.SavePartsCompany(model);
           

            return Json(id,JsonRequestBehavior.AllowGet);
        }

        //删除配件商
        public ActionResult DeletePartsCompany(long ID)
        {
            PartsCompanyService service = new PartsCompanyService();
            bool flag = service.DeleteByID(ID);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        //配件商的分类 获取 初始化
        //public ActionResult PartsCompanyClassifyGets(long PartsCompanyID)
        //{
        //    PartsClassifyCompanyService service = new PartsClassifyCompanyService();
        //    var model = service.GetByPartsCompanyID(PartsCompanyID);
        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}

        //配件商的分类  保存
        //public ActionResult PartsCompanyClassifySave(PartsClassifyCompanyModel model)
        //{
        //    PartsClassifyCompanyService service = new PartsClassifyCompanyService();
        //    var id = service.Save(model);
        //    return Json(id, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult Upload(HttpPostedFileBase upImg)
        {
            string Url_Path = "";
            string fileName = System.IO.Path.GetFileName(upImg.FileName);
            Url_Path = UpLoad_Image(upImg);
            return Json(new { Url_Path = Url_Path }, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //public ActionResult Upload(HttpPostedFileBase upImg)
        //{
        //    string Url_Path = "";
        //    try
        //    {
        //        Tracer.RunLog(MessageType.WriteInfomation, "", "log", "Upload start" + "\r\n");
        //        string fileName = System.IO.Path.GetFileName(upImg.FileName);
        //        Url_Path = UpLoad_Image(upImg);
        //    }
        //    catch (Exception ex)
        //    {
        //        Url_Path = ex.Message;
        //        Tracer.RunLog(MessageType.WriteInfomation, "", "log", "Upload 异常 = ：" + ex + "\r\n");
        //    }
        //    return Json(new { Url_Path = Url_Path }, JsonRequestBehavior.AllowGet);
        //}

        //将File文件保存到本地，返回物理的相对地址
        private string UpLoad_Image(HttpPostedFileBase file)
        {
            string rootPath = ConfigureHelper.Get("ImageSavePath"); //HttpContext.Request.PhysicalApplicationPath;
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