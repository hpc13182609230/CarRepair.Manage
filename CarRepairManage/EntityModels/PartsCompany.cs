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
    
    public partial class PartsCompany : BaseEntity
    {

        public string Name { get; set; }
        public string Content { get; set; }
        public string Remark { get; set; }
        public string PicURL { get; set; }
        public string Mobile { get; set; }
        public int PV { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int IP { get; set; }
        public string Contract { get; set; }
        public string Tel { get; set; }
        public string PartsClassifyIDNote { get; set; }
        public long PartsClassifyID { get; set; }
        public int Order { get; set; }
        public long WechatID { get; set; }
    }
}
