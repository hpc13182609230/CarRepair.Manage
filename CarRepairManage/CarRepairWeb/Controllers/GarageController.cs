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
using System.Data;

namespace CarRepairWeb.Controllers
{
    //  Garage
    public class GarageController : BaseController
    {
        GarageService _GarageService = new GarageService();

        // GET: Garage
        //配件商 列表
        public ActionResult GarageList(string keyword, DateTime startTime, DateTime? endTime, int pageIndex = 1, int pageSize = 10)
        {
            endTime = endTime ?? DateTime.Now.Date;
            PageInfoModel page = new PageInfoModel() { PageIndex = pageIndex, PageSize = pageSize };
            List<GarageModel> models = _GarageService.GetListByPage(keyword, startTime, Convert.ToDateTime(endTime).AddDays(1), ref page);

            ViewBag.page = page;
            ViewBag.keyword = keyword;
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime;
            ViewBag.models = models;

            return View();
        }

        public new ActionResult GarageDetail(long id = 0)
        {
            GarageModel model = (id == 0 ? new GarageModel() : _GarageService.GetByID(id));
            //获取所有省份 
            //List<AreaModel> provinces = _AreaService.GetListByParentID("0");
            //ViewBag.provinces = provinces;
            ViewBag.model = model;
            return View();
        }


        //保存 修理厂
        public ActionResult GarageSave(GarageModel model)
        {
            DataResultModel result = new DataResultModel();
            var id = _GarageService.Save(model);
            if (id<=0)
            {
                result.result = 0;
                result.message = "服务器异常，请联系管理员";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="excel"></param>
        /// <returns></returns>
        public ActionResult ImportExcel(HttpPostedFileBase file)
        {
            //string Url_Path = @"D:\WechatProject\CarRepair.Manage\CarRepairManage\文件\修理厂信息表.xls";
            DataResultModel result = new DataResultModel();
            try
            {
                string Url_Path = UpLoad_Excel(file);
                DataSet garages = FileHelper.ExcelToDS(ConfigureHelper.Get("ImageSavePath") + Url_Path);
                if (garages != null)
                {
                    bool flag = _GarageService.ExportDataFromDataset(garages);
                }
                result.data = Url_Path;
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }



        #region private

        private string UpLoad_Excel(HttpPostedFileBase file)
        {
            string rootPath = ConfigureHelper.Get("ImageSavePath"); //HttpContext.Request.PhysicalApplicationPath;
            DateTime date = DateTime.Now;
            string directory = rootPath + "Excel\\" + date.ToString("yyyyMM");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string filename = directory + "\\" + date.ToString("yyyyMMddHHmmssffff") + file.FileName;
            file.SaveAs(filename);
            string Url_Show = "\\" + filename.Replace(rootPath, "");
            return Url_Show;
        }

        #endregion


    }
}