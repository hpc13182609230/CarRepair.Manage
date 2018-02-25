namespace EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class Points : BaseEntity
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
        /// PointType
        /// </summary>		
        private long _pointtype;
        public long PointType
        {
            get { return _pointtype; }
            set { _pointtype = value; }
        }
        /// <summary>
        /// point
        /// </summary>		
        private int _point;
        public int point
        {
            get { return _point; }
            set { _point = value; }
        }
        /// <summary>
        /// PointSum
        /// </summary>		
        private int _pointsum;
        public int PointSum
        {
            get { return _pointsum; }
            set { _pointsum = value; }
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


    }
}

