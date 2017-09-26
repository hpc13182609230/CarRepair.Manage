using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public  class PurchaseOrderModel
    {
        public long ID { get; set; }
        public long WechatUserID { get; set; }
        public long PartsCompanyID { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Remark { get; set; }
        public int Statu { get; set; }
        public System.DateTime OrderTime { get; set; }
        public string PicURL { get; set; }
        public Nullable<int> DelTF { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}
