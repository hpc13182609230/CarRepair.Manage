﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class UserCarsModel:BaseModel
    {
        public long WechatUserID { get; set; }
        public string Attribution { get; set; }
        public string AttributionChar { get; set; }
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


        /// <summary>
        /// InsuranceMonth
        /// </summary>		
        private int _insurancemonth;
        public int InsuranceMonth
        {
            get
            {
                //return _insurancemonth;
                if (_insurancemonth > 0)
                {
                    return _insurancemonth;
                }
                else
                {
                    return InsuranceTime == null ? 1 : Convert.ToDateTime(InsuranceTime).Month;
                }

            }
            set { _insurancemonth = value; }
        }

    }
}
