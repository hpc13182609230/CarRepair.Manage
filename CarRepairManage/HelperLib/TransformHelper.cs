using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml;

namespace HelperLib
{

    public class TransformHelper
    {
        #region XML <=> JSON <=> STRING 
        ///xml转换成json
        public static string Convert_Xml2Json(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string json = JsonConvert.SerializeXmlNode(doc);
            return json;
        }

        /// json字符串转换为Xml对象   
        public static XmlDocument Convert_Json2Xml(string sJson)
        {
            //XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(sJson), XmlDictionaryReaderQuotas.Max);  
            //XmlDocument doc = new XmlDocument();  
            //doc.Load(reader);  
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();//这里需要引用 System.Web.Extensions.dll 
            Dictionary<string, object> Dic = (Dictionary<string, object>)oSerializer.DeserializeObject(sJson);
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDec;
            xmlDec = doc.CreateXmlDeclaration("1.0", "gb2312", "yes");
            doc.InsertBefore(xmlDec, doc.DocumentElement);
            XmlElement nRoot = doc.CreateElement("root");
            doc.AppendChild(nRoot);
            foreach (KeyValuePair<string, object> item in Dic)
            {
                XmlElement element = doc.CreateElement(item.Key);
                KeyValue2Xml(element, item);
                nRoot.AppendChild(element);
            }
            return doc;
        }

        private static void KeyValue2Xml(XmlElement node, KeyValuePair<string, object> Source)
        {
            object kValue = Source.Value;
            if (kValue.GetType() == typeof(Dictionary<string, object>))
            {
                foreach (KeyValuePair<string, object> item in kValue as Dictionary<string, object>)
                {
                    XmlElement element = node.OwnerDocument.CreateElement(item.Key);
                    KeyValue2Xml(element, item);
                    node.AppendChild(element);
                }
            }
            else if (kValue.GetType() == typeof(object[]))
            {
                object[] o = kValue as object[];
                for (int i = 0; i < o.Length; i++)
                {
                    XmlElement xitem = node.OwnerDocument.CreateElement("Item");
                    KeyValuePair<string, object> item = new KeyValuePair<string, object>("Item", o[i]);
                    KeyValue2Xml(xitem, item);
                    node.AppendChild(xitem);
                }

            }
            else
            {
                XmlText text = node.OwnerDocument.CreateTextNode(kValue.ToString());
                node.AppendChild(text);
            }
        }

        /// stream 转换成 XML

        public static string Convert_Stream2Xml(Stream stream)
        {
            //获取流
            //Stream s = System.Web.HttpContext.Current.Request.InputStream;

            //转换成Byte数组
            byte[] b = new byte[stream.Length];
            //读取流
            stream.Read(b, 0, (int)stream.Length);
            //转化成utf8编码
            string postStr = Encoding.UTF8.GetString(b);
            return postStr;
        }

        /// stream 转换成 json
        public static string Convert_Stream2Json(Stream stream)
        {
            string xml = Convert_Stream2Xml(stream);
            //Log.Tracer.RunLog(Log.MessageType.WriteInfomation, "", "log", "XML="+xml + "\r\n");
            string json = Convert_Xml2Json(xml);
            return json;
        }

        //除去json串中的cdata
        public static string Json_Remove_CData(string json)
        {
            if (json.Contains("#cdata-section"))
            {
                if (json.Contains("xml"))
                {
                    json = json.Replace("{\"xml\":", "");
                    json = json.Substring(0, json.Length - 1);
                }
                //判断最后一个属性中是否含有cdata
                JObject jb = JsonConvert.DeserializeObject(json) as JObject;
                List<JProperty> properties = jb.Properties().ToList();
                var len = properties.Count();
                string name = properties[len - 1].Name;
                JToken _JToken = properties[len - 1].Value;
                bool lastPropertyHasCData = _JToken.ToString().Contains("#cdata-section");
                json = json.Replace("{\"#cdata-section\":", "");
                json = json.Replace("},", ",");
                if (lastPropertyHasCData)
                {
                    json = json.Substring(0, json.Length - 1);//最后一个属性中是否含有cdata 会多出一个{
                }
            }
            return json;
        }
        #endregion

        #region  时间戳 <=> DateTime格式

        public static DateTime Convert_TimeStamp2DateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// DateTime时间格式转换为Unix时间戳格式
        public static int Convert_DateTime2Int(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }


        public static long Convert_DateTime2Int64(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalSeconds;
        }
        #endregion

        #region 对象B <=> 对象A

        public static T ConvertBToA<T>(T A, object B)
        {
            PropertyInfo[] bizPropertys = B.GetType().GetProperties();
            PropertyInfo[] dbPropertys = typeof(T).GetProperties();
            //将Model中所有属性复制给日志对象
            foreach (var bizProperty in bizPropertys)
            {
                ////排除Id
                //if (bizProperty.Name.ToLower() == "id")
                //    continue;

                //忽略不可写的属性
                if (!bizProperty.CanWrite)
                    continue;

                object modelValue = bizProperty.GetValue(B, null);
                foreach (PropertyInfo dbProperty in dbPropertys)
                {
                    if (dbProperty.Name.ToLower() == bizProperty.Name.ToLower())
                    {
                        // log为空
                        if (dbProperty.PropertyType.IsGenericType
                            && dbProperty.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            // model为空
                            if (bizProperty.PropertyType.IsGenericType &&
                                bizProperty.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                dbProperty.SetValue(A, modelValue, null);
                            }
                            else // model不为空
                            {
                                if (modelValue != null)
                                    dbProperty.SetValue(A, Convert.ChangeType(modelValue, dbProperty.PropertyType.GetGenericArguments()[0]), null);
                            }
                        }
                        else// log不为空
                        {
                            // model为空
                            if (bizProperty.PropertyType.IsGenericType &&
                                bizProperty.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                dbProperty.SetValue(A, Convert.ChangeType(modelValue, dbProperty.PropertyType.GetGenericArguments()[0]), null);
                            }
                            else // model都不为空
                            {
                                dbProperty.SetValue(A, Convert.ChangeType(modelValue, dbProperty.PropertyType), null);
                            }
                        }
                    }

                }
            }
            return A;
        }
        #endregion

        #region json <=> object
        // 从一个对象信息生成Json串
        public static string SerializeObject(object obj)
        {
            //DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            //MemoryStream stream = new MemoryStream();
            //serializer.WriteObject(stream, obj);
            //byte[] dataBytes = new byte[stream.Length];
            //stream.Position = 0;
            //stream.Read(dataBytes, 0, (int)stream.Length);
            //return Encoding.UTF8.GetString(dataBytes);
            return JsonConvert.SerializeObject(obj);
        }

        // 从一个Json串生成对象信息
        public static T DeserializeObject<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
        #endregion

        #region json <=> Dictionary<string,string>
        //public static string Convert_Json2Dic(string json)
        //{
        //    //Dictionary<string,string> dic=
        //    return postStr;
        //}

        #endregion

        #region 长链接<=>短链接 【百度API】 【rrd.me】
        //长链接转短链接
        public static string ConvertToShortURL_Baidu(string inputURL)
        {
            try
            {
                string request_URL = "http://dwz.cn/create.php";
                //此处的请求数据格式不是json 
                //不能用 string postData = "{\"url\":\""+EncryptHelper.UrlEncode("http://blog.csdn.net/wdw984/article/details/6639829")+"\"}";
                string postData = "url={0}";
                postData = string.Format(postData, EncryptHelper.UrlEncode(inputURL));//链接地址需要进行UrlEncode处理 

                string rtnStr = HttpHelper.HttpPost(request_URL, postData);
                JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;
                //Log.Tracer.RunLog(Log.MessageType.WriteInfomation, "", "url", "Transfer_URL_LongToShort bd 转换异常 = ：" + jb + "\r\n");
                string resultURL = (jb["tinyurl"] ?? inputURL).ToString();
                if (jb["status"].ToString() != "0")
                {
                    //Log.Tracer.RunLog(Log.MessageType.WriteInfomation, "", "error", "调用百度短网址API  请求错误= ：" + jb["err_msg"].ToString() + "\r\n");
                }
                return resultURL;
            }
            catch (Exception ex)
            {
                //Log.Tracer.RunLog(Log.MessageType.WriteInfomation, "", "error", "调用百度短网址API  内部错误 = ：" + ex + "\r\n");
                return inputURL;
            }
        }

        //短链回复成原始的链接
        public static string ConvertToLongURL_Baidu(string inputURL)
        {
            try
            {
                string request_URL = "http://dwz.cn/query.php";
                //此处的请求数据格式不是json 
                //不能用 string postData = "{\"url\":\""+EncryptHelper.UrlEncode("http://blog.csdn.net/wdw984/article/details/6639829")+"\"}";
                string postData = "tinyurl={0}";
                postData = string.Format(postData, inputURL);

                string rtnStr = HttpHelper.HttpPost(request_URL, postData);
                JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;
                string resultURL = (jb["longurl"] ?? inputURL).ToString();
                if (jb["status"].ToString() != "0")
                {
                    //Log.Tracer.RunLog(Log.MessageType.WriteInfomation, "", "error", "调用百度短网址API  请求错误= ：" + jb["err_msg"].ToString() + "\r\n");
                }
                return resultURL;
            }
            catch (Exception ex)
            {
                //Log.Tracer.RunLog(Log.MessageType.WriteInfomation, "", "error", "调用百度短网址API  内部错误 = ：" + ex + "\r\n");
                return inputURL;
            }
        }

        //长链接转短链接
        public static string ConvertToShortURL_RRDME(string inputURL)
        {
            try
            {
                string request_URL = "http://rrd.me/api.php?format=json&url={0}";//
                request_URL = string.Format(request_URL, EncryptHelper.UrlEncode(inputURL));//链接地址需要进行UrlEncode处理 


                string rtnStr = HttpHelper.HttpGet(request_URL, "").Substring(3);
                //rtnStr = rtnStr.Replace("\\","");
                JObject jb = JsonConvert.DeserializeObject(rtnStr) as JObject;
                //Log.Tracer.RunLog(Log.MessageType.WriteInfomation, "", "url", "Transfer_URL_LongToShort bd 转换异常 = ：" + jb + "\r\n");
                string resultURL = (jb["url"] ?? inputURL).ToString();
                if (jb["error"].ToString() != "0")
                {
                    //Log.Tracer.RunLog(Log.MessageType.WriteInfomation, "", "error", "调用 RRDME 短网址API  请求错误= ：" + jb["err_msg"].ToString() + "\r\n");
                }
                return resultURL;
            }
            catch (Exception ex)
            {
                //Log.Tracer.RunLog(Log.MessageType.WriteInfomation, "", "error", "调用 RRDME 短网址API  内部错误 = ：" + ex + "\r\n");
                return inputURL;
            }
        }

        #endregion


    }

}
