namespace EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class PurchaseOrder : BaseEntity
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
        /// Price
        /// </summary>		
        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }
        /// <summary>
        /// Tel
        /// </summary>		
        private string _tel;
        public string Tel
        {
            get { return _tel; }
            set { _tel = value; }
        }
        /// <summary>
        /// Name
        /// </summary>		
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
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
        /// Statu
        /// </summary>		
        private int _statu;
        public int Statu
        {
            get { return _statu; }
            set { _statu = value; }
        }
        /// <summary>
        /// OrderTime
        /// </summary>		
        private DateTime _ordertime;
        public DateTime OrderTime
        {
            get { return _ordertime; }
            set { _ordertime = value; }
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

