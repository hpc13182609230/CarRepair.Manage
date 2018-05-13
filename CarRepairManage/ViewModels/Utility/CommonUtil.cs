using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public  static class CommonUtil
    {
        public enum RedisKey
        {
            /// <summary>
            /// rediskey 配件商 地区分类
            /// </summary>
            PartsCompany_CodeID = 0,

            /// <summary>
            /// rediskey 微信推送 版本消息（BaseOptionsID =10  用户绑定）  地区分类
            /// </summary>
            PartsCompanyBindWechatUser_Date = 1
        }

    }
}
