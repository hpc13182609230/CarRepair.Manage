using HelperLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.CarRepair;
using WechatAppLib;


namespace CarRepairWeb.Controllers
{
    /// <summary>
    /// 后台 系统设置相关
    /// </summary>
    public class SystemController : BaseController
    {
        ManageUserService _ManageUserService = new ManageUserService();
        AreaService _AreaService = new AreaService();

        // GET: System
        public ActionResult Index()
        {
            return View();
        }


        public new ActionResult ManageUserList()
        {

            List<ManageUserModel> models = _ManageUserService.GetAll();
            ViewBag.models = models;
            return View();

        }

        public new ActionResult ManageUserDetail(long id=0)
        {
            ManageUserModel model = (id == 0 ? new ManageUserModel() : _ManageUserService.GetByID(id));
            //获取所有省份 
            List<AreaModel> provinces = _AreaService.GetListByParentID("0");
            ViewBag.provinces = provinces;
            ViewBag.model = model;
            return View();
        }

        public new ActionResult SaveManageUser(ManageUserModel model)
        {
            ManageUserModel _ManageUserModel = new ManageUserModel();
            if (model.ID>0)
            {
                _ManageUserModel = _ManageUserService.GetByID(model.ID);
                _ManageUserModel.AreaCodeID = model.AreaCodeID;
                _ManageUserModel.Permission = model.Permission;
                _ManageUserModel.LoginName = model.LoginName;
                _ManageUserModel.Openid = model.Openid;
            }
            else
            {
                _ManageUserModel = model;
                _ManageUserModel.Statu = 1;
                _ManageUserModel.UserName = _ManageUserModel.UserName;
                _ManageUserModel.EncryptKey = _ManageUserModel.LoginName;
                string pwd_encryption = EncryptHelper.ConvertToBase64(_ManageUserModel.LoginName+DateTime.Now.ToString("yyyyMMdd"));
                model.Password = EncryptHelper.Md5EncryptStr32((EncryptHelper.Md5EncryptStr32(pwd_encryption) + _ManageUserModel.EncryptKey));
            }

            var id = _ManageUserService.Save(_ManageUserModel);

            return Json(id, JsonRequestBehavior.AllowGet);
        }

    }
}