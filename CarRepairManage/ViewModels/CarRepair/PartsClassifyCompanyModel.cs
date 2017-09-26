using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class PartsClassifyCompanyModel
    {
        public long ID { get; set; }
        public long PartsCompanyID { get; set; }
        public string Note { get; set; }
        public Nullable<int> DelTF { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public string PartsClassifyIDs { get; set; }
    }
}
