using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class AreaModel:BaseModel
    {
        public string codeID { get; set; }
        public string name { get; set; }
        public string parentID { get; set; }
    }
}