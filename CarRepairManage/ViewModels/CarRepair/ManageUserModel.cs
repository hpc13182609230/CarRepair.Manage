using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class ManageUserModel:BaseModel
    {
        /// <summary>
        /// LoginName
        /// </summary>		
        private string _loginname;
        public string LoginName
        {
            get { return _loginname; }
            set { _loginname = value; }
        }
        /// <summary>
        /// Openid
        /// </summary>		
        private string _openid;
        public string Openid
        {
            get { return _openid; }
            set { _openid = value; }
        }
        /// <summary>
        /// AreaCodeID
        /// </summary>		
        private string _areacodeid;
        public string AreaCodeID
        {
            get { return _areacodeid; }
            set { _areacodeid = value; }
        }
        /// <summary>
        /// Permission
        /// </summary>		
        private string _permission;
        public string Permission
        {
            get { return _permission??""; }
            set { _permission = value; }
        }
        /// <summary>
        /// Password
        /// </summary>		
        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        /// <summary>
        /// UserName
        /// </summary>		
        private string _username;
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }
        /// <summary>
        /// Url
        /// </summary>		
        private string _url;
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
        /// <summary>
        /// Info
        /// </summary>		
        private string _info;
        public string Info
        {
            get { return _info; }
            set { _info = value; }
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
        /// Avatar
        /// </summary>		
        private string _avatar;
        public string Avatar
        {
            get { return _avatar; }
            set { _avatar = value; }
        }
        /// <summary>
        /// EncryptKey
        /// </summary>		
        private string _encryptkey;
        public string EncryptKey
        {
            get { return _encryptkey; }
            set { _encryptkey = value; }
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
        /// LastActiveTime
        /// </summary>		
        private DateTime _lastactivetime;
        public DateTime LastActiveTime
        {
            get { return _lastactivetime; }
            set { _lastactivetime = value; }
        }


        /// <summary>
        /// Province
        /// </summary>		
        private string _province;
        public string Province
        {
            get
            {
                if (string.IsNullOrWhiteSpace(AreaCodeID)|| AreaCodeID=="0")
                {
                    return "全国";
                } 
                else
	            {
                    return _province;
                }
            }
            set { _province = value; }

        }


    }
}
