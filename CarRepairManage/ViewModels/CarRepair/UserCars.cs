using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class UserCars : BaseModel
    {

        public long WechatUserID { get; set; }
        public string Attribution { get; set; }
        public string CarType { get; set; }
        public string CarNO { get; set; }
        public string CarOwnerName { get; set; }
        public string CarOwnerTel { get; set; }
        public Nullable<System.DateTime> InsuranceTime { get; set; }
       
        public string Note { get; set; }

        private string insuranceTimeFormat;

        public string InsuranceTimeFormat
        {
            get { return (InsuranceTime??DateTime.Now).ToString("yyyy-MM-dd"); }
            set { insuranceTimeFormat = value; }
        }

    }
}
