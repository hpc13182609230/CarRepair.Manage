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


        /// <summary>
        /// 上次Excel
        /// </summary>
        /// <param name="excel"></param>
        /// <returns></returns>
        public ActionResult ImportExcel(HttpPostedFileBase file)
        {
            string Url_Path = @"E:\soft\wps\修理厂信息表.xls";
            DataSet garages = FileHelper.ExcelToDS(Url_Path);
            if (garages!=null)
            {
                bool flag = _GarageService.ExportDataFromDataset(garages);
            }

            //string fileName = System.IO.Path.GetFileName(file.FileName);
            //Url_Path = UpLoad_Image(excel);
            return Json(new { Url_Path = Url_Path }, JsonRequestBehavior.AllowGet);
        }




    }
}