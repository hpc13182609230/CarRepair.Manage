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
using CarRepairWeb.Models.RequestModel;

namespace CarRepairWeb.Controllers
{
    /// <summary>
    ///  配件商 相关
    /// </summary>
    public class PartsSuppliersController : BaseController
    {
        BaseOptionsService _BaseOptionsService = new BaseOptionsService();
        PartsCompanyService service = new PartsCompanyService();
        PartsCallRecordService _PartsCallRecordService = new PartsCallRecordService();
        AreaService _AreaService = new AreaService();
        DateTime current = DateTime.Now.Date;
        VehicleTypeService _VehicleTypeService = new VehicleTypeService();

        //配件商 列表
        public ActionResult PartsList(string keyword, DateTime startTime,DateTime? endTime, string codeID = "370000", int pageIndex = 1, int pageSize = 10)
        {
            PageInfoModel page = new PageInfoModel() { PageIndex = pageIndex, PageSize = pageSize };
            endTime = endTime ?? DateTime.Now.Date;
            List<BaseOptionsModel> options = _BaseOptionsService.GetByParentID(1);
            List<PartsCompanyModel> partsCompanys = service.GetListByPage(keyword,codeID,startTime,Convert.ToDateTime(endTime).AddDays(1), ref page);
            List<AreaModel> provinces = _AreaService.GetListByParentID("0");

            ViewBag.provinces = provinces;
            ViewBag.page = page;
            ViewBag.keyword = keyword;
            ViewBag.codeID = codeID;
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime;
            ViewBag.options = options;
            ViewBag.partsCompanys = partsCompanys;

            return View();
        }

        //配件商 单个详情
        public ActionResult PartDetail(int id=0)
        {
            PartsCompanyModel model =(id==0?new PartsCompanyModel() : service.GetByID(id));

            //分类相关2
            List<BaseOptionsModel> options = _BaseOptionsService.GetByParentID(1);
            //PartsClassifyCompanyService _PartsClassifyCompanyService = new PartsClassifyCompanyService();
            //PartsClassifyCompanyModel PartsClassifyCompany = _PartsClassifyCompanyService.GetByPartsCompanyID(id);

            //获取所有省份 
            List<AreaModel> provinces = _AreaService.GetListByParentID("0"); 
            ViewBag.provinces = provinces;
            ViewBag.options = options;
            //ViewBag.PartsClassifyCompany = PartsClassifyCompany;

            ViewBag.PartsCompany = model;
            return View();
        }

        //配件商 单个保存
        public ActionResult PartsCompanySave(PartsCompanyRequestModel model)
        {
            model.Content = EncryptHelper.UrlDecode(model.Content);
            model.Address = model.Address ?? "";

            PartsCompanyService service = new PartsCompanyService();
            PartsCompanyModel _PartsCompanyModel = new PartsCompanyModel();
            TransformHelper.ConvertBToA(_PartsCompanyModel, model);
            var id =  service.Save(_PartsCompanyModel);
           

            return Json(id,JsonRequestBehavior.AllowGet);
        }

        //删除配件商
        public ActionResult DeletePartsCompany(long ID)
        {
            PartsCompanyService service = new PartsCompanyService();
            int flag = service.DeleteByID(ID);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        

        //重新排序
        public ActionResult ResetCompanyOrder(string codeID)
        {
            string msg= service.ResetCompanyOrder(codeID);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        //配件商的 通话记录
        public ActionResult CallRecordList(string keyword, DateTime startTime, DateTime? endTime,  int pageIndex = 1, int pageSize = 10)
        {
            PageInfoModel page = new PageInfoModel() { PageIndex = pageIndex, PageSize = pageSize };
            endTime = endTime ?? DateTime.Now.Date;
            List<PartsCallRecordModel> models = _PartsCallRecordService.GetListByPage(keyword, startTime, Convert.ToDateTime(endTime).AddDays(1), ref page);

            ViewBag.page = page;
            ViewBag.keyword = keyword;
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime;
            ViewBag.models = models;
            return View();
        }


        #region 上传素材

        /// <summary>
        /// 上次 图片
        /// </summary>
        /// <param name="upImg"></param>
        /// <returns></returns>
        public ActionResult Upload(HttpPostedFileBase upImg)
        {
            string Url_Path = "";
            string fileName = System.IO.Path.GetFileName(upImg.FileName);
            Url_Path = UpLoad_Image(upImg);
            return Json(new { Url_Path = Url_Path }, JsonRequestBehavior.AllowGet);
        }

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
        #endregion

    }
}