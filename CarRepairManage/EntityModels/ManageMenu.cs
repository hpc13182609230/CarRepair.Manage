//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntityModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class ManageMenu
    {
        public int ID { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Info { get; set; }
        public string Content { get; set; }
        public string Icon { get; set; }
        public string Permission { get; set; }
        public Nullable<int> DelTF { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}
