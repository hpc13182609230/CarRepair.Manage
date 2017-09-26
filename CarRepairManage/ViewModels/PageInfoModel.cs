using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class PageInfoModel
    {
        //private int pageIndex;
        //public int PageIndex { get { return pageIndex; } set { pageIndex = value == 0 ? 1 : value; } }

        //private int pageSize;
        //public int PageSize { get { return pageSize; } set { pageSize = value == 0 ? 10 : value; } }

        public int Start { get; set; }
        public int Offset { get; set; }
        public long Total { get; set; }
    }
}
