using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModels
{
    //entity 的基类
    public abstract class BaseEntity
    {
        public virtual long ID { get; set; }
        public virtual Nullable<int> DelTF { get; set; }
        public virtual System.DateTime CreateTime { get; set; }
        public virtual Nullable<System.DateTime> UpdateTime { get; set; }
    }
}
