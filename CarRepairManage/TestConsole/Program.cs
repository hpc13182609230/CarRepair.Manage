using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HelperLib;
using LogLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service;
using WechatAppLib;
using static WechatAppLib.WeChatServiceHelper;
using RedisLib;

namespace TestConsole
{
    class Program
    {
       public static  WechatUserService service = new WechatUserService();
        static void Main(string[] args)
        {


            #region test redis
            //string a = StackExchangeRedisClient.StringGet("a");
            //StackExchangeRedisClient.StringSet("a","b");
            #endregion

            //var model =  service.GetByID(1);
            //WeChatServiceHelper
            #region 测试图文推送
            //string newsID = "DwDVF-oaQyQ4Y_AaM-s9O9BLjY5ZZssKEM4anxK8GyA";
            //List<WX_Model_ReplyMSG_News_Article> Articles = new List<WX_Model_ReplyMSG_News_Article>();

            //List<JToken> jb_List_Articles = WeChatServiceHelper.WX_API_GetURL_News(newsID);
            ////Tracer.RunLog(MessageType.WriteInfomation, "", MessageType.WriteInfomation.ToString(), "jb_List_Articles = ：" + TransformHelper.SerializeObject(jb_List_Articles) + "\r\n");
            //foreach (var article in jb_List_Articles)
            //{
            //    WX_Model_ReplyMSG_News_Article _WXArticleModel = new WX_Model_ReplyMSG_News_Article();

            //    _WXArticleModel.Title = article["title"].ToString();
            //    _WXArticleModel.Url = article["url"].ToString();
            //    _WXArticleModel.Description = article["content"].ToString();
            //    _WXArticleModel.PicUrl = article["thumb_media_id"].ToString();

            //    Articles.Add(_WXArticleModel);
            //}
            #endregion

            //string accesstoken = "8_AacYDouoLFCYM8I0QI35uG-JtAZznzpcsEpcvhw4lUwd7sKGsSB8RpIbHrJC2YYoT1P7JbfIQGYWcPRtDQx4YoCadLci_0-kM1TWpCsJOONIWXStcksjtTR9sTUCupyNmIP2j9Oqzl81_Gn9ZSHbAEAPIX";
            ////string param = "scene_str:tirelabel";
            //string param = "103";
            //WX_API_Create_QRcode_Forever(param, accesstoken);
        }


        #region 生成带参数的二维码

        //永久二维码请求[post]
        public static string WX_API_Create_QRcode_Forever(string scene_id,string Access_Token)
        {
            if (string.IsNullOrEmpty(scene_id))
                return null;
            //Access_Token = CheckTokenExpired(Access_Token);
            string request_URL = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
            request_URL = string.Format(request_URL, Access_Token);
            //post参数格式  {"action_name": "QR_LIMIT_SCENE", "action_info": {"scene": {"scene_id": 123}}}
            string postData = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + scene_id + "}}}";
            //string postData = "{\"action_name\": \"QR_LIMIT_STR_SCENE\", \"action_info\": {\"scene\": {\"scene_str\": \"" + scene_id + "\"}}}";
            string rtnStr = HttpHelper.HttpPost(request_URL, postData);
            string ticket = GetJsonValue_JObject(rtnStr, "ticket");
            //if (!string.IsNullOrEmpty(ticket))
            //    WX_API_Show_QRcode(ticket, scene_id);
            //string expire_seconds = (jb["expire_seconds"] ?? "").ToString();
            //string url = (jb["url"] ?? "").ToString();
            WX_API_Show_QRcode(ticket, scene_id);
            return ticket;
        }

        //通过ticket换取二维码[get]【此处的图片请求地址不要用微信的登录验证 所以不用下载到本地 可以直接使用】
        private static string WX_API_Show_QRcode(string ticket, string scene_id)
        {
            if (string.IsNullOrEmpty(ticket))
                ticket = "gQEE7zoAAAAAAAAAASxodHRwOi8vd2VpeGluLnFxLmNvbS9xL3BuVnQ2ckhrbHpqdVBwYlZtRmw4AAIEIC5NVwMEAAAAAA==";
            //gQEE7zoAAAAAAAAAASxodHRwOi8vd2VpeGluLnFxLmNvbS9xL3BuVnQ2ckhrbHpqdVBwYlZtRmw4AAIEIC5NVwMEAAAAAA%3d%3d
            //ICKET记得进行UrlEncode
            ticket = EncryptHelper.UrlEncode(ticket);

            string request_URL = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}";
            request_URL = string.Format(request_URL, ticket);
            // post参数格式  {"action_name": "QR_LIMIT_SCENE", "action_info": {"scene": {"scene_id": 123}}}
            //string postData = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + scene_id + "}}}";
            //string rtnStr = HttpHelper.HttpGet(request_URL, "");
            DownLoadPicture(request_URL, "", scene_id);
            return "";
        }

        //下载图片到本地
        private static void DownLoadPicture(string downLoadURL, string savePath, string scene_id)
        {
            string timeStr = DateTime.Now.ToString("yyyyMMddHHmmss");
            if (string.IsNullOrEmpty(savePath))
                savePath = @"E:\桌面文档\" + "WeiXin_QRcode_SceneId=" + scene_id + "_" + timeStr + ".jpg";
            WebClient wx_upload = new WebClient();
            //string downLoadURL = string.Format(W);
            wx_upload.DownloadFile(downLoadURL, savePath);
        }
        #endregion

        #region 内部通用方法

        /// 获取Json字符串某节点的值
        private static string GetJsonValue(string jsonStr, string key)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                key = "\"" + key.Trim('"') + "\"";
                int index = jsonStr.IndexOf(key) + key.Length + 1;
                if (index > key.Length + 1)
                {
                    //先截逗号，若是最后一个，截“｝”号，取最小值
                    int end = jsonStr.IndexOf(',', index);
                    if (end == -1)
                    {
                        end = jsonStr.IndexOf('}', index);
                    }

                    result = jsonStr.Substring(index, end - index);
                    result = result.Trim(new char[] { '"', ' ', '\'' }); //过滤引号或空格
                }
            }
            return result;
        }

        //获取json串内的指定值
        public static string GetJsonValue_JObject(string jsonStr, string key)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                JObject jb = JsonConvert.DeserializeObject(jsonStr) as JObject;
                result = (jb[key] ?? "").ToString();
            }
            return result;
        }

        #endregion


    }
}
