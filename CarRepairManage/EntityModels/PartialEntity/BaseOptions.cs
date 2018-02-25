namespace EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class BaseOptions:BaseEntity
    {
        public long ParentID { get; set; }
        public string Content { get; set; }
        public string Remark { get; set; }
    }
}

