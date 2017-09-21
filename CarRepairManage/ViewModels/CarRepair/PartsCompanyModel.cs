using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class PartsCompanyModel
    {
        public long ID { get; set; }
        public Nullable<int> DelTF { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }

        public string Name { get; set; }
        public string Content { get; set; }
        public string Remark { get; set; }
        public string PicURL { get; set; }
        public string Mobile { get; set; }
        public Nullable<int> PV { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public Nullable<int> IP { get; set; }
        public string Contract { get; set; }
        public string Tel { get; set; }

    }
}
