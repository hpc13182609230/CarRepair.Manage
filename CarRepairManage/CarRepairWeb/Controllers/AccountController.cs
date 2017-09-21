using HelperLib;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels;

namespace CarRepairWeb.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            if (Request.Cookies["UserName"] != null)
            {
                ViewBag.UserName = Request.Cookies["UserName"].Value;
            }
            return View();
        }

        public ActionResult LoginFunction()
        {
            DataResultModel _DataResultModel = new DataResultModel();

            ManageUserService _UserInfoService = new ManageUserService();
            string errorMsg = "";
            //验证码验证
            string uname = (Request["username"] ?? "").Trim();
            string pwd = (Request["password"] ?? "").Trim();
            string pwd_encryption = EncryptHelper.ConvertToBase64(pwd);
            if (string.IsNullOrWhiteSpace(uname))
            {
                _DataResultModel.result = 0;
                _DataResultModel.message = "用户名不能为空！";
                return Json(_DataResultModel,JsonRequestBehavior.AllowGet);
            }
            if (!RegexHelper.IsSafe(uname))
            {
                _DataResultModel.result = 0;
                _DataResultModel.message = "用户名非法！";
                return Json(_DataResultModel, JsonRequestBehavior.AllowGet);
            }


            ManageUserModel _UserInfoModel = _UserInfoService.UserInfo_CheckLogin(uname, pwd_encryption, ref errorMsg);
            if (_UserInfoModel != null)
            {
                #region 验证成功，更新最后登录时间 ,保存到cookie和session中
                _UserInfoService.UserInfo_Update_LastLoginTime(_UserInfoModel.id);
                var cookie = new HttpCookie("UserName", _UserInfoModel.LoginName);
                cookie.Expires = DateTime.Now.AddMonths(1);
                Response.Cookies.Add(cookie);

                Response.Cookies["AccessUserID"].Expires = DateTime.MaxValue;
                Response.Cookies["AccessUserID"].Value = _UserInfoModel.id.ToString();

                Utility.UserUtility.Current.SetLogin(_UserInfoModel.id, _UserInfoModel.LoginName);

                //_DataResultModel.data = _UserInfoModel;

                #endregion
                return Json(_DataResultModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _DataResultModel.result = 0;
                _DataResultModel.message = errorMsg;
                return Json(_DataResultModel, JsonRequestBehavior.AllowGet);
            }
        }



    }
}