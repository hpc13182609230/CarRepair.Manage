using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HelperLib
{
    public class RegexHelper
    {
        /// <summary>
        /// 是否为汉字
        /// </summary>
        /// <param name="text">文本</param>
        public static bool IsChinese(string text)
        {
            return Regex.IsMatch(text, @"^([\u4E00-\u9FA5]+)$", RegexOptions.ExplicitCapture);
        }

        /// <summary>
        /// 是否为汉字|字母
        /// </summary>
        /// <param name="text">文本</param>
        public static bool IsChineseAndLetters(string text)
        {
            return Regex.IsMatch(text, @"^([a-zA-Z\u4E00-\u9FA5]+)$", RegexOptions.ExplicitCapture);
        }

        /// <summary>
        /// 是否为邮编号码
        /// </summary>
        /// <param name="text">文本</param>
        public static bool IsZipCode(string text)
        {
            return Regex.IsMatch(text, @"^(\d{6})$", RegexOptions.ExplicitCapture);
        }

        /// <summary>
        /// 是否为身份证号码
        /// </summary>
        /// <param name="text">文本</param>
        public static bool IsID(string text)
        {
            return Regex.IsMatch(text, @"^(\d{17}[\d|X]|\d{15})$", RegexOptions.ExplicitCapture);
        }

        /// <summary>
        /// 是否为电话号码
        /// </summary>
        /// <param name="text">文本</param>
        public static bool IsPhone(string text)
        {
            return Regex.IsMatch(text, @"^(((\(\d{3,4}\))|(\d{3,4}-))?[1-9]\d{6,7})$", RegexOptions.ExplicitCapture);
        }

        /// <summary>
        /// 是否为邮箱地址
        /// </summary>
        /// <param name="text">文本</param>
        public static bool IsEmail(string text)
        {
            return Regex.IsMatch(text, @"^([\w\.\-]+@[\w\.\-]+\.[\w\.\-]+)$", RegexOptions.ExplicitCapture);
        }

        /// <summary>
        /// 是否为手机号码
        /// </summary>
        /// <param name="text">文本</param>
        public static bool IsMobile(string text)
        {
            return Regex.IsMatch(text, @"^(1[0-9]{10})$", RegexOptions.ExplicitCapture);
        }

        /// <summary>
        /// 是否为合法字符串
        /// </summary>
        /// <param name="text">文本</param>
        public static bool IsSafe(string text)
        {
            var regex =
                new Regex(
                    @"insert |delete |count\(|asc\(|mid\(|char\(|exec master|net user|net localgroup administrators|or |and |update |drop table|truncate |xp_cmdshell |create |alert |'",
                    RegexOptions.IgnoreCase);

            return regex.Match(text).Success ? false : true;
        }

        /// <summary>
        /// 是否为数字|逗号
        /// </summary>
        /// <param name="text">文本</param>
        public static bool IsNumAndComma(string text)
        {
            return Regex.IsMatch(text, @"^([\d,]+)$", RegexOptions.ExplicitCapture);
        }

        /// <summary>
        /// 是否为密码
        /// </summary>
        /// <param name="text">文本</param>
        public static bool IsPassword(string text)
        {
            return Regex.IsMatch(text, @"^([\S\s]{6,16})$", RegexOptions.ExplicitCapture);
        }
    }
}
