using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace CarRepairWeb.Controllers
{
    /// <summary>
    /// metronic 模板 相关
    /// </summary>
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
        /// <summary>
        /// 表单 提交相关的样式
        /// </summary>
        /// <returns></returns>
        public ActionResult FormComponents()
        {
            return View();
        }
        /// <summary>
        /// 时间轨迹页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Page_TimeLine()
        {
            return View();
        }
        /// <summary>
        /// 个人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult User_Profile()
        {
            return View();
        }
        /// <summary>
        /// 搜索 列表展示 以及 分页
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchAndList()
        {
            return View();
        }
        public ActionResult Table_Editable()
        {
            return View();
        }
        public ActionResult Table_Advance()
        {
            return View();
        }
        public ActionResult VisualCharts()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }


        public ActionResult  Error()
        {
            return View();
        }
    }
}