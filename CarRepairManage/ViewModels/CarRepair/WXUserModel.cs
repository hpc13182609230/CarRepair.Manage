using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class WXUserModel:BaseModel
    {
        public string Openid { get; set; }
        public string Unionid { get; set; }
        public string NickName { get; set; }
        public Nullable<int> Sex { get; set; }
        public string Language { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string Headimgurl { get; set; }
        public Nullable<System.DateTime> Subscribe_time { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> TokenExpire { get; set; }
        public Nullable<int> Groupid { get; set; }
        public string Tagid_list { get; set; }
        public DateTime LastActiveTime { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> TgID { get; set; }

    }
}
