using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class BaseOptionsModel
    {
        public long ID { get; set; }
        public Nullable<int> DelTF { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }

        public Nullable<int> ParentID { get; set; }
        public string Content { get; set; }
        public string Remark { get; set; }
    }
}
