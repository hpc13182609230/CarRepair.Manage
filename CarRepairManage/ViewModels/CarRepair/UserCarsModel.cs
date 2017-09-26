using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class UserCarsModel
    {
        public long ID { get; set; }
        public long WechatUserID { get; set; }
        public string Attribution { get; set; }
        public string CarType { get; set; }
        public string CarNO { get; set; }
        public string CarOwnerName { get; set; }
        public Nullable<int> CarOwnerTel { get; set; }
        public Nullable<System.DateTime> InsuranceTime { get; set; }
        public string Note { get; set; }
        public Nullable<int> DelTF { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}
