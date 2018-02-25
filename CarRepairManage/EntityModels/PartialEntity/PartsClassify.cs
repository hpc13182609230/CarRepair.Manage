namespace EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class PartsClassify : BaseEntity
    {

        /// <summary>
        /// OptionID
        /// </summary>		
        private long _optionid;
        public long OptionID
        {
            get { return _optionid; }
            set { _optionid = value; }
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
        /// <summary>
        /// Order
        /// </summary>		
        private int _order;
        public int Order
        {
            get { return _order; }
            set { _order = value; }
        }
        /// <summary>
        /// PicURL
        /// </summary>		
        private string _picurl;
        public string PicURL
        {
            get { return _picurl; }
            set { _picurl = value; }
        }

    }
}

