namespace EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class UserCars : BaseEntity
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
        /// Attribution
        /// </summary>		
        private string _attribution;
        public string Attribution
        {
            get { return _attribution; }
            set { _attribution = value; }
        }

        /// <summary>
        /// Attribution
        /// </summary>		
        private string _attributionchar;
        public string AttributionChar
        {
            get { return _attributionchar; }
            set { _attributionchar = value; }
        }
        /// <summary>
        /// CarType
        /// </summary>		
        private string _cartype;
        public string CarType
        {
            get { return _cartype; }
            set { _cartype = value; }
        }
        /// <summary>
        /// CarNO
        /// </summary>		
        private string _carno;
        public string CarNO
        {
            get { return _carno; }
            set { _carno = value; }
        }
        /// <summary>
        /// CarOwnerName
        /// </summary>		
        private string _carownername;
        public string CarOwnerName
        {
            get { return _carownername; }
            set { _carownername = value; }
        }
        /// <summary>
        /// CarOwnerTel
        /// </summary>		
        private string _carownertel;
        public string CarOwnerTel
        {
            get { return _carownertel; }
            set { _carownertel = value; }
        }
        /// <summary>
        /// InsuranceTime
        /// </summary>		
        private DateTime _insurancetime;
        public DateTime InsuranceTime
        {
            get { return _insurancetime; }
            set { _insurancetime = value; }
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
        /// Note
        /// </summary>		
        private string _note;
        public string Note
        {
            get { return _note; }
            set { _note = value; }
        }
    }
}

