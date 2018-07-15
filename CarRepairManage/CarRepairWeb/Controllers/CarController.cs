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
    /// <summary>
    ///  车 相关
    /// </summary>
    public class CarController : BaseController
    {
        DateTime current = DateTime.Now.Date;
        VehicleTypeService _VehicleTypeService = new VehicleTypeService();

        #region 车型分类

        // 列表 
        public ActionResult VehicleTypeList( DateTime startTime, DateTime? endTime, string keyword = "", int pageIndex = 1, int pageSize = 10)
        {
            PageInfoModel page = new PageInfoModel() { PageIndex = pageIndex, PageSize = pageSize };
            endTime = endTime ?? DateTime.Now.Date;
            List<VehicleTypeModel> models = _VehicleTypeService.GetListByPage(keyword, startTime, Convert.ToDateTime(endTime).AddDays(1), ref page);
            ViewBag.page = page;
            ViewBag.keyword = keyword;
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime;
            ViewBag.Models = models;


            return View();
        }

        //详情 
        public ActionResult VehicleTypeDetail(int id = 0)
        {
            VehicleTypeModel model = (id == 0 ? new VehicleTypeModel() : _VehicleTypeService.GetByID(id));

            ViewBag.Model = model;
            return View();
        }

        //保存 
        public ActionResult SaveVehicleType(VehicleTypeModel model)
        {
            long id = 0;
            PingYinModel _PingYinModel = TransformHelper.GetTotalPingYin(model.Name);
            model.Name_PY = _PingYinModel.TotalPingYin.FirstOrDefault();
            model.Name_FC = _PingYinModel.FirstPingYin.FirstOrDefault()[0].ToString();
            id = _VehicleTypeService.Save(model);
            return Json(id, JsonRequestBehavior.AllowGet);
        }

        //删除  
        public ActionResult DeleteVehicleType(long ID)
        {
            int flag = _VehicleTypeService.DeleteByID(ID);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}