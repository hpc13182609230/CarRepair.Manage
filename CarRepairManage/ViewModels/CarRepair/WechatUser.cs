using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class WechatUser:BaseModel
    {
        public string Openid { get; set; }
        public string Unionid { get; set; }
        public string Password { get; set; }
        public string EncryptKey { get; set; }
        public string NickName { get; set; }
        public Nullable<int> Sex { get; set; }
        public string Language { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string HeadImgUrl { get; set; }
        public System.DateTime SubScribeTime { get; set; }
        public string Remark { get; set; }
        public string Content { get; set; }
        public string LoginToken { get; set; }
        public string LoginTokenExpire { get; set; }
        public string TgID { get; set; }
        public int Statu { get; set; }
        public System.DateTime LastActiveTime { get; set; }
        public string ShareID { get; set; }
    }
}
