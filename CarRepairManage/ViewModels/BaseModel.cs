﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public abstract class BaseModel
    {
        public long ID { get; set; }
        public Nullable<int> DelTF { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}