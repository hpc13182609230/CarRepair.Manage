using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class PartsClassifyModel:BaseModel
    {
        private string baseImageURL = ConfigurationManager.AppSettings["ImageShowURL_Mini"];
        public long OptionID { get; set; }
        public string Content { get; set; }
        public string Remark { get; set; }
        public string PicURL { get; set; }
        public int Order { get; set; }

        private string picURLShow;
        public string PicURLShow
        {

            get { return string.IsNullOrWhiteSpace(PicURL) ? "" :string.Format(baseImageURL, PicURL.Replace("\\", "/")); }
            set { picURLShow = value; }
        }
    }
}
