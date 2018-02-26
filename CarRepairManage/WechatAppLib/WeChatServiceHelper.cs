using HelperLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogLib;
using System.Net;
using System.IO;
using System.Web;
using System.Web.Security;

namespace WechatAppLib
{
    public static class WeChatServiceHelper
    {
        #region 参数
        //微信开发的基本参数
        //AppID(应用ID)
        private static string AppID = ConfigureHelper.Get("XCBB_FWH:AppID");//从配置文件中获取
        //AppSecret(应用密钥)
        private static string AppSecret = ConfigureHelper.Get("XCBB_FWH:AppSecret");//从配置文件中获取
        //获取access_token填写client_credential
        private static string Grant_Type = "client_credential";//固定不变
        //公众号的全局唯一接口调用凭据  有效期为2个小时
        //private static string Access_Token = (HttpContext.Current.Session["WX_Access_Token"] ?? "").ToString() == "" ? GetAccessToken() : CheckTokenExpired(HttpContext.Current.Session["WX_Access_Token"].ToString());
        private static string Access_Token = "";

        private static DateTime Access_Token_ExpireTime = DateTime.MinValue;
        //Access_Token过期提示信息
        private static string TokenExpireMSG = "Token Expire";
        //微信公众号的推广id
        private static string Tgid_WX = ConfigureHelper.Get("Tgid_WX");//从配置文件中获取

        private static string AccessToken_CacheKey = "XCBB_FWH:AccessToken";//从配置文件中获取



        //这个是一个特殊的网页授权access_token,与基础支持中的access_token（该access_token用于调用其他接口）不同
        private static string access_Token_Special = "";
        private static string refresh_token = "";
        private static string openid = "";
        private static string unionid = "";
        #endregion

        #region AccessToken相关

        //获取AccessToken
        public static WX_Model_AccessToken GetAccessToken()
        {
            WX_Model_AccessToken _WX_Model_AccessToken = new WX_Model_AccessToken();
            try
            {
                int expireMinute = 60;  //AccessToken的有效期时间
                string url_base = "https://api.weixin.qq.com/cgi-bin/token";//get方式
                string postData = "grant_type=" + Grant_Type + "&appid=" + AppID + "&secret=" + AppSecret;
                string rtnStr = HttpHelper.HttpGet(url_base, postData);

                Tracer.RunLog(MessageType.WriteInfomation, "", "Token", "GetAccessToken postData = ：" + postData + "\r\n");
                Tracer.RunLog(MessageType.WriteInfomation, "", "Token", "GetAccessToken rtnStr = ：" + rtnStr + "\r\n");
                _WX_Model_AccessToken.AccessToken = GetJsonValue_JObject(rtnStr, "access_token");
                if (string.IsNullOrWhiteSpace(_WX_Model_AccessToken.AccessToken))
                {
                    Tracer.RunLog(MessageType.WriteInfomation, "", "Token", " GetAccessToken  接口请求失败= ：" + "\r\n");
                    return null;
                }
                _WX_Model_AccessToken.ExpireTime = DateTime.Now.AddMinutes(Convert.ToInt32(expireMinute));
            }
            catch (Exception ex)
            {
                Tracer.RunLog(MessageType.WriteInfomation, "", "Token", " GetAccessToken  获取异常= ：" + ex + "\r\n");
                return null;
            }

            return _WX_Model_AccessToken;
        }

        //获取AccessToken时 进行 去重【方法2： increment】
        //public static bool CheckWechatAccessTokenDuplicate(string traceID)
        //{
        //    int redisDBIndex = 0;
        //    long incrementStep = 1;
        //    string redisKey = ZS_Configure_Global.ZS_Wechat_AccessToken_Duplicate;

        //    long count = StackExchangeRedisClient.StringIncrement(redisKey, incrementStep, redisDBIndex);
        //    if (count == incrementStep)//第一次
        //    {
        //        StackExchangeRedisClient.KeyExpire(redisKey, DateTime.Now.AddSeconds(5), redisDBIndex);
        //        return false;
        //    }
        //    else
        //    {
        //        MongoDBClient.InsertOneAsync(new Mongo_Log_Duplicate_Model() { TraceID = traceID, Type = "AccessToken", CreateTime = DateTime.Now, Note = "Access_Token 去重：第" + (count - 1) + "次 " }, EnumCollectionName.Duplicate);
        //        return true;
        //    }
        //}

        //验证AccessToken是否过期 并更新 

        public static string CheckTokenExpired(string access_token)
        {
            //string traceID = Guid.NewGuid().ToString();
            WX_Model_AccessToken token = GetAccessTokenRedisValue();
            //Tracer.RunLog(MessageType.WriteInfomation, "", "Token", "WX_Model_AccessToken = ：" + TransformHelper.SerializeObject(token) + "\r\n");
            if (token == null || string.IsNullOrWhiteSpace(token.AccessToken))//redis 为空，重新获取
            {
                token = GetAccessToken();
                //MongoDBClient.InsertOneAsync(new Mongo_Log_AccessToken_Model() { TraceID = traceID, CreateTime = DateTime.Now, Note = "Access_Token 为空：第一次 获取" + System.Reflection.MethodBase.GetCurrentMethod().Name, ExpireTime = token.ExpireTime, NewToken = token.AccessToken }, EnumCollectionName.AccessToken);
                if (token != null)//AccessToken获取成功
                {
                    bool flag = SetAccessTokenRedisValue(token);//更新到redis
                }
                return token.AccessToken;
            }
            else if (token.ExpireTime < DateTime.Now)//已经超过1小时
            {
                token = GetAccessToken();
                //MongoDBClient.InsertOneAsync(new Mongo_Log_AccessToken_Model() { TraceID = traceID, CreateTime = DateTime.Now, Note = "Access_Token 重新获取" + System.Reflection.MethodBase.GetCurrentMethod().Name, ExpireTime = token.ExpireTime, NewToken = token.AccessToken }, EnumCollectionName.AccessToken);
                if (token != null)//AccessToken获取成功
                {
                    bool flag = SetAccessTokenRedisValue(token);//更新到redis
                }
                 return token.AccessToken;
            }
            else
            {
                //Tracer.RunLog(MessageType.WriteInfomation, "", "Token", "Access_Token ：有效期内" + "\r\n");
                return token.AccessToken;
            }
        }

     

        //获取 AccessToken【redis】 
        public static WX_Model_AccessToken GetAccessTokenRedisValue()
        {
            string key = AccessToken_CacheKey;
            WX_Model_AccessToken model =CacheHelper.GetCache<WX_Model_AccessToken>(key);
            return model;  
        }

        //更新 AccessToken【redis】 
        //accessToken 为null
        public static bool SetAccessTokenRedisValue(WX_Model_AccessToken accessToken)
        {
            string key = AccessToken_CacheKey;
            try
            {
                //RedisHelper _RedisHelper = new RedisHelper();
                //_RedisHelper.Set(accessToken, key);
                CacheHelper.SetCache(key, accessToken);
                return true;
            }
            catch (Exception ex)
            {
                Tracer.RunLog(MessageType.WriteInfomation, "", "Error_Token", "更新 AccessToken【Cache】  更新 异常：" + ex + "\r\n");
                Tracer.RunLog(MessageType.WriteInfomation, "", "Error_Token", "更新 AccessToken【Cache】  更新 异常：key" + key + "\r\n");
                Tracer.RunLog(MessageType.WriteInfomation, "", "Error_Token", "更新 AccessToken【Cache】  更新 异常：value" + TransformHelper.SerializeObject(accessToken) + "\r\n");
                return false;
            }
        }

        #endregion

        #region 微信认证

        #region 在公众号内获取用户信息

        //通过code换取网页授权access_token 的请求地址
        private static string WeiXin_URL_GetAccessToken = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code ";
        //刷新access_token 的请求地址
        private static string WeiXin_URL_RefreshAccessToken = "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}";
        //拉取用户信息的请求地址
        private static string WeiXin_URL_GetUserInfo = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN";

         
        private static string GetAccessTokenByCode(string code)
        {
            string url = string.Format(WeiXin_URL_GetAccessToken, AppID, AppSecret, code);
            string rtnStr = HttpHelper.HttpGet(url, "");
            //string rtnStr0 = Helper.WebHelper.HttpGet(url_show + "?source=" + source + "&device=" + device + "&osver=" + osver + "&idfa=" + idfa, "");
            //string rtnStr1 = Helper.WebHelper.HttpPost(url_show, postData);
            JObject showResult = JsonConvert.DeserializeObject(rtnStr) as JObject;
            //Tracer.RunLog(MessageType.WriteInfomation, "", "jump", "GetAccessTokenByCode  1  =：" + showResult + "\r\n");
            //需要对showResult的返回值进行判断
            if (showResult["errcode"] == null)//请求成功
            {
                access_Token_Special = (showResult["access_token"] ?? "").ToString();
                openid = (showResult["openid"] ?? "").ToString();
                unionid = (showResult["unionid"] ?? "").ToString();
                Tracer.RunLog(MessageType.WriteInfomation, "", "log", "openid  =：" + openid + "\r\n");
                Tracer.RunLog(MessageType.WriteInfomation, "", "log", "unionid  =：" + unionid + "\r\n");
            }
            else
            {
                Tracer.RunLog(MessageType.WriteInfomation, "", "log", "error  =：" + showResult["errmsg"] + "\r\n");
                Tracer.RunLog(MessageType.WriteInfomation, "", "log", "url  =：" + url + "\r\n");
            }
            //JArray jb_Array = JsonConvert.DeserializeObject(showResult["table"].ToString()) as JArray;
            //var jb_List = jb_Array.ToList();
            return rtnStr;
        }

        //第三步：刷新access_token（如果需要）
        public static string RefreshAccessToken(string code)
        {
            string rtnStr = HttpHelper.HttpPost(WeiXin_URL_RefreshAccessToken, "");
            //string rtnStr0 = Helper.WebHelper.HttpGet(url_show + "?source=" + source + "&device=" + device + "&osver=" + osver + "&idfa=" + idfa, "");
            //string rtnStr1 = Helper.WebHelper.HttpPost(url_show, postData);
            JObject showResult = JsonConvert.DeserializeObject(rtnStr) as JObject;

            access_Token_Special = (showResult["access_token"] ?? "").ToString();
            return null;
        }

        //第四步：拉取用户信息(需scope为 snsapi_userinfo)
        private static WX_BaseModel_UserInfo GetWeiXin_UserInfo(string token, string openId)
        {
            string url = string.Format(WeiXin_URL_GetUserInfo, token, openid);
            string rtnStr = HttpHelper.HttpPost(url, "");
            //string rtnStr0 = Helper.WebHelper.HttpGet(url_show + "?source=" + source + "&device=" + device + "&osver=" + osver + "&idfa=" + idfa, "");
            //string rtnStr1 = Helper.WebHelper.HttpPost(url_show, postData);
            JObject showResult = JsonConvert.DeserializeObject(rtnStr) as JObject;

            WX_BaseModel_UserInfo user = new WX_BaseModel_UserInfo() { };

            user.Openid = (showResult["openid"] ?? "").ToString();
            user.Unionid = (showResult["unionid"] ?? "").ToString();
            user.NickName = (showResult["nickname"] ?? "").ToString();
            user.Sex = string.IsNullOrWhiteSpace(showResult["sex"].ToString()) ? 0 : Convert.ToInt32(showResult["sex"].ToString());
            Tracer.RunLog(MessageType.WriteInfomation, "", "log", "GetWeiXin_UserInfo  showResult=：" + showResult + "\r\n");
            return user;
        }

        public static WX_BaseModel_UserInfo WX_API_GetUserInfo(string code)
        {
            WX_BaseModel_UserInfo user = new WX_BaseModel_UserInfo();
            string message = GetAccessTokenByCode(code);

            user = GetWeiXin_UserInfo(access_Token_Special, openid);
            return user;
        }



        public static WX_BaseModel_UserInfo WX_API_GetUserInfo_ByOpenID(string openid)
        {
            WX_BaseModel_UserInfo user = new WX_BaseModel_UserInfo();
            Access_Token = CheckTokenExpired(Access_Token);
            string url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN&random={2}";
            url = string.Format(url, Access_Token, openid, DateTime.Now.ToString("yyyyMMdd HHmmssfff"));
            string rtnStr = "";
            try
            {
                //rtnStr = HttpHelper.HttpGet(url, "");
                rtnStr = HttpHelper.HttpGet_UTF8(url);
                JObject showResult = JsonConvert.DeserializeObject(rtnStr) as JObject;
                user.Openid = openid;
                user.Subscribe = showResult["subscribe"].ToString();
                user.Unionid = showResult["unionid"].ToString();
                user.Tagid_list = showResult["tagid_list"].ToString();
                if (user.Subscribe == "1")
                {
                    user.NickName = showResult["nickname"].ToString();
                    user.Sex = string.IsNullOrWhiteSpace(showResult["sex"].ToString()) ? 0 : Convert.ToInt32(showResult["sex"].ToString());
                    user.Language = showResult["language"].ToString();
                    user.City = showResult["city"].ToString();
                    user.Province = showResult["province"].ToString();
                    user.Country = showResult["country"].ToString();
                    user.Headimgurl = showResult["headimgurl"].ToString();
                    user.Subscribe_time = TransformHelper.Convert_TimeStamp2DateTime(showResult["subscribe_time"].ToString());
                    user.Remark = showResult["remark"].ToString();
                    user.Groupid = string.IsNullOrWhiteSpace(showResult["groupid"].ToString()) ? 0 : Convert.ToInt32(showResult["groupid"].ToString());
                }
                return user;
            }
            catch (Exception ex)
            {
                //MongoDBClient.InsertOneAsync(new Mongo_Log_Error_Model() { TraceID = Guid.NewGuid().ToString(), CreateTime = DateTime.Now, Type = "Exception", Note = System.Reflection.MethodBase.GetCurrentMethod().Name, Message = ex.Message, RequestData = url, ResponseInfo = rtnStr }, EnumCollectionName.Error);
                return null;
            }
        }

        #endregion

        #region 生成带参数的二维码

        //永久二维码请求[post]
        public static string WX_API_Create_QRcode_Forever(string scene_id)
        {
            if (string.IsNullOrEmpty(scene_id))
                return null;
            Access_Token = CheckTokenExpired(Access_Token);
            string request_URL = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
            request_URL = string.Format(request_URL, Access_Token);
            // post参数格式  {"action_name": "QR_LIMIT_SCENE", "action_info": {"scene": {"scene_id": 123}}}
            string postData = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + scene_id + "}}}";
            string rtnStr = HttpHelper.HttpPost(request_URL, postData);
            string ticket = GetJsonValue_JObject(rtnStr, "ticket");
            //if (!string.IsNullOrEmpty(ticket))
            //    WX_API_Show_QRcode(ticket, scene_id);
            //string expire_seconds = (jb["expire_seconds"] ?? "").ToString();
            //string url = (jb["url"] ?? "").ToString();
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
                savePath = @"C:\Users\user\Desktop\picture\" + "WeiXin_QRcode_SceneId=" + scene_id + "_" + timeStr + ".jpg";
            WebClient wx_upload = new WebClient();
            //string downLoadURL = string.Format(W);
            wx_upload.DownloadFile(downLoadURL, savePath);
        }
        #endregion

        #endregion

        #region  素材管理 相关方法

        //上传 临时 多媒体文件 的请求地址 [无法上次临时的图文素材] 
        private static string WX_URL_UploadFile_Temporary = "http://api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}";

        //新增永久图文素材 的请求地址
        private static string WX_URL_UploadNews_Forever = "https://api.weixin.qq.com/cgi-bin/material/add_news?access_token={0}";
        //新增其他类型永久素材 的请求地址
        private static string WX_URL_UploadMedia_Forever = "https://api.weixin.qq.com/cgi-bin/material/add_material?access_token={0}&type={1}";

        //获取临时素材 的请求地址
        private static string WX_URL_DownloadFile_Temporary = "https://api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}";


        #region 临时素材
        // 新增临时素材[不包括news]   [媒体文件在后台保存时间为3天，即3天后media_id失效]
        public static string WX_API_UploadFile_Temporary(string physicalPath)
        {
            Access_Token = CheckTokenExpired(Access_Token);
            WebClient _WebClient = new WebClient();
            string WeiXin_Request_URL = WX_URL_UploadFile_Temporary;

            //@"C:\Users\user\Downloads\002.jpg";【这里的文件需要使用物理路径，不能使用url替代】//http://p1.gexing.com/shaitu/20120729/1831/501510e694278.jpg
            string filename = string.IsNullOrWhiteSpace(physicalPath) ? @"C:\Users\user\Desktop\picture\" + "002.jpg" : physicalPath;
            //string filename = "http://p1.gexing.com/shaitu/20120729/1831/501510e694278.jpg";

            WeiXin_Request_URL = string.Format(WeiXin_Request_URL, Access_Token, "image");

            string extension = System.IO.Path.GetExtension(filename).ToLower();
            if (extension == ".jpg" || extension == ".png" || extension == ".jpeg" || extension == ".gif")
            {
                //将指定的文件上传到知道的url上面 
                byte[] result = _WebClient.UploadFile(new Uri(WeiXin_Request_URL), filename);
                string resultjson = Encoding.Default.GetString(result);//在这里获取json数据，以获取media_id
                //返回值格式{"type":"image","media_id":"u42HN_OiKcYYHPUDQ75Z4kGztHjDvT_yuamq-SfyshQ6SJj9ezo3MSum-xLoFBCW","created_at":1464156907}
                JObject jb = JsonConvert.DeserializeObject(resultjson) as JObject;
                string media_id = (jb["media_id"] ?? "").ToString();
                return media_id;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region 永久素材
        //新增永久素材[不包括图文素材]
        public static string WX_API_UploadFile_Forever(string physicalPath)
        {
            Access_Token = CheckTokenExpired(Access_Token);
            WebClient _WebClient = new WebClient();
            //@"C:\Users\user\Downloads\002.jpg";【这里的文件需要使用物理路径，不能使用url替代】//http://p1.gexing.com/shaitu/20120729/1831/501510e694278.jpg
            string filename = string.IsNullOrWhiteSpace(physicalPath) ? @"C:\Users\user\Desktop\picture\" + "002.jpg" : physicalPath;
            string WeiXin_Request_URL = string.Format(WX_URL_UploadMedia_Forever, Access_Token, "image");

            string extension = System.IO.Path.GetExtension(filename).ToLower();
            if (extension == ".jpg" || extension == ".png")//这能上传jpg和png格式的图片
            {
                string resultjson = HttpUploadFile_Base(WeiXin_Request_URL, filename, "media"); ;//在这里获取json数据，以获取media_id
                //返回值格式{"media_id":"KPoE9Hzq37QRhyQ4Nip86yXIb--VkqbrkXf86OBIObo","url":"https:\/\/mmbiz.qlogo.cn\/mmbiz\/l6V1ZR8yGZxMeAC0daByV8o0laFF3icWxqEpG1BrSWVA09EQmMIesSRFVmN7ZawMXRJVeXBalfvicEKmeBRRVKNQ\/0?wx_fmt=gif"}
                //JObject jb = JsonConvert.DeserializeObject(resultjson) as JObject;
                //string media_id = GetJsonValue_JObject(resultjson,"media_id");//(jb["media_id"] ?? "").ToString();
                //return media_id;
                return resultjson;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///  Http上传文件【基础方法】
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <param name="path">文件的绝对物理路径</param>
        /// formName 表单的名称 用于特定的要求【如上传微信的永久素材需要指定为media】
        /// <returns>返回值格式：{"media_id":"NV5LxfECHIR1F0HOM_B3tU0sZFdqtblnEfRL9YINFLM","url":"https:\/\/mmbiz.qlogo.cn\/mmbiz\/NBm1e06f9ibyEDr3gV01ntZkwdIzaibicfzn1dB13bS8qVBusiadlz4FCB4ba5UO5ak5kFtezYCSqSGHzwFGCbnaFw\/0?wx_fmt=jpeg"}</returns>
        private static string HttpUploadFile_Base(string url, string path, string formName)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
            request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            int pos = path.LastIndexOf("\\");
            string fileName = path.Substring(pos + 1);

            //请求头部信息    通过POST表单来调用接口，表单id为media，包含需要上传的素材内容，有filename、filelength、content-type等信息
            StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"{0}\";filename=\"{1}\"\r\nContent-Type:application/octet-stream\r\n\r\n", formName, fileName));
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] bArr = new byte[fs.Length];
            fs.Read(bArr, 0, bArr.Length);
            fs.Close();

            Stream postStream = request.GetRequestStream();
            postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
            postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
            postStream.Write(bArr, 0, bArr.Length);
            postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            postStream.Close();

            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream instream = response.GetResponseStream();
            StreamReader sr = new StreamReader(instream, Encoding.UTF8);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            return content;
        }

        // 新增永久图文素材
        public static string WX_API_Add_News(List<WX_Model_Add_News_Article> articles)
        {
            Access_Token = CheckTokenExpired(Access_Token);
            string url_base = WX_URL_UploadNews_Forever;
            url_base = string.Format(url_base, Access_Token);
            // "{\"articles\":[{\"title\":\"" + title + "\",\"thumb_media_id\":\"" + thumb_media_id + "\",\"author\":\"" + author + "\",\"digest\":\"" + digest + "\",\"show_cover_pic\":\"" + show_cover_pic + "\",\"content\":\"" + content + "\",\"content_source_url\":\"" + content_source_url + "\"}]}";
            string postData = "{\"articles\":[";
            foreach (var article in articles)
            {
                postData += "{\"title\":\"" + article.Title + "\",\"thumb_media_id\":\"" + article.Thumb_Media_ID + "\",\"author\":\"" + article.Author + "\",\"digest\":\"" + article.Digest + "\",\"show_cover_pic\":\"" + article.Show_Cover_Pic + "\",\"content\":\"" + article.Content + "\",\"content_source_url\":\"" + article.Content_Source_Url + "\"},";
            }
            postData = postData.Substring(0, postData.Length - 1);
            postData += "]}";

            string rtnStr = HttpHelper.HttpPost(url_base, postData);
            JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;
            string media_id = (jb["media_id"] ?? "").ToString();
            return media_id;
        }

        // 修改永久图文素材
        public static string WX_API_Update_News(string media_ID, List<WX_Model_Add_News_Article> articles)
        {
            Access_Token = CheckTokenExpired(Access_Token);
            string url_base = "https://api.weixin.qq.com/cgi-bin/material/update_news?access_token={0}";
            url_base = string.Format(url_base, Access_Token);
            // {"media_id":"MEDIA_ID","index":"INDEX","articles":{"title":"TITLE","thumb_media_id":"THUMB_MEDIA_ID","author":"AUTHOR","digest":"DIGEST","show_cover_pic":"SHOW_COVER_PIC(0 / 1)","content":"CONTENT","content_source_url":"CONTENT_SOURCE_URL"}}
            for (int i = 0; i < articles.Count; i++)
            {
                var article = articles[i];
                string postData = "{\"media_id\":\"" + media_ID + "\",\"index\":\"" + i + "\",\"articles\":";
                postData += "{\"title\":\"" + article.Title + "\",\"thumb_media_id\":\"" + article.Thumb_Media_ID + "\",\"author\":\"" + article.Author + "\",\"digest\":\"" + article.Digest + "\",\"show_cover_pic\":\"" + article.Show_Cover_Pic + "\",\"content\":\"" + article.Content + "\",\"content_source_url\":\"" + article.Content_Source_Url + "\"}}";
                string rtnStr = HttpHelper.HttpPost(url_base, postData);
                JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;
                string errcode = (jb["errcode"] ?? "").ToString();
                if (errcode != "0")
                {
                    media_ID = "";
                    break;
                }
            }
            return media_ID;
        }

        // 删除永久素材
        public static string WX_API_Delete_Material_Forever(string media_ID)
        {
            try
            {
                Access_Token = CheckTokenExpired(Access_Token);
                string request_URL = "https://api.weixin.qq.com/cgi-bin/material/del_material?access_token={0}";
                request_URL = string.Format(request_URL, Access_Token);
                var obj = new { media_id = media_ID };
                string postData = TransformHelper.SerializeObject(obj);
                string rtnStr = HttpHelper.HttpPost(request_URL, postData);
                JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;

                string errcode = (jb["errcode"] ?? "").ToString();
                if (errcode != "0")
                {
                    Tracer.RunLog(MessageType.WriteInfomation, "", "error", "删除永久素材  【" + System.Reflection.MethodBase.GetCurrentMethod().Name + "】异常 =" + errcode + "\r\n");
                }
                return errcode;
            }
            catch (Exception ex)
            {
                Tracer.RunLog(MessageType.WriteInfomation, "", "error", "删除永久素材  【" + System.Reflection.MethodBase.GetCurrentMethod().Name + "】异常 =" + ex + "\r\n");
                return "";
            }
        }

        // 获取永久图文素材【非列表】
        public static List<JToken> WX_API_GetURL_News(string media_id)
        {
            Access_Token = CheckTokenExpired(Access_Token);
            string url_base = "https://api.weixin.qq.com/cgi-bin/material/get_material?access_token={0}";
            url_base = string.Format(url_base, Access_Token);
            // "{\"articles\":[{\"title\":\"" + title + "\",\"thumb_media_id\":\"" + thumb_media_id + "\",\"author\":\"" + author + "\",\"digest\":\"" + digest + "\",\"show_cover_pic\":\"" + show_cover_pic + "\",\"content\":\"" + content + "\",\"content_source_url\":\"" + content_source_url + "\"}]}";

            string postData = "{\"media_id\":\"" + media_id + "\"}";
            string rtnStr = HttpHelper.HttpPost(url_base, postData);
            JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;

            if (string.IsNullOrWhiteSpace(jb["news_item"].ToString()))
            {
                return null;
            }
            else
            {
                List<JToken> result = new List<JToken>();
                JArray jb_Array = JsonConvert.DeserializeObject(jb["news_item"].ToString()) as JArray;
                List<JToken> jb_List = jb_Array.ToList();
                result.AddRange(jb_List);
                return result;
            }

        }

        //获取永久素材的列表
        public static List<JToken> WX_API_BatchGet_Material(string meterialType)
        {
            WX_Model_Material_Count Material_Count = WX_API_BatchGet_Material();
            List<JToken> result = new List<JToken>();
            int offset = 0;
            int count = 10;
            int page = 1;

            if (meterialType == "image")
            {
                page = (int)Math.Ceiling(Convert.ToDouble(Material_Count.Image_Count) / count);
            }
            else if (meterialType == "news")
            {
                page = (int)Math.Ceiling(Convert.ToDouble(Material_Count.News_Count) / count);
            }

            for (int i = 1; i <= page; i++)
            {
                offset = (i - 1) * count;
                string rtnStr = Get_Material_Base(meterialType, offset, count);
                JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;


                //jb.Add(new JProperty("name","content"));
                //jb.Remove("name");


                JArray jb_Array = JsonConvert.DeserializeObject(jb["item"].ToString()) as JArray;
                List<JToken> jb_List = jb_Array.ToList();
                foreach (var item in jb_List)
                {
                    var url = (item["url"] ?? "").ToString();
                    if (url == "")
                    {
                        int sss = 0;
                    }
                }
                result.AddRange(jb_List);
            }

            return result;
        }

        //获取 指定位置 的 图文【获取图文的基本方法】
        public static List<JToken> WX_API_BatchGet_Material_Page(string meterialType, int offset = 0, int count = 10)
        {
            List<JToken> result = new List<JToken>();

            string rtnStr = Get_Material_Base(meterialType, offset, count);
            JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;

            JArray jb_Array = JsonConvert.DeserializeObject(jb["item"].ToString()) as JArray;
            List<JToken> jb_List = jb_Array.ToList();
            result.AddRange(jb_List);

            return result;
        }

        /// <summary>
        /// 获取素材列表
        /// </summary>
        /// <param name="type">素材的类型，图片（image）、视频（video）、语音 （voice）、图文（news）</param>
        /// <param name="offset">从全部素材的该偏移位置开始返回，0表示从第一个素材 返回</param>
        /// <param name="count">返回素材的数量，取值在1到20之间</param>
        /// <returns></returns>
        private static string Get_Material_Base(string type, int offset, int count)
        {
            Access_Token = CheckTokenExpired(Access_Token);
            string url_base = "https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token={0}";
            url_base = string.Format(url_base, Access_Token);
            string postData = "{\"type\": \"" + type + "\",\"offset\": \"" + offset + "\",\"count\": \"" + count + "\"}";// post参数格式  {"msg_id":"201053012"}
            string rtnStr = HttpHelper.HttpPost(url_base, postData);
            return rtnStr;
        }

        //获取素材总数[http请求方式: GET]
        public static WX_Model_Material_Count WX_API_BatchGet_Material()
        {
            WX_Model_Material_Count result = new WX_Model_Material_Count();

            Access_Token = CheckTokenExpired(Access_Token);
            string url_base = "https://api.weixin.qq.com/cgi-bin/material/get_materialcount?access_token={0}";//[获取素材总数]
            url_base = string.Format(url_base, Access_Token);

            string rtnStr = HttpHelper.HttpGet(url_base, "");
            JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;
            result.Voice_Count = Convert.ToInt32(GetJsonValue_JObject(rtnStr, "voice_count"));
            result.Video_Count = Convert.ToInt32(GetJsonValue_JObject(rtnStr, "video_count"));
            result.Image_Count = Convert.ToInt32(GetJsonValue_JObject(rtnStr, "image_count"));
            result.News_Count = Convert.ToInt32(GetJsonValue_JObject(rtnStr, "news_count"));

            return result;
        }

        #endregion


        /// 获取临时素材[下载到本地]
        public static void WX_API_DownLoadFile_Temporary(string media_id, string fileName = "")
        {
            Access_Token = CheckTokenExpired(Access_Token);
            if (string.IsNullOrWhiteSpace(media_id))
                media_id = "u42HN_OiKcYYHPUDQ75Z4kGztHjDvT_yuamq-SfyshQ6SJj9ezo3MSum-xLoFBCW";
            if (string.IsNullOrWhiteSpace(fileName))
                fileName = @"C:\Users\user\Desktop\picture\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            WebClient wx_upload = new WebClient();
            string downLoadURL = string.Format(WX_URL_DownloadFile_Temporary, Access_Token, media_id);
            try
            {
                wx_upload.DownloadFile(downLoadURL, fileName);
            }
            catch (Exception ex)
            {
            }
        }


        /// 获取永久素材[下载到本地]
        public static void WX_API_DownLoadFile_Forever(string ImageURL, string fileName = "")
        {
            WebClient wx_upload = new WebClient();
            try
            {
                wx_upload.DownloadFile(ImageURL, fileName);
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region  在微信公众号内的消息的群发处理
        //由于群发任务提交后，群发任务可能在一定时间后才完成，因此，群发接口调用时，仅会给出群发任务是否提交成功的提示，若群发任务提交成功，则在群发任务结束时，会向开发者在公众平台填写的开发者URL（callback URL）推送事件。

        //根据标签进行群发【订阅号与服务号认证后均可用】
        public static string WX_SendMSG_Tag(string tag_id, WX_MessageType type, string content)
        {
            Access_Token = CheckTokenExpired(Access_Token);
            string request_URL = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token={0}";
            request_URL = string.Format(request_URL, Access_Token);
            //string media_id = "NV5LxfECHIR1F0HOM_B3tU0sZFdqtblnEfRL9YINFLM";
            // post参数格式  图文{"filter":{"is_to_all":false,"tag_id":2},"mpnews":{"media_id":"123dsdajkasd231jhksad"},"msgtype":"mpnews"}
            //               文本{"filter":{"is_to_all":false,"tag_id":2},"text":{"content":"CONTENT"},"msgtype":"text"}
            string postData = "{\"filter\":{\"is_to_all\":false,\"tag_id\":" + tag_id + "}";

            if (type == WX_MessageType.mpnews)
            {
                postData += ",\"mpnews\":{\"media_id\":\"" + content + "\"},\"msgtype\":\"mpnews\"}";
            }
            else if (type == WX_MessageType.text)
            {
                postData += ",\"text\":{\"content\":\"" + content + "\"},\"msgtype\":\"text\"}";
            }
            //.......

            //返回格式{"errcode":0,"errmsg":"send job submission success","msg_id":34182,"msg_data_id":206227730}
            string rtnStr = HttpHelper.HttpPost(request_URL, postData);
            string msg_id = GetJsonValue_JObject(rtnStr, "msg_id");//(jb["media_id"] ?? "").ToString();
            if (msg_id == TokenExpireMSG)
            {
                msg_id = WX_SendMSG_Tag(tag_id, type, content);
            }
            return msg_id;
        }

        //根据OpenID列表群发【订阅号不可用，服务号认证后可用】 OpenID至少两个
        public static string WX_SendMSG_OpenID(List<string> OpenIDs, WX_MessageType type, string content)
        {
            //测试
            //OpenIDs = new List<string>() { "ouN6_jv0J68lNbbvn1sC29wc08EY", "ouN6_jr37IYAMuJ4J2uJ3CvLRN-U"};
            //type = WX_MessageType.news;
            //content = "NV5LxfECHIR1F0HOM_B3tVoGfFNTD75IQO5Oqn4R7ao";
            Access_Token = CheckTokenExpired(Access_Token);
            string request_URL = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token={0}";
            request_URL = string.Format(request_URL, Access_Token);
            //string media_id = "NV5LxfECHIR1F0HOM_B3tU0sZFdqtblnEfRL9YINFLM";
            // post参数格式  图文{"touser":["OPENID1","OPENID2"],"mpnews":{"media_id":"123dsdajkasd231jhksad"},"msgtype":"mpnews"}
            //               文本{"touser":["OPENID1","OPENID2"],"msgtype":"text","text":{"content":"hello from boxer."}}
            string postData = "{\"touser\":[";
            List<string> temp = new List<string>();
            foreach (var item in OpenIDs)
            {
                string tempData = "\"" + item + "\"";
                temp.Add(tempData);
            }
            postData += String.Join(",", temp);
            if (type == WX_MessageType.mpnews)
            {
                postData += "],\"mpnews\":{\"media_id\":\"" + content + "\"},\"msgtype\":\"mpnews\"}";
            }
            else if (type == WX_MessageType.text)
            {
                postData += "],\"msgtype\":\"text\",\"text\":{\"content\":\"" + content + "\"}}";
            }
            //.......

            //返回格式{"errcode":0,"errmsg":"send job submission success","msg_id":34182,"msg_data_id":206227730}
            string rtnStr = HttpHelper.HttpPost(request_URL, postData);
            string msg_id = GetJsonValue_JObject(rtnStr, "msg_id");//(jb["media_id"] ?? "").ToString();
            if (msg_id == TokenExpireMSG)
            {
                msg_id = WX_SendMSG_OpenID(OpenIDs, type, content);
            }
            return msg_id;
        }

        //查询发送信息的状态
        private static string WX_SendMSG_QueryStatu(string msg_id)
        {
            Access_Token = CheckTokenExpired(Access_Token);
            string request_URL = "https://api.weixin.qq.com/cgi-bin/message/mass/get?access_token={0}";
            request_URL = string.Format(request_URL, Access_Token);
            string postData = "{\"msg_id\": \"" + msg_id + "\"}";// post参数格式  {"msg_id":"201053012"}
            string rtnStr = HttpHelper.HttpPost(request_URL, postData);
            JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;
            string MSG_Status = (jb["msg_status"] ?? "").ToString();
            return MSG_Status;
        }

        //删除发送信息
        //1、只有已经发送成功的消息才能删除
        //2、删除消息是将消息的图文详情页失效，已经收到的用户，还是能在其本地看到消息卡片。
        //3、删除群发消息只能删除图文消息和视频消息，其他类型的消息一经发送，无法删除。
        //4、如果多次群发发送的是一个图文消息，那么删除其中一次群发，就会删除掉这个图文消息也，导致所有群发都失效
        private static string WX_DeleteMSG(string msg_id)
        {
            Access_Token = CheckTokenExpired(Access_Token);
            string request_URL = "https://api.weixin.qq.com/cgi-bin/message/mass/delete?access_token=ACCESS_TOKEN";
            request_URL = string.Format(request_URL, Access_Token);
            string postData = "{\"msg_id\": \"" + msg_id + "\"}";// post参数格式  {"msg_id":"201053012"}
            string rtnStr = HttpHelper.HttpPost(request_URL, postData);
            JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;
            string MSG_Status = (jb["msg_status"] ?? "").ToString();
            return MSG_Status;
        }

        #endregion

        #region  接收的微信回调[如果是传递流，请不要用参数接收]

        //验证消息的确来自微信服务器
        public static void WX_API_Check_CallBack_URL(HttpRequest request, HttpResponse response)
        {
            string echoStr = (request.QueryString["echoStr"] ?? "").ToString();
            Tracer.RunLog(MessageType.WriteInfomation, "", "sign", "echoStr=" + echoStr + "\r\n");
            if (CheckSignature(request))
            {
                if (!string.IsNullOrEmpty(echoStr))
                {
                    response.Write(echoStr);
                    response.End();
                }
            }
        }

        //验证 微信回调的url签名
        private static bool CheckSignature(HttpRequest Request)
        {
            string Token = ConfigureHelper.Get("WeiXin_URL_Ckeck_Token");
            string signature = Request.QueryString["signature"].ToString();
            string timestamp = Request.QueryString["timestamp"].ToString();
            string nonce = Request.QueryString["nonce"].ToString();
            string[] ArrTmp = { Token, timestamp, nonce };
            Array.Sort(ArrTmp);　　 //字典排序  
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            Tracer.RunLog(MessageType.WriteInfomation, "", "sign", "tmpStr=" + tmpStr + "\r\n");
            Tracer.RunLog(MessageType.WriteInfomation, "", "sign", "signature=" + signature + "\r\n");
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //业务逻辑写在微信回调的url内【此处只是样例】
        public static string WX_API_CallBack(HttpRequest request, HttpResponse response)
        {
            string HttpMethod = request.HttpMethod.ToUpper();
            Tracer.RunLog(MessageType.WriteInfomation, "", "log", "回调的请求方式：" + HttpMethod + "\r\n");
            if (HttpMethod == "GET")//验证 url 是get方式
            {
                WX_API_Check_CallBack_URL(request, response);
                return "";
            }
            else
            {
                //获取流
                Stream WX_InputStream_Stream = request.InputStream;
                string WX_InputStream_Json = TransformHelper.Convert_Stream2Json(WX_InputStream_Stream);
                //Tracer.RunLog(MessageType.WriteInfomation, "", "log", "获取流原json ：" + WX_InputStream_Json + "\r\n");
                WX_InputStream_Json = TransformHelper.Json_Remove_CData(WX_InputStream_Json);
                //Tracer.RunLog(MessageType.WriteInfomation, "", "log", "获取流的除去CData的json ：" + WX_InputStream_Json + "\r\n");
                //关于重试的消息排重，推荐使用msgid排重。
                string ToUserName = GetJsonValue_JObject(WX_InputStream_Json, "ToUserName");//开发者微信号
                string FromUserName = GetJsonValue_JObject(WX_InputStream_Json, "FromUserName");//发送方帐号（一个OpenID）
                string MsgType = GetJsonValue_JObject(WX_InputStream_Json, "MsgType");	 //消息类型， 【接收事件推送 是 event】【接收普通消息  文本消息=text。。。。】
                int CreateTime = TransformHelper.Convert_DateTime2Int(DateTime.Now);//消息创建时间 （整型）

                //Tracer.RunLog(MessageType.WriteInfomation, "", "log", "回调参数:"
                //                                                            + "[ToUserName=" + ToUserName + "]"
                //                                                            + "[FromUserName=" + FromUserName + "]"
                //                                                            + "[Event=" + Event + "]"
                //                                                            + "[Ticket=" + Ticket + "]"
                //                                                            + "\r\n");

                if (MsgType == "event")//消息的来源 是  接收事件推送   
                {
                    //事件类型
                    //[关注/取消关注事件=subscribe(订阅)、unsubscribe(取消订阅)]
                    //[扫描带参数二维码事件 =subscribe]
                    //[上报地理位置事件=LOCATION]
                    //[自定义菜单事件 拉取消息= CLICK  跳转链接= VIEW]
                    string Event = GetJsonValue_JObject(WX_InputStream_Json, "Event");
                    //ticket 用来判断【扫描带参数二维码事件|关注/取消关注事件】
                    string Ticket = GetJsonValue_JObject(WX_InputStream_Json, "Ticket");
                    if (Event == "subscribe")//subscribe(订阅)|unsubscribe(取消订阅)
                    {
                        ///扫描带参数二维码事件
                        string EventKey = GetJsonValue_JObject(WX_InputStream_Json, "EventKey");//事件KEY值，qrscene_为前缀，后面为二维码的参数值 

                        WX_Model_ReplyMSG_Text WX_Model_ReplyMSG_Text = new WX_Model_ReplyMSG_Text() { ToUserName = FromUserName, FromUserName = ToUserName, CreateTime = CreateTime };
                        WX_Model_ReplyMSG_Text.Content = "关注了小开，就说明你是个有内涵的人主人，点击开卷书城，百万正版小说等着你，让你彻底远离书荒，任性看书！！！";
                        WX_Model_ReplyMSG_Text.MsgType = "text";
                        return WX_API_AutomaticReply_Subscribe_Event(WX_Model_ReplyMSG_Text, response);


                    }
                    else if (true)
                    {

                    }
                }
                else////消息的来源  是  接收普通消息
                {
                    if (MsgType == "text")//文本消息
                    {
                        //获取接收到的微信的信息 文本
                        string receve_MSG = GetJsonValue_JObject(WX_InputStream_Json, "Content");
                        #region 从数据库获取所有的任务 并去匹配出一个要回复的任务【若订阅用户发送的信息含有多个设置的关键字，则会随机回复】。。。。。
                        #endregion

                        List<string> keyWords = new List<string>() { "幸运字", "兑奖", "奖品", "元宵", "守护神" };//从任务中获取关键字集合
                        var isExist = keyWords.Where(p => receve_MSG.Contains(p)).Count();
                        string reply_type = "news";//回复微信消息的类型 [从任务中获取回复的类型]

                        #region 关键字自动回复逻辑
                        Tracer.RunLog(MessageType.WriteInfomation, "", "log", "自动回复文本 isExist= ：" + isExist + "\r\n");
                        if (isExist > 0)
                        {
                            if (reply_type == "text")//回复文本
                            {
                                WX_Model_ReplyMSG_Text WX_Model_ReplyMSG_Text = new WX_Model_ReplyMSG_Text() { ToUserName = FromUserName, FromUserName = ToUserName, CreateTime = CreateTime };
                                WX_Model_ReplyMSG_Text.MsgType = "text";//回复的消息类型
                                WX_Model_ReplyMSG_Text.Content = "[图文消息]【活动】抽猴年守护神，赢取幸运大礼！ ";
                                return WX_API_AutomaticReply_Text(WX_Model_ReplyMSG_Text, response);
                            }
                            else if (reply_type == "news")//回复图文
                            {
                                Tracer.RunLog(MessageType.WriteInfomation, "", "log", "进入到news 的回复阶段" + "\r\n");
                                WX_Model_ReplyMSG_News WX_Model_ReplyMSG_News = new WX_Model_ReplyMSG_News() { ToUserName = FromUserName, FromUserName = ToUserName, CreateTime = CreateTime };
                                WX_Model_ReplyMSG_News.MsgType = "news";
                                WX_Model_ReplyMSG_News_Article ar = new WX_Model_ReplyMSG_News_Article() { Title = "ar测试news", Description = "ar测试new", Url = "https://mp.weixin.qq.com/wiki", PicUrl = "http://mmbiz.qpic.cn/mmbiz/NBm1e06f9ibwrbGDwIRIZELia9N5sxR3BCA7X3SSjnykcKxHDO5CSAicKvtXkxeQ5lRN4cWZsYicf7Pwb50GibareHA/0" };
                                WX_Model_ReplyMSG_News_Article br = new WX_Model_ReplyMSG_News_Article() { Title = "br测试news", Description = "br测试new", Url = "https://www.baidu.com/", PicUrl = "http://mmbiz.qpic.cn/mmbiz/NBm1e06f9ibwrbGDwIRIZELia9N5sxR3BCA7X3SSjnykcKxHDO5CSAicKvtXkxeQ5lRN4cWZsYicf7Pwb50GibareHA/0" };
                                WX_Model_ReplyMSG_News.Articles = new List<WX_Model_ReplyMSG_News_Article>() { ar, br };
                                WX_Model_ReplyMSG_News.ArticleCount = WX_Model_ReplyMSG_News.Articles.Count.ToString();
                                return WX_API_AutomaticReply_News(WX_Model_ReplyMSG_News, response);
                            }

                        }
                        //先判断是否设置的了 普通的消息自动回复【有则回复 ，没有 返回success】
                        //假如服务器无法保证在五秒内处理并回复，必须回复"success"，这样微信服务器才不会对此作任何处理，并且不会发起重试（这种情况下，可以使用客服消息接口进行异步回复），否则，将出现严重的错误提示。
                        //用户回复的消息不包含关键字，请回复success
                        else
                        {
                            response.ContentEncoding = Encoding.UTF8;
                            response.Write("success");
                            response.End();
                        }

                        #endregion 
                    }
                }
                return null;
            }


        }
        #endregion

        #region 自动回复[将在接收微信的回调的时候触发]

        //关注事件[被添加的自动回复]
        public static string WX_API_AutomaticReply_Subscribe_Event(WX_Model_ReplyMSG_Text WX_Model_ReplyMSG_Text, HttpResponse response)
        {
            //Tracer.RunLog(MessageType.WriteInfomation, "", "log", "进入到自动回复方法" + "\r\n");
            return WX_API_AutomaticReply_Text(WX_Model_ReplyMSG_Text, response);
        }

        #region 回复文本消息 基本方法
        private static string WX_Tamplate_Automatic_Reply_Text = "<xml><ToUserName><![CDATA[{0}]]></ToUserName>                                                                                                                    <FromUserName><![CDATA[{1}]]></FromUserName>                                                                                                                <CreateTime>{2}</CreateTime>                                                                                                                                <MsgType><![CDATA[{3}]]></MsgType>                                                                                                                          <Content><![CDATA[{4}]]></Content></xml>";
        //自动回复 文本
        public static string WX_API_AutomaticReply_Text(WX_Model_ReplyMSG_Text WX_Model_ReplyMSG_Text, HttpResponse response)
        {
            string WX_OutPutStream_Json = string.Format(WX_Tamplate_Automatic_Reply_Text,
                                                        WX_Model_ReplyMSG_Text.ToUserName,
                                                        WX_Model_ReplyMSG_Text.FromUserName,
                                                        WX_Model_ReplyMSG_Text.CreateTime,
                                                        WX_Model_ReplyMSG_Text.MsgType,
                                                        WX_Model_ReplyMSG_Text.Content);
            response.ContentEncoding = Encoding.UTF8;
            response.Write(WX_OutPutStream_Json);
            response.End();
            return WX_OutPutStream_Json;
        }
        #endregion

        #region 回复图文 基本方法
        private static string WX_Tamplate_Automatic_Reply_News = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[fromUser]]></FromUserName><CreateTime>[CreateTime]</CreateTime><MsgType><![CDATA[news]]></MsgType><ArticleCount>[ArticleCount]</ArticleCount><Articles>[Articles]</Articles></xml>";
        private static string WX_Tamplate_Automatic_Reply_News_Article = "<item><Title><![CDATA[title]]></Title><Description><![CDATA[description]]></Description><PicUrl><![CDATA[picurl]]></PicUrl><Url><![CDATA[url]]></Url></item>";
        //自动回复 文本
        public static string WX_API_AutomaticReply_News(WX_Model_ReplyMSG_News WX_Model_ReplyMSG_News, HttpResponse response)
        {
            string resulu_Articles = "";
            foreach (var article in WX_Model_ReplyMSG_News.Articles)
            {
                string resulu_Article = "";
                resulu_Article = WX_Tamplate_Automatic_Reply_News_Article.Replace("[title]", "[" + article.Title + "]");
                resulu_Article = resulu_Article.Replace("[description]", "[" + article.Description + "]");
                resulu_Article = resulu_Article.Replace("[picurl]", "[" + article.PicUrl + "]");
                resulu_Article = resulu_Article.Replace("[url]", "[" + article.Url + "]");
                resulu_Articles += resulu_Article;
            }

            string WX_OutPutStream_Json = WX_Tamplate_Automatic_Reply_News.Replace("[toUser]", "[" + WX_Model_ReplyMSG_News.ToUserName + "]");
            WX_OutPutStream_Json = WX_OutPutStream_Json.Replace("[fromUser]", "[" + WX_Model_ReplyMSG_News.FromUserName + "]");
            WX_OutPutStream_Json = WX_OutPutStream_Json.Replace("[CreateTime]", WX_Model_ReplyMSG_News.CreateTime.ToString());
            WX_OutPutStream_Json = WX_OutPutStream_Json.Replace("[ArticleCount]", WX_Model_ReplyMSG_News.Articles.Count.ToString());
            WX_OutPutStream_Json = WX_OutPutStream_Json.Replace("[Articles]", resulu_Articles);
            //string WX_OutPutStream_Json = "<xml><ToUserName><![CDATA[" + WX_Model_ReplyMSG_Text.ToUserName + "]]></ToUserName>                                                                          <FromUserName><![CDATA[" + WX_Model_ReplyMSG_Text.FromUserName + "]]></FromUserName>                                                                    <CreateTime>" + WX_Model_ReplyMSG_Text.CreateTime + "</CreateTime>                                                                                      <MsgType><![CDATA[" + WX_Model_ReplyMSG_Text.MsgType + "]]></MsgType>                                                                                   <Content><![CDATA[" + WX_Model_ReplyMSG_Text.Content + "]]></Content></xml>";
            Tracer.RunLog(MessageType.WriteInfomation, "", "log", "返回 news 的 xml:" + WX_OutPutStream_Json
                                                                        + "\r\n");
            //XmlDocument  xml = TransformHelper.Convert_Json2Xml(WX_OutPutStream_Json);
            //{"ToUserName":"toUser","FromUserName":"fromUser","CreateTime":"12345678","MsgType":"text","Content":"你好"}
            response.ContentEncoding = Encoding.UTF8;
            response.Write(WX_OutPutStream_Json);
            return WX_OutPutStream_Json;
        }
        #endregion

        #region 获取公众号的自动回复规则列表[get]
        //此处的数据是编辑模式的数据 开启开发者模式后，基本无效
        public static string WX_API_Get_Current_AutoReply_Info()
        {
            Access_Token = CheckTokenExpired(Access_Token);
            string request_URL = "https://api.weixin.qq.com/cgi-bin/get_current_autoreply_info?access_token={0}";
            request_URL = string.Format(request_URL, Access_Token);
            string rtnStr = HttpHelper.HttpGet(request_URL, "");
            JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;
            string MSG = (jb["msg_status"] ?? "").ToString();
            return MSG;
        }

        #endregion

        #endregion

        #region 自定义菜单

        //自定义菜单 创建接口 url
        private static string WX_URL_Menu_Create = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";
        //自定义菜单 创建接口 [post]
        public static string WX_API_Menu_Create(string menu_Json_String)
        {
            Access_Token = CheckTokenExpired(Access_Token);
            string request_URL = string.Format(WX_URL_Menu_Create, Access_Token);
            // post参数格式  {"button":[{"type":"click","name":"今日歌曲","key":"V1001_TODAY_MUSIC"},{"name":"菜单","sub_button":[{"type":"view","name":"搜索","url":"http://www.soso.com/"},{"type":"view","name":"视频","url":"http://v.qq.com/"},{"type":"click","name":"赞一下我们","key":"V1001_GOOD"}]}]}
            string postData = "{\"button\":[{\"type\":\"view\",\"name\":\"我的书架\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx9dc24125364121ce&redirect_uri=http%3a%2f%2fapi2.kingreader.com%2fV3%2fTrade%2fwxJump.aspx%3ftgid%3d8875803%26url%3dhttp%253a%252f%252fm.kingreader.com%252fUser%252fCollection%253ftgid%253d8875803&response_type=code&scope=snsapi_userinfo&state=123#wechat_redirect\"},{\"name\":\"开卷书城\",\"sub_button\":[{\"type\":\"view\",\"name\":\"热门推荐\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx9dc24125364121ce&redirect_uri=http%3a%2f%2fapi2.kingreader.com%2fV3%2fTrade%2fwxJump.aspx%3ftgid%3d8875803%26url%3dhttp%253a%252f%252fm.kingreader.com%252f%253ftgid%253d8875803&response_type=code&scope=snsapi_userinfo&state=123#wechat_redirect\"},{\"type\":\"view\",\"name\":\"图书搜索\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx9dc24125364121ce&redirect_uri=http%3a%2f%2fapi2.kingreader.com%2fV3%2fTrade%2fwxJump.aspx%3ftgid%3d8875803%26url%3dhttp%253a%252f%252fm.kingreader.com%252fSearch%252fIndex.shtml%253ftk%253dcSdVAPfW3MR%25252fsHB6hxDU4aAV%25252fluXvp71gYd%25252fE9OIHGs%25253d%2526ak%253dtkr_web%2526tgid%253d8875803%2526ism%253d1%2526sc%253d640&response_type=code&scope=snsapi_userinfo&state=123#wechat_redirect\"},{\"type\":\"view\",\"name\":\"排行榜\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx9dc24125364121ce&redirect_uri=http%3a%2f%2fapi2.kingreader.com%2fV3%2fTrade%2fwxJump.aspx%3ftgid%3d8875803%26url%3dhttp%253a%252f%252fm.kingreader.com%252fHome%252fRank.shtml%253ftk%253dcSdVAPfW3MR%25252fsHB6hxDU4aAV%25252fluXvp71gYd%25252fE9OIHGs%25253d%2526ak%253dtkr_web%2526tgid%253d8875803%2526ism%253d1%2526sc%253d640&response_type=code&scope=snsapi_userinfo&state=123#wechat_redirect\"},{\"type\":\"view\",\"name\":\"分类\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx9dc24125364121ce&redirect_uri=http%3a%2f%2fapi2.kingreader.com%2fV3%2fTrade%2fwxJump.aspx%3ftgid%3d8875803%26url%3dhttp%253a%252f%252fm.kingreader.com%252fClass%252fIndex.shtml%253ftk%253dcSdVAPfW3MR%25252fsHB6hxDU4aAV%25252fluXvp71gYd%25252fE9OIHGs%25253d%2526ak%253dtkr_web%2526tgid%253d8875803%2526ism%253d1%2526sc%253d640&response_type=code&scope=snsapi_userinfo&state=123#wechat_redirect\"},{\"type\":\"view\",\"name\":\"书单推荐\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx9dc24125364121ce&redirect_uri=http%3a%2f%2fapi2.kingreader.com%2fV3%2fTrade%2fwxJump.aspx%3ftgid%3d8875803%26url%3dhttp%253a%252f%252fm.kingreader.com%252fClass%252fDefault.shtml%253ftk%253dcSdVAPfW3MR%25252fsHB6hxDU4aAV%25252fluXvp71gYd%25252fE9OIHGs%25253d%2526ak%253dtkr_web%2526tgid%253d8875803%2526ism%253d1%2526sc%253d640&response_type=code&scope=snsapi_userinfo&state=123#wechat_redirect\"}]},{\"name\":\"个人中心\",\"sub_button\":[{\"type\":\"view\",\"name\":\"个人中心\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx9dc24125364121ce&redirect_uri=http%3a%2f%2fapi2.kingreader.com%2fV3%2fTrade%2fwxJump.aspx%3ftgid%3d8875803%26url%3dhttp%253a%252f%252fm.kingreader.com%252fuser%252findex%253ftgid%253d8875803&response_type=code&scope=snsapi_userinfo&state=123#wechat_redirect\"},{\"type\":\"view\",\"name\":\"客户端下载\",\"url\":\"http://www.kingreader.com/mobile.html\"},{\"type\":\"view\",\"name\":\"快速充值\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx9dc24125364121ce&redirect_uri=http%3a%2f%2fapi2.kingreader.com%2fV3%2fTrade%2fwxJump.aspx%3ftgid%3d8875803%26url%3dhttp%253a%252f%252fm.kingreader.com%252fUser%252fPayway%253ftgid%253d8875803&response_type=code&scope=snsapi_userinfo&state=123#wechat_redirect\"}]}]}";
            if (!string.IsNullOrEmpty(menu_Json_String))
                postData = menu_Json_String;

            string rtnStr = HttpHelper.HttpPost(request_URL, postData);
            JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;
            return rtnStr;
        }

        //自定义菜单 查询接口 url
        private static string WX_URL_Menu_Get = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}";
        //自定义菜单 查询接口 [get]
        public static string WX_API_Menu_Get()
        {
            Access_Token = CheckTokenExpired(Access_Token);
            string request_URL = string.Format(WX_URL_Menu_Get, Access_Token);
            string rtnStr = HttpHelper.HttpGet(request_URL, "");
            JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;
            string MSG_Status = (jb["msg_status"] ?? "").ToString();
            return MSG_Status;
        }

        //自定义菜单 删除接口 url
        private static string WX_URL_Menu_Delete = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}";
        //自定义菜单 删除接口 [get]
        public static string WX_API_Menu_Delete(string msg_id)
        {
            Access_Token = CheckTokenExpired(Access_Token);
            string request_URL = string.Format(WX_URL_Menu_Delete, Access_Token);
            string rtnStr = HttpHelper.HttpGet(request_URL, "");
            JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;
            string MSG_Status = (jb["msg_status"] ?? "").ToString();
            return MSG_Status;
        }

        #endregion

        #region 客服管理
        //将消息转到多客服系统
        //如果公众号处于开发模式，需要在接收到用户发送的消息时，返回一个MsgType为transfer_customer_service的消息，微信服务器在收到这条消息时，会把这次发送的消息转到多客服系统
        //<xml><ToUserName><![CDATA[touser]]></ToUserName><FromUserName><![CDATA[fromuser]]></FromUserName><CreateTime>1399197672</CreateTime><MsgType><![CDATA[transfer_customer_service]]></MsgType></xml>
        public static string Transfer_Customer_Service(string ToUserName, string FromUserName, string CreateTime)
        {
            string xml = "<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime><MsgType><![CDATA[transfer_customer_service]]></MsgType></xml>";
            xml = string.Format(xml, ToUserName, FromUserName, CreateTime);
            Tracer.RunLog(MessageType.WriteInfomation, "", "log", "转入客服系统：" + xml + "\r\n");
            return xml;
        }

        //客服接口-发送文本 【post】
        public static string WX_API_Custom_SendMSG(string msg, string openid)
        {
            try
            {
                Access_Token = CheckTokenExpired(Access_Token);
                string request_URL = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";
                request_URL = string.Format(request_URL, Access_Token);
                var obj = new { touser = openid, msgtype = "text", text = new { content = msg } };
                string postData = TransformHelper.SerializeObject(obj);
                string rtnStr = HttpHelper.HttpPost(request_URL, postData);
                JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;
                //{ "errcode":45015,"errmsg":"response out of time limit or subscription is canceled hint: [dlrK0780age3]"}回复时间超过限制
                string errcode = (jb["errcode"] ?? "").ToString();
                if (errcode != "0")
                {
                    Tracer.RunLog(MessageType.WriteInfomation, "", "error", "WX_API_Custom_SendMSG  客服接口-发消息  异常= ：" + errcode + "\r\n"
                        + " request_URL = " + request_URL + "\r\n"
                        + " postData = " + postData + "\r\n"
                        );
                }
                return errcode;
            }
            catch (Exception ex)
            {
                Tracer.RunLog(MessageType.WriteInfomation, "", "error", "WX_API_Custom_SendMSG  客服接口-发消息  异常=  ：" + ex + "\r\n");
                return "";
            }
        }

        //客服接口-发送图片 【post】
        public static string WX_API_Custom_SendPicture(string mediaID, string openid)
        {
            try
            {
                Access_Token = CheckTokenExpired(Access_Token);
                string request_URL = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";
                request_URL = string.Format(request_URL, Access_Token);

                var obj = new { touser = openid, msgtype = "image", image = new { media_id = mediaID } };
                string postData = TransformHelper.SerializeObject(obj);
                string rtnStr = HttpHelper.HttpPost(request_URL, postData);
                JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;
                //{ "errcode":45015,"errmsg":"response out of time limit or subscription is canceled hint: [dlrK0780age3]"}回复时间超过限制
                string errcode = (jb["errcode"] ?? "").ToString();
                if (errcode != "0")
                {
                    Tracer.RunLog(MessageType.WriteInfomation, "", "error", "WX_API_Custom_SendMSG  客服接口-发消息  异常= ：" + errcode + "\r\n"
                        + " request_URL = " + request_URL + "\r\n"
                        + " postData = " + postData + "\r\n"
                        );
                }
                return errcode;
            }
            catch (Exception ex)
            {
                Tracer.RunLog(MessageType.WriteInfomation, "", "error", "WX_API_Custom_SendMSG  客服接口-发消息  异常=  ：" + ex + "\r\n");
                return "";
            }
        }

        //客服接口-发送图文 【post】点击跳转到图文消息页面
        public static string WX_API_Custom_SendNews_ByID(string newsId, string openid)
        {
            try
            {
                Access_Token = CheckTokenExpired(Access_Token);
                string request_URL = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";
                request_URL = string.Format(request_URL, Access_Token);
                var obj = new { touser = openid, msgtype = "mpnews", mpnews = new { media_id = newsId } };
                string postData = TransformHelper.SerializeObject(obj);
                string rtnStr = HttpHelper.HttpPost(request_URL, postData);
                JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;
                //{ "errcode":45015,"errmsg":"response out of time limit or subscription is canceled hint: [dlrK0780age3]"}回复时间超过限制
                string errcode = (jb["errcode"] ?? "").ToString();
                if (errcode != "0")
                {
                    Tracer.RunLog(MessageType.WriteInfomation, "", "error", "WX_API_Custom_SendMSG  客服接口-发消息  异常= ：" + errcode + "\r\n");
                }
                return errcode;
            }
            catch (Exception ex)
            {
                Tracer.RunLog(MessageType.WriteInfomation, "", "error", "WX_API_Custom_SendMSG  客服接口-发消息  异常=  ：" + ex + "\r\n");
                return "";
            }
        }

        //客服接口-发送图文 【post】 点击跳转到外链
        public static string WX_API_Custom_SendNews(List<WX_Model_Add_News_Article> articles, string openid)
        {
            try
            {
                List<object> arts = new List<object>();
                foreach (var item in articles)
                {
                    //object art = new { title=item.Title, description = item.Content, url = item.Content_Source_Url, picurl = item.Thumb_Url };
                    object art = new { title = item.Title, description = "", url = item.Content_Source_Url, picurl = item.Thumb_Url };
                    arts.Add(art);
                }
                Access_Token = CheckTokenExpired(Access_Token);
                string request_URL = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";
                request_URL = string.Format(request_URL, Access_Token);
                var obj = new { touser = openid, msgtype = "news", news = new { articles = arts } };
                string postData = TransformHelper.SerializeObject(obj);
                string rtnStr = HttpHelper.HttpPost(request_URL, postData);
                JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;
                //{ "errcode":45015,"errmsg":"response out of time limit or subscription is canceled hint: [dlrK0780age3]"}回复时间超过限制
                string errcode = (jb["errcode"] ?? "").ToString();
                if (errcode != "0")
                {
                    Tracer.RunLog(MessageType.WriteInfomation, "", "error", "WX_API_Custom_SendMSG  客服接口-发消息  异常= ：" + errcode + "\r\n");
                }
                return errcode;
            }
            catch (Exception ex)
            {
                Tracer.RunLog(MessageType.WriteInfomation, "", "error", "WX_API_Custom_SendMSG  客服接口-发消息  异常=  ：" + ex + "\r\n");
                return "";
            }
        }

        //客服接口-获取客服基本信息 【get】[每日500000次 用于验证access token是否过期 ]
        public static string WX_API_Custom_GetKfList()
        {
            try
            {
                Access_Token = CheckTokenExpired(Access_Token);
                string request_URL = "https://api.weixin.qq.com/cgi-bin/customservice/getkflist?access_token={0}";
                request_URL = string.Format(request_URL, Access_Token);
                string rtnStr = HttpHelper.HttpGet(request_URL, "");
                JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;
                string result = (jb["kf_list"] ?? "").ToString();
                return result;
            }
            catch (Exception ex)
            {
                Tracer.RunLog(MessageType.WriteInfomation, "", "error", "WX_API_Custom_GetKfList  异常 = ：" + ex + "\r\n");
                return "";
            }
        }

        #endregion

        #region 微信长连接转短链接

        /// 微信菜单上的链接 转换
        public static string Transfer_URL_LongToShort(string longURL)
        {
            string short_url = "";
            try
            {
                //Tracer.RunLog(MessageType.WriteInfomation, "", "log", "longURL = ：" + longURL + "\r\n");
                Access_Token = CheckTokenExpired(Access_Token);
                string request_URL = "https://api.weixin.qq.com/cgi-bin/shorturl?access_token={0}";
                request_URL = string.Format(request_URL, Access_Token);
                string postData = "{\"action\":\"long2short\",\"long_url\":\"" + longURL + "\"}";// post参数格式  {"msg_id":"201053012"}
                string rtnStr = HttpHelper.HttpPost(request_URL, postData);
                //Tracer.RunLog(MessageType.WriteInfomation, "", "log", "rtnStr = ：" + rtnStr + "\r\n");
                JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;
                //Tracer.RunLog(MessageType.WriteInfomation, "", "url", "Transfer_URL_LongToShort wx 转换异常 = ：" + jb + "\r\n");
                if (jb["short_url"] == null)//超过微信的每日调用次数1000 【转到百度的短链接】
                {
                    short_url = TransformHelper.ConvertToShortURL_RRDME(longURL);
                }
                else
                {
                    short_url = jb["short_url"].ToString();
                }
                return short_url;
            }
            catch (Exception ex)
            {
                Tracer.RunLog(MessageType.WriteInfomation, "", "error", "Transfer_URL_LongToShort转换异常 = ：" + ex + "\r\n");
                return longURL;
            }
        }
        #endregion

        #region 微信网页授权 来获取用户基本信息 实现自动登录

        public static string WX_API_MenuURL_OAuth(string originalURL)
        {
            string result_URL = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=123#wechat_redirect";
            originalURL = EncryptHelper.UrlEncode(originalURL);
            string temp_URL = "http://wxsm.kingreader.com/WX_CallBack/WxJump?tgid={0}&url={1}";
            temp_URL = string.Format(temp_URL, Tgid_WX, originalURL);
            temp_URL = EncryptHelper.UrlEncode(temp_URL);
            result_URL = string.Format(result_URL, AppID, temp_URL);
            return result_URL;
        }

        #endregion

        #region 用户管理
        //客服接口-获取客服基本信息 【get】[每日500000次 用于验证access token是否过期 ]
        public static List<string> WX_API_User_GetAll()
        {
            try
            {
                Access_Token = CheckTokenExpired(Access_Token);
                string base_URL = "https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}";
                string request_URL = string.Format(base_URL, Access_Token, "");
                string rtnStr = HttpHelper.HttpGet(request_URL, "");
                //{"total":2,"count":2,"data":{"openid":["","OPENID1","OPENID2"]},"next_openid":"NEXT_OPENID"}
                JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;
                int total = Convert.ToInt32(jb["total"].ToString());
                int count = Convert.ToInt32(jb["count"].ToString());
                string next_openid = (jb["next_openid"] ?? "").ToString();
                JObject data = JsonConvert.DeserializeObject(jb["data"].ToString()) as JObject;
                string openidString = data["openid"].ToString();
                List<string> openids = JsonConvert.DeserializeObject<List<string>>(openidString);

                //循环查询
                while (count == 10000)
                {
                    request_URL = string.Format(base_URL, Access_Token, next_openid);
                    rtnStr = HttpHelper.HttpGet(request_URL, "");
                    //{"total":2,"count":2,"data":{"openid":["","OPENID1","OPENID2"]},"next_openid":"NEXT_OPENID"}
                    jb = JsonConvert.DeserializeObject(rtnStr) as JObject;
                    count = Convert.ToInt32(jb["count"].ToString());
                    next_openid = (jb["next_openid"] ?? "").ToString();
                    data = JsonConvert.DeserializeObject(jb["data"].ToString()) as JObject;
                    openidString = data["openid"].ToString();
                    List<string> openids_temp = JsonConvert.DeserializeObject<List<string>>(openidString);
                    openids.AddRange(openids_temp);
                }

                return openids;
            }
            catch (Exception ex)
            {
                Tracer.RunLog(MessageType.WriteInfomation, "", "error", "WX_API_Custom_GetKfList  异常 = ：" + ex + "\r\n");
                return null;
            }
        }

        #endregion

        #region 卡券相关API


        #region 1.创建卡券

        #endregion


        #region 2.导入自定义code

        #endregion

        #region 3.线上核销卡券



        #endregion


        #endregion


        #region 模版消息

        public static string WX_Message_Template_Send(string touser, string template_id, string data)
        {
            Access_Token = CheckTokenExpired(Access_Token);
            string request_URL = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
            request_URL = string.Format(request_URL, Access_Token);
            //string media_id = "NV5LxfECHIR1F0HOM_B3tU0sZFdqtblnEfRL9YINFLM";
            // post参数格式  图文{"filter":{"is_to_all":false,"tag_id":2},"mpnews":{"media_id":"123dsdajkasd231jhksad"},"msgtype":"mpnews"}
            //               文本{"filter":{"is_to_all":false,"tag_id":2},"text":{"content":"CONTENT"},"msgtype":"text"}
            string postData = "{\"touser\":\""+ touser + "\",\"template_id\":\""+ template_id + "\",\"data\":"+data+"}";
            //.......

            //返回格式{"errcode":0,"errmsg":"send job submission success","msg_id":34182,"msg_data_id":206227730}
            string rtnStr = HttpHelper.HttpPost(request_URL, postData);
            string msgid = GetJsonValue_JObject(rtnStr, "msgid");//(jb["media_id"] ?? "").ToString();     
            return msgid;
        }

        #endregion

        #region Enum

        // 媒体文件的类型
        public enum WX_MediaType
        {
            /// <summary>
            /// 图文[只存在于永久素材]
            /// </summary>
            news = -1,
            /// <summary>
            /// 图片
            /// </summary>
            image = 0,
            /// <summary>
            /// 语音
            /// </summary>
            voice = 1,
            /// <summary>
            /// 视频
            /// </summary>
            video = 2,
            /// <summary>
            /// 缩略图[主要用于视频与音乐格式的缩略图]
            /// </summary>
            thumb = 3
        }


        // 群发消息的类型
        public enum WX_MessageType
        {
            // 图文[只存在于永久素材]
            mpnews = -1,
            // 图片
            image = 0,
            // 语音
            voice = 1,
            // 视频
            video = 2,
            // 纯文本
            text = 3,
            // 卡券
            wxcard = 4
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

        #region 微信通用model

        //被动回复用户消息 基本 model
        public class WX_BaseModel_ReplyMSG
        {
            public string ToUserName { get; set; }
            public string FromUserName { get; set; }
            public int CreateTime { get; set; }
            public string MsgType { get; set; }
        }

        //被动回复用户消息 文本 model
        public class WX_Model_ReplyMSG_Text : WX_BaseModel_ReplyMSG
        {
            public string Content { get; set; }
        }

        //被动回复用户消息 图文 model
        public class WX_Model_ReplyMSG_News : WX_BaseModel_ReplyMSG
        {
            public string ArticleCount { get; set; }
            public List<WX_Model_ReplyMSG_News_Article> Articles { get; set; }
        }
        public class WX_Model_ReplyMSG_News_Article
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string PicUrl { get; set; }
            public string Url { get; set; }
        }

        public class WX_Model_Add_News_Article
        {
            public string Title { get; set; }
            public string Thumb_Media_ID { get; set; }
            public string Thumb_Url { get; set; }
            public string Author { get; set; }
            public string Digest { get; set; }

            public string Show_Cover_Pic { get; set; }
            public string Content { get; set; }
            public string Content_Source_Url { get; set; }
            public string Url { get; set; }//图文页的URL 【图文的预览地址】 [微信里面点击进入的url]
        }
        public class WX_Model_Material_Count
        {
            public int Voice_Count { get; set; }
            public int Video_Count { get; set; }
            public int Image_Count { get; set; }
            public int News_Count { get; set; }
        }

        public class WX_BaseModel_UserInfo
        {
            public string Token { get; set; }
            public Nullable<System.DateTime> TokenExpire { get; set; }
            public string Openid { get; set; }
            public string NickName { get; set; }
            public Nullable<int> Sex { get; set; }
            public string Unionid { get; set; }
            public string Language { get; set; }
            public string City { get; set; }
            public string Province { get; set; }
            public string Country { get; set; }
            public string Headimgurl { get; set; }
            public Nullable<System.DateTime> Subscribe_time { get; set; }
            public string Remark { get; set; }
            public Nullable<int> Groupid { get; set; }
            public string Tagid_list { get; set; }

            public int UserID { get; set; }//注册id
            public string Tgid { get; set; }//推广id

            public string Subscribe { get; set; }//是否关注 0.未关注 1.已关注

            public string promoterId { get; set; }//推广员ID
            public string channelName { get; set; }//渠道名称
        }

        public class WX_Model_AccessToken
        {
            public string AccessToken { get; set; }
            public DateTime ExpireTime { get; set; }
        }


        #region 卡券 相关 model     

        #endregion

        #endregion
    }
}
