namespace EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class WXMenu : BaseEntity
    {
        public long ParentID { get; set; }
        public string MenuType { get; set; }
        public string Name { get; set; }
        public string KeyForClick { get; set; }
        public string Url { get; set; }
        public string MediaID { get; set; }
    }
}

