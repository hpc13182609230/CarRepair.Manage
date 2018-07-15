using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class PartsCompanyModel:BaseModel
    {

        private string baseImageURL = ConfigurationManager.AppSettings["ImageShowURL_Mini"];

        public PartsCompanyModel(bool showMiniImage = true)
        {
            if (showMiniImage)//显示小图
            {
                baseImageURL = ConfigurationManager.AppSettings["ImageShowURL_Mini"];
            }
            else
            {
                baseImageURL = ConfigurationManager.AppSettings["ImageShowURL"];
            }
        }



        public string Name { get; set; }
        public string Content { get; set; }
        public string Remark { get; set; }
        public string PicURL { get; set; }
        public string Mobile { get; set; }
        public Nullable<int> PV { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public Nullable<int> IP { get; set; }//option 对应的 id
        public string Contract { get; set; }//openid
        public string Tel { get; set; }
        public string Phone { get; set; }
        public string LoginToken { get; set; }
        public Nullable<System.DateTime> ExpireTime { get; set; }//配件商 有效期
        public string codeID { get; set; }//所属省份的 codeid
        public string Address { get; set; }


        public int Order { get; set; }//配件商显示的顺序
        public long PartsClassifyID { get; set; }
        public long WechatID { get; set; }
        public string PartsClassifyIDNote { get; set; }//ip对应的地址


        private string picURLShow;
        public string PicURLShow
        {
            get { return string.IsNullOrWhiteSpace(PicURL) ? "" : string.Format(baseImageURL, PicURL.Replace("\\", "/")); }
            set { picURLShow = value; }
        }

    }
}
