using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class WXArticleModel
    {
        public int ID { get; set; }
        public string NewsID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Digests { get; set; }
        public string Content { get; set; }
        public string Content_Source_Url { get; set; }
        public string Thumb_Media_ID { get; set; }
        public string Thumb_Url { get; set; }
        public string Show_Cover_Pic { get; set; }
        public string Url { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }

        public string Thumb_Url_Show { get; set; }
    }
}
