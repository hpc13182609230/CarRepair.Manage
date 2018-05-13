using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public abstract class BaseModel
    {
        public long ID { get; set; }
	
        private Nullable<int> _deltf;
        public Nullable<int> DelTF
        {
            get { return _deltf ?? 0; }
            set { _deltf = value; }
        }

        public System.DateTime CreateTime { get; set; }

        private Nullable<DateTime> _updatetime;
        public Nullable<DateTime> UpdateTime
        {
            get { return _updatetime ?? DateTime.MinValue; }
            set { _updatetime = value; }
        }

    }
}
