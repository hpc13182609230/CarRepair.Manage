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
    /// 服务号相关功能
    /// </summary>
    public class WechatController : BaseController
    {
        WXMenuService _WXMenuService = new WXMenuService();

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

            long id = _WXMenuService.Save(model);
            string WX_Menu_Json = WXMenu_UpdateToPublicNoMenu();
            return Json(id);
        }

        public ActionResult DeleteMenu(int id)
        {
            int flag = _WXMenuService.DeleteByID(id);
            string WX_Menu_Json = WXMenu_UpdateToPublicNoMenu();
            return Json(flag);
        }

        private string WXMenu_UpdateToPublicNoMenu()
        {
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

        #region 图文推送
        public ActionResult WXNews(int index = 0)
        {
            List<WXNewsModel> models = new List<WXNewsModel>();
            #region 同步图文
            List<JToken> news = new List<JToken>();
            //news = WeiXinHelper.WX_API_BatchGet_Material_Page(MediaType, offset, count);
            news = WeChatServiceHelper.WX_API_BatchGet_Material("news");

            #region 图文的返回格式
            /*
             {
              "media_id": "KPoE9Hzq37QRhyQ4Nip86_8nSK1TiC8ZxlLEZvuass4",
              "content": {
                "news_item": [
                  {
                    "title": "测试 开发模式下的 群发功能",
                    "author": "hpc",
                    "digest": "测试 开发模式下的 群发功能[编辑]",
                    "content": "<p>测试 开发模式下的 群发功能[编辑]</p>",
                    "content_source_url": "http://www.baidu.com",
                    "thumb_media_id": "KPoE9Hzq37QRhyQ4Nip8633iZR5XydbrdBWtyE1jJ2A",
                    "show_cover_pic": 1,
                    "url": "http://mp.weixin.qq.com/s?__biz=MzAwMTg2MDg1Ng==&mid=100000006&idx=1&sn=e6c0dc00b4a5953139f6e9a855de32de#rd",
                    "thumb_url": "http://mmbiz.qpic.cn/mmbiz/l6V1ZR8yGZynhHDJ8sSQZibfNRf4LLFjob1ibz85TvEPJmgy7CsD19uiab6xdDhoZB8T9ibU7IHbY8y1DHiafdRjnug/0?wx_fmt=jpeg"
                  }
                ],
                "create_time": 1464834391,
                "update_time": 1464834556
              },
              "update_time": 1464834556
             }
          */
            #endregion

            foreach (var item in news)
            {
                WXNewsModel model = new WXNewsModel();
                model.Articles = new List<WXArticleModel>();

                model.NewsID = item["media_id"].ToString();//整个图文的id【对应WXArticle的 newsID】

                string content = item["content"].ToString();
                JObject jb_content = JsonConvert.DeserializeObject(content) as JObject;
                JArray jb_Array = JsonConvert.DeserializeObject(jb_content["news_item"].ToString()) as JArray;
                List<JToken> jb_List_Articles = jb_Array.ToList();
                DateTime create = TransformHelper.Convert_TimeStamp2DateTime(jb_content["create_time"].ToString());
                DateTime update = TransformHelper.Convert_TimeStamp2DateTime(jb_content["update_time"].ToString());

                List<int> articleIDs = new List<int>();
                foreach (var article in jb_List_Articles)
                {
                    WXArticleModel _WXArticleModel = new WXArticleModel();

                    _WXArticleModel.NewsID = model.NewsID;
                    _WXArticleModel.CreateTime = create;
                    _WXArticleModel.UpdateTime = update;
                    _WXArticleModel.Title = article["title"].ToString();
                    _WXArticleModel.Author = article["author"].ToString();
                    _WXArticleModel.Digests = article["digest"].ToString();
                    _WXArticleModel.Content = article["content"].ToString();
                    _WXArticleModel.Content_Source_Url = article["content_source_url"].ToString();
                    _WXArticleModel.Thumb_Media_ID = article["thumb_media_id"].ToString();
                    _WXArticleModel.Show_Cover_Pic = article["show_cover_pic"].ToString();
                    _WXArticleModel.Url = article["url"].ToString();
                    _WXArticleModel.Thumb_Url = article["thumb_url"].ToString();

                    model.Articles.Add(_WXArticleModel);
                }
                models.Add(model);
            }
            #endregion
            ViewBag.models = models;
            return View();
        }

        public ActionResult WXNewsPush(string NewsID)
        {
            //List<string> OpenIDs = new List<string>() { "oqtR-w5-J7_sSDGuB9KTOHxwLuuQ", "oqtR-w03IY8eE1Nym5QtTaDQ--bs" };
            List<string> OpenIDs = WeChatServiceHelper.WX_API_User_GetAll();
            WeChatServiceHelper.WX_SendMSG_OpenID(OpenIDs,WeChatServiceHelper.WX_MessageType.mpnews, NewsID);
            return Json(null,JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}