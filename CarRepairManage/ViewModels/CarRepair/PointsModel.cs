using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class PointsModel
    {
        public long ID { get; set; }
        public long WechatUserID { get; set; }
        public int PointType { get; set; }
        public int point { get; set; }
        public int PointSum { get; set; }
        public string Remark { get; set; }
        public string Note { get; set; }
        public Nullable<int> DelTF { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}
