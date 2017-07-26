using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HelperLib
{
    public class HttpHelper
    {
        public static string HttpPost(string Url, string postDataStr, int timeOut = 5000)
        {
            return HttpPost(Url, postDataStr, "application/x-www-form-urlencoded", timeOut);
        }
        public static string HttpPost(string Url, string postDataStr, string ContentType, int timeOut = 5000)
        {
            string str;
            HttpWebRequest request = null;
            try
            {
                Encoding encoding = Encoding.GetEncoding("utf-8");
                Uri requestUri = new Uri(Url);
                byte[] bytes = encoding.GetBytes(postDataStr);
                request = (HttpWebRequest)WebRequest.Create(requestUri);
                request.AllowAutoRedirect = false;
                request.ContentType = ContentType;
                request.Method = "POST";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; QQWubi 133; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; InfoPath.2)";
                request.ContentLength = bytes.Length;
                request.ServicePoint.Expect100Continue = false;
                request.CookieContainer = new CookieContainer();
                request.Timeout = timeOut;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                StreamReader stream = new StreamReader(response.GetResponseStream(), encoding);
                str = stream.ReadToEnd();
                response.Close();
                stream.Close();
            }
            catch (Exception ex)
            {
                str = "请求错误!";
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                }
            }
            return str;
        }

        public static string HttpGet(string Url, string postDataStr, string encoding = "UTF-8")
        {
            string str = string.Empty;
            HttpWebRequest request = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(Url + (string.IsNullOrEmpty(postDataStr) ? string.Empty : "?") + postDataStr);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8;";
                //request.Referer = "http://e.dangdang.com/html/reader.html;jsessionid=D66188339B8A00D1E88D64D51148C53B?productId=1900049115&bookUid=br.132380429822068594";
                request.Host = new Uri(Url).Host;
                request.KeepAlive = false;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.153 Safari/537.36 SE 2.X MetaSr 1.0";
                request.Timeout = 10000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    using (StreamReader reader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress), Encoding.GetEncoding(encoding)))
                    {
                        str = reader.ReadToEnd();
                    }
                }
                else
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding)))
                    {
                        str = reader.ReadToEnd();
                    }
                }
                return str;
            }
            catch (WebException ex)
            {
                WebResponse wr = ex.Response;

                Stream st = wr.GetResponseStream();
                StreamReader sr = new StreamReader(st, System.Text.Encoding.UTF8);
                string sError = sr.ReadToEnd();
                sr.Close();
                st.Close();
                //Log.Tracer.RunLog(Log.MessageType.WriteInfomation, "", "error",
                //  "HttpGet异常 ：ex=" + ex.Message + "\r\n"
                //  + "请求参数：Url=" + Url + "\r\n"
                //  + "请求参数：postDataStr=" + postDataStr + "\r\n"
                //  + "返回值：sError=" + sError + "\r\n"
                //  );
                return sError;
            }
            catch (Exception ex)
            {
                //Log.Tracer.RunLog(Log.MessageType.WriteInfomation, "", "error",
                //"HttpGet异常 ：ex=" + ex.Message + "\r\n"
                //+ "请求参数：Url=" + Url + "\r\n"
                //+ "请求参数：postDataStr=" + postDataStr + "\r\n"
                //);
                return "请求错误!" + ex.Message;
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                }
            }
        }

        public static string HttpPost_New(string Url, string postDataStr)
        {
            string str = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postDataStr.Length;
                StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.UTF8);
                writer.Write(postDataStr);
                writer.Flush();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string encoding = response.ContentEncoding;
                if (string.IsNullOrWhiteSpace(encoding))
                {
                    encoding = "UTF-8"; //默认编码  
                }
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                str = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                str = "请求错误!";
            }
            return str;
        }

        public static string Post_UTF8(string url, string param, Action<HttpStatusCode, string> onComplete = null, string contentType = "")
        {
            byte[] bufferBytes = Encoding.UTF8.GetBytes(param);

            var request = WebRequest.Create(url) as HttpWebRequest;//HttpWebRequest方法继承自WebRequest, Create方法在WebRequest中定义，因此此处要显示的转换
            request.Method = "POST";
            request.ContentLength = bufferBytes.Length;
            if (!String.IsNullOrWhiteSpace(contentType))
            {
                request.ContentType = contentType;
            }

            try
            {
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bufferBytes, 0, bufferBytes.Length);
                }
            }
            catch (WebException ex)
            {
                //Log.Tracer.RunLog(Log.MessageType.WriteInfomation, "", "error", "Post_UTF8 异常： " + " Message = " + ex.Message + "\r\n");
                return ex.Message;
            }

            return HttpRequest(request, onComplete);
        }

        public static string HttpGet_UTF8(string url, Action<HttpStatusCode, string> onComplete = null)
        {

            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentType = "text/html;charset=UTF-8;";
                return HttpRequest(request, onComplete);
            }
            catch (WebException ex)
            {
                //Log.Tracer.RunLog(Log.MessageType.WriteInfomation, "", "error", "HttpGet_UTF8 异常： " + " Message = " + ex.Message + "\r\n");
                return "";
            }
        }

        private static string HttpRequest(HttpWebRequest request, Action<HttpStatusCode, string> onComplete = null)
        {
            HttpWebResponse response = null;

            try
            {
                response = request.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                //Log.Tracer.RunLog(Log.MessageType.WriteInfomation, "", "error",
                //"Post_UTF8 HttpRequest 异常： " + System.Reflection.MethodBase.GetCurrentMethod().Name + " Message = " + ex.Message + "\r\n");
                response = ex.Response as HttpWebResponse;
            }

            if (response == null)
            {
                if (onComplete != null)
                    onComplete(HttpStatusCode.NotFound, "请求远程返回为空");
                return null;
            }

            string result = string.Empty;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                result = reader.ReadToEnd();
            }

            if (onComplete != null)
                onComplete(response.StatusCode, result);

            return result;

        }


    }
}
