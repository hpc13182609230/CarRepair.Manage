using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HelperLib
{
    public static class EncryptHelper
    {

        #region MD5加密
        //没有 编码方式的 MD5 如果有汉字 则编码结果不一样
        [Obsolete]
        public static string Md5EncryptStr32(string str)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            // Convert the input string to a byte array and compute the hash.  
            char[] temp = str.ToCharArray();
            byte[] buf = new byte[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                buf[i] = (byte)temp[i];
            }
            byte[] data = md5Hasher.ComputeHash(buf);
            // Create a new Stringbuilder to collect the bytes  
            // and create a string.  
            StringBuilder sBuilder = new StringBuilder();
            // Loop through each byte of the hashed data   
            // and format each one as a hexadecimal string.  
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.  
            return sBuilder.ToString();
        }

        //有 默认的编码 
        public static string MD5_UTF8(string unsignStr, Encoding encoding)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(encoding.GetBytes(unsignStr));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        public static string MD5Encrypt(string sourceStr)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bys = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(sourceStr));
            return BitConverter.ToString(bys).Replace("-", "");
        }
        #endregion

        #region base64

        //字符串转为base64字符串
        public static string ConvertToBase64(string str)
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(str))
            {
                byte[] b = Encoding.UTF8.GetBytes(str);
                result = Convert.ToBase64String(b);
            }
            return result;
        }

        // 将Base64编码的文本转换成普通文本
        public static string DeConvertToBase64(string base64)
        {
            if (!string.IsNullOrWhiteSpace(base64))
            {
                char[] charBuffer = base64.ToCharArray();
                byte[] bytes = Convert.FromBase64CharArray(charBuffer, 0, charBuffer.Length);
                string result = Encoding.UTF8.GetString(bytes);
                return result;
            }
            return "";
        }

        //base64密钥
        private static string encryptKey = "Tkr0";
        /// <summary>
        /// base64解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DecryptBase64(string str, string keyString)
        {
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();     //实例化加/解密类对象    
            if (keyString == "")
            {
                keyString = encryptKey;
            }

            byte[] key = Encoding.Unicode.GetBytes(keyString);
            //定义字节数组，用来存储密钥    
            byte[] data = Convert.FromBase64String(str);
            //定义字节数组，用来存储要解密的字符串    
            MemoryStream MStream = new MemoryStream();
            //实例化内存流对象    
            //使用内存流实例化解密流对象    
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length);
            //向解密流中写入数据    
            CStream.FlushFinalBlock();
            //释放解密流    
            return Encoding.Unicode.GetString(MStream.ToArray());             //返回解密后的字符串    
        }

        #endregion

        #region UrlEncode编码/解码
        public static string UrlEncode(string str)
        {
            string result = HttpUtility.UrlEncode(str, System.Text.Encoding.UTF8); //编码
            return result;
        }

        public static string UrlDecode(string str)
        {
            string result = HttpUtility.UrlDecode(str, System.Text.Encoding.UTF8);  //解码
            return result;
        }

        #endregion

        #region sha1
        public static string EncryptToSHA1(string str, Encoding encode, bool isUpper = false)
        {
            try
            {
                if (encode == null)
                    encode = Encoding.UTF8;
                SHA1 sha1 = SHA1.Create();
                byte[] bytes_in = encode.GetBytes(str);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                string result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1加密出错：" + ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 解析Token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="deviceId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool DecryptToken(string token, out long deviceId, out long userId)
        {
            deviceId = 0;
            userId = 0;
            bool m_result = true;

            try
            {
                //token = "Ijk0dXNSATbwu81ncNqh7mtv3a1FnFLqFOgG6s0W0H0U0j1%2b0oh2z0Cw8BTFVaBYeShIiSgpsgL%2f66MBos2ppg%3d%3d";
                token = UrlDecode(token);
                string output = DecryptBase64(token, "");
                if (output.Contains("_"))
                {
                    string[] m_outs = output.Split('_');
                    if (m_outs.Length == 3)
                    {
                        if (long.TryParse(m_outs[1], out deviceId) && long.TryParse(m_outs[2], out userId))
                        {
                            m_result = true;
                        }
                        else
                        {
                            m_result = false;
                        }
                    }
                    else
                    {
                        m_result = false;
                    }
                }
                else
                {
                    m_result = false;
                }
            }
            catch
            {
                m_result = false;
            }

            return m_result;
        }

        #region Oauth 验证
        /// <summary>
        /// ECOS.md5算法是否成功
        /// </summary>
        /// <param name="request">请求信息</param>
        /// <param name="secret">密钥</param>
        /// <returns>是否成功</returns>
        public static bool ECOSmd5IsSuccess(NameValueCollection request, string secret)
        {
            if (request.Count < 3 || string.IsNullOrWhiteSpace(secret))
                return false;
            string sign = "";
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (string key in request.Keys)
            {
                if (key.Equals("Sign"))
                    sign = request[key];
                else
                    dictionary.Add(key, request[key]);
            }
            if (string.IsNullOrWhiteSpace(sign) || sign.Length != 32)
                return false;
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(dictionary);
            StringBuilder signString = new StringBuilder();
            foreach (string key in sortedParams.Keys)
            {
                signString.Append(key).Append(sortedParams[key]);
            }
            string md5 = MD5_UTF8(signString.ToString(),null).ToUpper();
            string sign1 = MD5_UTF8(md5 + secret,null).ToUpper();
            return sign.Equals(sign1);
        }

        #endregion

    }
}
