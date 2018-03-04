using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class WXMenuModel : BaseModel
    {
        public long ParentID { get; set; }
        public string MenuType { get; set; }
        public string Name { get; set; }
        public string KeyForClick { get; set; }
        public string Url { get; set; }
        public string MediaID { get; set; }
    }
}
