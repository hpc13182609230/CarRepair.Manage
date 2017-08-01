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
            this.result = 1;
            this.message = "OK";
        }

        public int result { get; set; }
        public string message { get; set; }
        public object data { get; set; }

        //public double Count { get; set; }
    }
}
