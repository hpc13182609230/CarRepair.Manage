namespace EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class PartsCompanyBindWechatUser : BaseEntity
    {
        /// <summary>
        /// WechatUserID
        /// </summary>		
        private long _wechatuserid;
        public long WechatUserID
        {
            get { return _wechatuserid; }
            set { _wechatuserid = value; }
        }
        /// <summary>
        /// PartsCompanyID
        /// </summary>		
        private long _partscompanyid;
        public long PartsCompanyID
        {
            get { return _partscompanyid; }
            set { _partscompanyid = value; }
        }

        /// <summary>
        /// Content
        /// </summary>		
        private string _content;
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        /// <summary>
        /// Remark
        /// </summary>		
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
       
    }
}

