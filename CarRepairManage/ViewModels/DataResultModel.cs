using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class DataResultModel
    {
        public DataResultModel()
        {
            this.rs = 1;
            this.em = "OK";
        }

        public int rs { get; set; }
        public string em { get; set; }
        public object dt { get; set; }

        //public double Count { get; set; }
    }
}
