using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public  class PurchaseOrder : BaseModel
    {

        public long WechatUserID { get; set; }
        public long PartsCompanyID { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public string Content { get; set; }
        public string Remark { get; set; }
        public int Statu { get; set; }
        public System.DateTime OrderTime { get; set; }
        public string PicURL { get; set; }

        private string picURLShow;
        public string PicURLShow
        {

            get { return string.IsNullOrWhiteSpace(PicURL) ? "" : (ConfigurationManager.AppSettings["ImageShowURL"] ?? "") + PicURL.Replace("\\", "/"); }
            set { picURLShow = value; }
        }


        private string orderTimeFormat;
        public string OrderTimeFormat
        {
            
            get { return OrderTime.ToString("yyyy-MM-dd"); }
            set { orderTimeFormat = value; }
        }

    }
}
