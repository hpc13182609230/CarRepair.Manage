using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class RepairOrderModel
    {
        public long ID { get; set; }
        public long WechatUserID { get; set; }
        public string PurchaseOrderID { get; set; }
        public long UserCarID { get; set; }
        public string Note { get; set; }
        public string Contract { get; set; }
        public System.DateTime RepairTime { get; set; }
        public string Tel { get; set; }
        public string CarNo { get; set; }
        public Nullable<int> DelTF { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public decimal Price { get; set; }
        public decimal Debt { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal CostEvaluation { get; set; }

        private string repairTimeFormat;
        public string RepairTimeFormat
        {
            get { return RepairTime.ToString("yyyy-MM-dd"); }
            set { repairTimeFormat = value; }
        }


        public PurchaseOrderModel PurchaseOrder { get; set; }
    }
}
