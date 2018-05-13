using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class PartsCallRecordModel : BaseModel
    {
        /// <summary>
        /// 此次 通话的 id 由前端生成的 id
        /// </summary>		
        private long _callid;
        public long CallID
        {
            get { return _callid; }
            set { _callid = value; }
        }

        /// <summary>
        /// 此次 通话的 时长
        /// </summary>		
        private long _calltime;
        public long CallTime
        {
            get { return _calltime; }
            set { _calltime = value; }
        }

        /// <summary>
        /// 配件商的 openid
        /// </summary>		
        private string _openid;
        public string Openid
        {
            get { return _openid; }
            set { _openid = value; }
        }

        /// <summary>
        /// Garage 的id
        /// </summary>		
        private long _garageid;
        public long GarageID
        {
            get { return _garageid; }
            set { _garageid = value; }
        }

        /// <summary>
        /// 手机号 用户的 唯一标识
        /// </summary>
        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
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


        public string PartsName { get; set; }
    }
}
