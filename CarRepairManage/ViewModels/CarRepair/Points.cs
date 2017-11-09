using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class Points : BaseModel
    {
        public long WechatUserID { get; set; }
        public int PointType { get; set; }
        public int point { get; set; }
        public int PointSum { get; set; }
        public string Remark { get; set; }
        public string Note { get; set; }


        private string createTimeFormat;
        public string CreateTimeFormat
        {

            get { return CreateTime.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { createTimeFormat = value; }
        }
    }
}
