namespace EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class RepairOrder : BaseEntity
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
        /// PurchaseOrderID
        /// </summary>		
        private string _purchaseorderid;
        public string PurchaseOrderID
        {
            get { return _purchaseorderid; }
            set { _purchaseorderid = value; }
        }
        /// <summary>
        /// Title
        /// </summary>		
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        /// <summary>
        /// UserCarID
        /// </summary>		
        private long _usercarid;
        public long UserCarID
        {
            get { return _usercarid; }
            set { _usercarid = value; }
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
        /// Note
        /// </summary>		
        private string _note;
        public string Note
        {
            get { return _note; }
            set { _note = value; }
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
        /// Price
        /// </summary>		
        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }
        /// <summary>
        /// Debt
        /// </summary>		
        private decimal _debt;
        public decimal Debt
        {
            get { return _debt; }
            set { _debt = value; }
        }
        /// <summary>
        /// TotalPrice
        /// </summary>		
        private decimal _totalprice;
        public decimal TotalPrice
        {
            get { return _totalprice; }
            set { _totalprice = value; }
        }
        /// <summary>
        /// CostEvaluation
        /// </summary>		
        private decimal _costevaluation;
        public decimal CostEvaluation
        {
            get { return _costevaluation; }
            set { _costevaluation = value; }
        }
        /// <summary>
        /// RepairTime
        /// </summary>		
        private DateTime _repairtime;
        public DateTime RepairTime
        {
            get { return _repairtime; }
            set { _repairtime = value; }
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
        /// CarNo
        /// </summary>		
        private string _carno;
        public string CarNo
        {
            get { return _carno; }
            set { _carno = value; }
        }
    }
}

