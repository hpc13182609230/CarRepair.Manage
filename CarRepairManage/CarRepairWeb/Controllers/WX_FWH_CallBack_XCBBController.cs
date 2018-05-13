using HelperLib;
using LogLib;
using Newtonsoft.Json.Linq;
using Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ViewModels.CarRepair;
using ViewModels.Utility;
using WechatAppLib;
using static WechatAppLib.WeChatServiceHelper;

namespace CarRepairWeb.Controllers
{
    public class WX_FWH_CallBack_XCBBController : Controller
    {
        //
        // GET: /WX_CallBack_ZS/
        //
        // GET: /WX_CallBack_MHD/

        //微信授权 自动登录
        public ActionResult WxJump(string tgid, string url, string code)
        {
            ////LogHelper.Info("logger WxJump 1");
            //BLL_MHD _BLL_MHD = new BLL_MHD();
            //WX_BaseModel_UserInfo user = WeChatServiceHelper.WX_API_GetUserInfo(code);//信息不全
            //user = WeChatServiceHelper.WX_API_GetUserInfo_ByOpenID(user.Openid);//获取所有信息
            //Log.Tracer.RunLog(Log.MessageType.WriteInfomation, "", "log", "user = : " + TransformHelper.SerializeObject(user) + "\r\n");
            //user.Tgid = tgid;
            //MHD_UserInfo_Model _MHD_UserInfo_Model = _BLL_MHD.GetWebLoginToken(user);
            //Log.Tracer.RunLog(Log.MessageType.WriteInfomation, "", "log", "_MHD_UserInfo_Model = : " + TransformHelper.SerializeObject(_MHD_UserInfo_Model) + "\r\n");
            //user.Token = _MHD_UserInfo_Model.logintoken;
            //user.UserID = _MHD_UserInfo_Model.id;
            //Log.Tracer.RunLog(Log.MessageType.WriteInfomation, "", "log", "url = : " + url + "\r\n");
            //string res = _BLL_MHD.AutoLoginUrlRefactor("1", user.UserID.ToString(), user.Token, user.Tgid, url, false);
            //Log.Tracer.RunLog(Log.MessageType.WriteInfomation, "", "log", "res : " + res + "\r\n");
            ////return Json("<script>location.href='" + res + "'</script>",JsonRequestBehavior.AllowGet);
            //return Content("<script>location.href='" + res + "'</script>");
            return Content("");
        }

        public ActionResult Entrance()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            #region 初始化  读取配置文件
            WXUserService _WXUserService = new WXUserService();

            var request = HttpContext.ApplicationInstance.Context.Request;
            var response = HttpContext.ApplicationInstance.Context.Response;
            string httpMethod = request.HttpMethod.ToUpper();
            long timeOutThreshold = 500;
            string logName = MessageType.Information.ToString();
            string logErrorName = MessageType.Error.ToString();

            #endregion
            try
            {
                if (httpMethod == "GET")
                {
                    //get方式 只在第一次做接口验证的回调的时候验证
                    WeChatServiceHelper.WX_API_Check_CallBack_URL(request, response);
                    TimeOutLog("", "API-微信 Get", timeOutThreshold, ref watch);
                }
                else
                {
                    //post方式 实现业务逻辑 
                    #region 获取请求参数 以及 获取用户的基本信息【微信的】
                    Stream WX_InputStream_Stream = request.InputStream;
                    string WX_InputStream_Json = TransformHelper.Convert_Stream2Json(WX_InputStream_Stream);
                    WX_InputStream_Json = TransformHelper.Json_Remove_CData(WX_InputStream_Json);
                    string ToUserName = WeChatServiceHelper.GetJsonValue_JObject(WX_InputStream_Json, "ToUserName");//开发者微信号
                    string FromUserName = WeChatServiceHelper.GetJsonValue_JObject(WX_InputStream_Json, "FromUserName");//发送方帐号（一个OpenID）
                    int CreateTime = Convert.ToInt32(WeChatServiceHelper.GetJsonValue_JObject(WX_InputStream_Json, "CreateTime"));//消息创建时间 （整型）
                    string traceID = FromUserName + CreateTime;

                    Tracer.RunLog(MessageType.WriteInfomation, "", logName, "InputStream = ：" + WX_InputStream_Json + "\r\n");

                    //MongoDBClient.InsertOneAsync(new Mongo_Log_UserTrace_Model() { TraceID = FromUserName + CreateTime, CreateTime = DateTime.Now, Content = WX_InputStream_Json, Note = "Callback start" }, EnumCollectionName.UserTrace);

                    string EventKey = WeChatServiceHelper.GetJsonValue_JObject(WX_InputStream_Json, "EventKey").Trim();
                    string MsgType = WeChatServiceHelper.GetJsonValue_JObject(WX_InputStream_Json, "MsgType");  //消息类型， 【接收事件推送 是 event】【接收普通消息  文本消息=text。。。。】
                    string msgId = WeChatServiceHelper.GetJsonValue_JObject(WX_InputStream_Json, "MsgId");
                    //TimeOutLog(traceID, "初始化 参数", timeOutThreshold, ref watch);
                    #endregion

                    #region 获取 微信用户信息
                    //FromUserName = "oqtR-w-Ik-2eXjD5PVMqAccC7w-M";
                    WX_BaseModel_UserInfo userInfo = WeChatServiceHelper.WX_API_GetUserInfo_ByOpenID(FromUserName);//根据openid获取用户的基本信息
                    if (userInfo == null)
                    {
                        ResponseEnd(response);
                        return Json(null);
                    }
                    Tracer.RunLog(MessageType.WriteInfomation, "", logName, "WX_BaseModel_UserInfo = ：" + TransformHelper.SerializeObject(userInfo) + "\r\n");
                    TimeOutLog(traceID, "API-微信 获取微信用户信息", timeOutThreshold, ref watch);
                    #endregion


                    #region 用户信息验证 [获取M站的登录token,更新用户的信息]

                    //获取数据库 用户信息
                    WXUserModel _WXUserModel = GetUserinfo(userInfo);


                    Tracer.RunLog(MessageType.WriteInfomation, "", logName, "WXUserModel = ：" + TransformHelper.SerializeObject(_WXUserModel) + "\r\n");

                    //用户登录信息 存储和更新
                    long id = _WXUserService.Save(_WXUserModel);
                    TimeOutLog(traceID,  "DB  用户信息保存 SaveUserInfo", timeOutThreshold, ref watch);
                    #endregion

                    if (MsgType == "event")//消息的来源 是  接收事件推送  
                    {
                        string _Event = WeChatServiceHelper.GetJsonValue_JObject(WX_InputStream_Json, "Event");
                        if (_Event == "subscribe")//subscribe(订阅)|unsubscribe(取消订阅)
                        {
                            #region  订阅公众号 订阅时的自动回复

                            string newsID = "DwDVF-oaQyQ4Y_AaM-s9O9BLjY5ZZssKEM4anxK8GyA";
                            List<WX_Model_ReplyMSG_News_Article> Articles = new List<WX_Model_ReplyMSG_News_Article>();

                            List<JToken> jb_List_Articles = WeChatServiceHelper.WX_API_GetURL_News(newsID);
                            //Tracer.RunLog(MessageType.WriteInfomation, "", MessageType.WriteInfomation.ToString, "jb_List_Articles = ：" + TransformHelper.SerializeObject(jb_List_Articles) + "\r\n");
                            foreach (var article in jb_List_Articles)
                            {
                                WX_Model_ReplyMSG_News_Article _WXArticleModel = new WX_Model_ReplyMSG_News_Article();

                                _WXArticleModel.Title = article["title"].ToString();
                                _WXArticleModel.Url = article["url"].ToString();
                                _WXArticleModel.Description = article["content"].ToString();
                                _WXArticleModel.PicUrl = article["thumb_url"].ToString();

                                Articles.Add(_WXArticleModel);
                            }

                            WX_Model_ReplyMSG_News _WX_Model_ReplyMSG_News = new WX_Model_ReplyMSG_News() { ToUserName = FromUserName, FromUserName = ToUserName, CreateTime = CreateTime,Articles=Articles };
                            string msg = WeChatServiceHelper.WX_API_AutomaticReply_News(_WX_Model_ReplyMSG_News, response);
                            //TimeOutLog(traceID, "DB  subscribe(订阅)", timeOutThreshold, ref watch);
                            #endregion
                        }
                    }
                    else//消息的来源  是  接收普通消息
                    {

                    }

                     AccessToCustomerSystem(response, FromUserName, ToUserName, CreateTime.ToString());
                }
            }
            catch (Exception ex)
            {
                //MongoDBClient.InsertOneAsync(new Mongo_Log_Error_Model() { TraceID = "", CreateTime = DateTime.Now, Type = "Exception", Note = System.Reflection.MethodBase.GetCurrentMethod().Name, Message = ex.Message }, EnumCollectionName.Error);
                Tracer.RunLog(MessageType.WriteInfomation, "", logErrorName, "ex = ：" + ex.Message + "\r\n");
              
            }

            ResponseEnd(response);
            TimeOutLog(Guid.NewGuid().ToString(), "End TotalTime", timeOutThreshold, ref watch);
            watch.Stop();
            return Json(null);
        }

        #region 内部方法
        //在没有设置自动回复的情况下 需要默认回复success
        private void ResponseEnd(HttpResponse response)
        {
            response.ContentEncoding = Encoding.UTF8;//必须加上
            response.Write("success");
            response.End();
            response.Close();
        }
       
        //日志
        private void TimeOutLog(string traceID, string note, long timeOutThreshold, ref Stopwatch watch)
        {
            if (watch.ElapsedMilliseconds > timeOutThreshold)
            {
                Tracer.RunLog(MessageType.WriteInfomation, "", "timeout", "note=" + note + "\r\n");
                //MongoDBClient.InsertOneAsync(new Mongo_Log_TimeOut_Model() { TraceID = traceID, CreateTime = DateTime.Now, Content = watch.ElapsedMilliseconds.ToString(), Note = note }, EnumCollectionName.TimeOut);
            }
            watch.Restart();
        }


        private WXUserModel GetUserinfo(WX_BaseModel_UserInfo userInfo)
        {
            WXUserModel model = new WXUserModel();
            model.Openid = userInfo.Openid;
            model.Unionid = userInfo.Unionid;
            model.NickName = userInfo.NickName;
            model.Sex = userInfo.Sex;
            model.Language = userInfo.Language;
            model.City = userInfo.City;
            model.Country = userInfo.Country;
            model.Province = userInfo.Province;
            model.Headimgurl = userInfo.Headimgurl;
            model.Subscribe_time = userInfo.Subscribe_time;
            model.Groupid = userInfo.Groupid;
            model.Tagid_list = userInfo.Tagid_list;
            model.LastActiveTime = DateTime.Now;
            return model;
        }

         
         private void AccessToCustomerSystem(HttpResponse response, string FromUserName, string ToUserName, string CreateTime)
        {
            response.ContentEncoding = Encoding.UTF8;
            response.Write(WeChatServiceHelper.Transfer_Customer_Service(FromUserName, ToUserName, CreateTime));
        }
        #endregion

    }
}