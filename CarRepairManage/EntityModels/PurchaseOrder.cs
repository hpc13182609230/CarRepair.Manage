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
    
    public partial class PurchaseOrder : BaseEntity
    {
        public long WechatUserID { get; set; }
        public long PartsCompanyID { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Remark { get; set; }
        public int Statu { get; set; }
        public System.DateTime OrderTime { get; set; }
        public string PicURL { get; set; }
    }
}
