namespace EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class PartsCompany : BaseEntity
    {
        /// <summary>
        /// codeID
        /// </summary>		
        private string _codeid;
        public string codeID
        {
            get { return _codeid; }
            set { _codeid = value; }
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
        /// Contract
        /// </summary>		
        private string _contract;
        public string Contract
        {
            get { return _contract; }
            set { _contract = value; }
        }
        /// <summary>
        /// Address
        /// </summary>		
        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; }
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
        /// PicURL
        /// </summary>		
        private string _picurl;
        public string PicURL
        {
            get { return _picurl; }
            set { _picurl = value; }
        }
        /// <summary>
        /// Mobile
        /// </summary>		
        private string _mobile;
        public string Mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
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
        /// Phone 用户 唯一标识 手机号 
        /// </summary>		
        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        /// <summary>
        /// LoginToken
        /// </summary>		
        private string _logintoken;
        public string LoginToken
        {
            get { return _logintoken; }
            set { _logintoken = value; }
        }

        /// <summary>
        /// PartsClassifyIDNote
        /// </summary>		
        private string _partsclassifyidnote;
        public string PartsClassifyIDNote
        {
            get { return _partsclassifyidnote; }
            set { _partsclassifyidnote = value; }
        }
        /// <summary>
        /// PartsClassifyID
        /// </summary>		
        private long _partsclassifyid;
        public long PartsClassifyID
        {
            get { return _partsclassifyid; }
            set { _partsclassifyid = value; }
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
        /// WechatID
        /// </summary>		
        private long _wechatid;
        public long WechatID
        {
            get { return _wechatid; }
            set { _wechatid = value; }
        }
        /// <summary>
        /// PV
        /// </summary>		
        private int _pv;
        public int PV
        {
            get { return _pv; }
            set { _pv = value; }
        }
        /// <summary>
        /// Latitude
        /// </summary>		
        private decimal _latitude;
        public decimal Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }
        /// <summary>
        /// Longitude
        /// </summary>		
        private decimal _longitude;
        public decimal Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }
        /// <summary>
        /// IP
        /// </summary>		
        private long _ip;
        public long IP
        {
            get { return _ip; }
            set { _ip = value; }
        }


    }
}

