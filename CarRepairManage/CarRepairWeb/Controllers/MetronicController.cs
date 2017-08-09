using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace CarRepairWeb.Controllers
{
    public class MetronicController : Controller
    {
        // GET: metronic
        /// <summary>
        /// 后台管理网站的首页 仪表板 页面 【统计页面】
        /// </summary>
        /// <returns></returns>
        public ActionResult Dashboard()
        {
            return View();
        }
        /// <summary>
        /// 宣传页面 【对外使用】 【导航栏 使用横向布局】
        /// </summary>
        /// <returns></returns>
        public ActionResult Promo()
        {
            return View();
        }
        /// <summary>
        /// 一些通用的组件
        /// </summary>
        /// <returns></returns>
        public ActionResult GeneralComponents()
        {
            return View();
        }
        /// <summary>
        /// 按钮
        /// </summary>
        /// <returns></returns>
        public ActionResult Button()
        {
            return View();
        }
        public ActionResult FormComponents()
        {
            return View();
        }
    }
}