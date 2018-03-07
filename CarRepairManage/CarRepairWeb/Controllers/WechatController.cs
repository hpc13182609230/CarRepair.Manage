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
    /// 服务号相关功能
    /// </summary>
    public class WechatController : BaseController
    {
        #region 自定义菜单

        public ActionResult Menu(int index = 0)
        {
            WXMenuService _WXMenuService = new WXMenuService();
            List<WXMenuModel> WXMenuModels = _WXMenuService.WXMenu_GetAll();
            ViewBag.WXMenuModels = WXMenuModels;
            ViewBag.Index = index;
            return View();
        }

        public ActionResult SaveMenu(WXMenuModel model)
        {
            WXMenuService _WXMenuService = new WXMenuService();
            long id = _WXMenuService.Save(model);
            string WX_Menu_Json = WXMenu_UpdateToPublicNoMenu();
            return Json(id);
        }

        public ActionResult DeleteMenu(int id)
        {
            WXMenuService _WXMenuService = new WXMenuService();
            int flag = _WXMenuService.DeleteByID(id);
            string WX_Menu_Json = WXMenu_UpdateToPublicNoMenu();
            return Json(flag);
        }

        private string WXMenu_UpdateToPublicNoMenu()
        {
            WXMenuService _WXMenuService = new WXMenuService();
            string WX_Menu_Json = "";
            List<WXMenuModel> WXMenuModels = _WXMenuService.WXMenu_GetAll();
            List<WXMenuModel> WXMenu_Firsts = WXMenuModels.Where(p => p.ParentID == 0).ToList();
            if (WXMenu_Firsts.Count == 0)
            {
                return WX_Menu_Json;
            }

            //模板
            //{"button":[{"type":"click","name":"今日歌曲","key":"V1001_TODAY_MUSIC"},{"name":"菜单","sub_button":[{"type":"view","name":"搜索","url":"http://www.soso.com/"},{"type":"view","name":"视频","url":"http://v.qq.com/"},{"type":"click","name":"赞一下我们","key":"V1001_GOOD"}]}]}
            WX_Menu_Json += "{\"button\":[";
            int i = 0;
            foreach (var first in WXMenu_Firsts)
            {
                i++;
                List<WXMenuModel> WXMenu_First_Childs = WXMenuModels.Where(p => p.ParentID == first.ID).ToList();
                if (WXMenu_First_Childs.Count == 0)//一级菜单没有子菜单
                {
                    WX_Menu_Json += "{\"type\":\"" + first.MenuType + "\",\"name\":\"" + first.Name + "\",";
                    if (first.MenuType == "click")
                    {
                        WX_Menu_Json += "\"key\":\"" + first.KeyForClick + "\"},";
                    }
                    else if (first.MenuType == "view")
                    {
                        WX_Menu_Json += "\"url\":\"" + first.Url + "\"},";
                    }
                    else if (first.MenuType == "miniprogram")
                    {
                        WX_Menu_Json += "\"url\":\"" + "http://mp.weixin.qq.com" + "\",";
                        WX_Menu_Json += "\"appid\":\"" + first.KeyForClick + "\",";
                        WX_Menu_Json += "\"pagepath\":\"" + first.Url + "\"},";
                    }
                }
                else//一级菜单有子菜单
                {
                    WX_Menu_Json += "{\"name\":\"" + first.Name + "\",";
                    WX_Menu_Json += "\"sub_button\":[";
                    int j = 0;
                    foreach (var child in WXMenu_First_Childs)
                    {
                        j++;
                        WX_Menu_Json += "{\"type\":\"" + child.MenuType + "\",\"name\":\"" + child.Name + "\",";
                        if (child.MenuType == "click")
                        {
                            WX_Menu_Json += "\"key\":\"" + child.KeyForClick + "\"},";
                        }
                        else if (child.MenuType == "view")
                        {
                            WX_Menu_Json += "\"url\":\"" + child.Url + "\"},";
                        }
                        else if (child.MenuType == "miniprogram")
                        {
                            WX_Menu_Json += "\"url\":\"" + "http://mp.weixin.qq.com" + "\",";
                            WX_Menu_Json += "\"appid\":\"" + child.KeyForClick.Trim() + "\",";
                            WX_Menu_Json += "\"pagepath\":\"" + child.Url + "\"},";
                        }
                        if (j == WXMenu_First_Childs.Count)
                        {
                            WX_Menu_Json = WX_Menu_Json.Substring(0, WX_Menu_Json.Length - 1);
                        }
                    }

                    WX_Menu_Json += "]";
                    WX_Menu_Json += "},";
                }
            }
            if (i == WXMenu_Firsts.Count)
            {
                WX_Menu_Json = WX_Menu_Json.Substring(0, WX_Menu_Json.Length - 1);
            }
            WX_Menu_Json += "]}";

            //调用微信的接口，更新菜单
            string msg = WeChatServiceHelper.WX_API_Menu_Create(WX_Menu_Json);
            return WX_Menu_Json;
        }

        #endregion
    }
}