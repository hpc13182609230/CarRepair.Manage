namespace EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class ZTest : BaseEntity
    {
        /// <summary>
        /// ParentID
        /// </summary>		
        private long _parentid;
        public long ParentID
        {
            get { return _parentid; }
            set { _parentid = value; }
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

