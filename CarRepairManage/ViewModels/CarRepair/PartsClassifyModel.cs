using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class PartsClassifyModel
    {
        public long ID { get; set; }
        public long OptionID { get; set; }
        public string Content { get; set; }
        public string Remark { get; set; }
        public string PicURL { get; set; }
        public int Order { get; set; }
        public Nullable<int> DelTF { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }

        private string picURLShow;
        public string PicURLShow
        {

            get { return string.IsNullOrWhiteSpace(PicURL) ? "" : (ConfigurationManager.AppSettings["ImageShowURL"] ?? "") + PicURL.Replace("\\","/"); }
            set { picURLShow = value; }
        }
    }
}
