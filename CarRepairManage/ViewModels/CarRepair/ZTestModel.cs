using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class ZTestModel:BaseModel
    {
        public Nullable<int> ParentID { get; set; }
        public string Content { get; set; }
        public string Remark { get; set; }
    }
}
