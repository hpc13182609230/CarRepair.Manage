﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CarRepairEntities : DbContext
    {
        public CarRepairEntities()
            : base("name=CarRepairEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ManageMenu> ManageMenu { get; set; }
        public virtual DbSet<ManageUser> ManageUser { get; set; }
        public virtual DbSet<WechatUser> WechatUser { get; set; }
    }
}
