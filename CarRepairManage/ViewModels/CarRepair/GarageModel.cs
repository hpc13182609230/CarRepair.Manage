using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class GarageModel : BaseModel
    {
        /// <summary>
        /// WechatUser  的  openid ID
        /// </summary>		
        private string _openid;
        public string Openid
        {
            get { return _openid; }
            set { _openid = value; }
        }
        /// <summary>
        /// 公司名称
        /// </summary>		
        private string _companyname;
        public string CompanyName
        {
            get { return _companyname; }
            set { _companyname = value; }
        }
        /// <summary>
        /// 老板姓名
        /// </summary>		
        private string _bossname;
        public string BossName
        {
            get { return _bossname; }
            set { _bossname = value; }
        }

        /// <summary>
        /// 手机号 支持多个 , 连接
        /// </summary>
        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        /// <summary>
        /// 固话
        /// </summary>
        private string _mobile;
        public string Mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }

        /// <summary>
        /// 是否是我们的用户
        /// </summary>
        private int _ischeck;
        public int IsCheck
        {
            get { return _ischeck; }
            set { _ischeck = value; }
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
        /// Remark
        /// </summary>		
        private string _pictureurl;
        public string PictureURL
        {
            get { return "https://xcx.xiuchebibei.com/piclib/default.png"; }
            set { _pictureurl = value; }
        }

    }
}
