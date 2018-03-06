using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Utility
{
    public  class CacheModel
    {
        /// <summary>
        /// 地域 省级 缓存
        /// </summary>
        public static string CacheKey_Area_Province = "Area:Province";

        public static List<string> NotShowProvince = new List<string>() { "西藏自治区", "宁夏回族自治区", "新疆维吾尔自治区", "台湾省", "香港特别行政区", "澳门特别行政区" , "青海省", "内蒙古自治区" };
    }
}
