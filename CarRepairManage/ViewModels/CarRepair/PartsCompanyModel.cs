using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class PartsCompanyModel
    {
        public long ID { get; set; }
        public Nullable<int> DelTF { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }

        public string Name { get; set; }
        public string Content { get; set; }
        public string Remark { get; set; }
        public string PicURL { get; set; }
        public string Mobile { get; set; }
        public Nullable<int> PV { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public Nullable<int> IP { get; set; }//option 对应的 id
        public string Contract { get; set; }
        public string Tel { get; set; }
        public Nullable<System.DateTime> ExpireTime { get; set; }//配件商 有效期
       

        //额外字段
        public string Address { get; set; }//ip对应的地址


        public int Order { get; set; }//配件商显示的顺序
        public long PartsClassifyID { get; set; }
        public long WechatID { get; set; }
        public string PartsClassifyIDNote { get; set; }//ip对应的地址


        private string picURLShow;
        public string PicURLShow
        {

            get { return string.IsNullOrWhiteSpace(PicURL) ? "" : (ConfigurationManager.AppSettings["ImageShowURL"] ?? "") + PicURL.Replace("\\","/"); }
            set { picURLShow = value; }
        }

    }
}
