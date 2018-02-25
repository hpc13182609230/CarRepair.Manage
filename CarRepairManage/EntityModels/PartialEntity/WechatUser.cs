namespace EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class WechatUser : BaseEntity
    {
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
        /// Unionid
        /// </summary>		
        private string _unionid;
        public string Unionid
        {
            get { return _unionid; }
            set { _unionid = value; }
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
        /// EncryptKey
        /// </summary>		
        private string _encryptkey;
        public string EncryptKey
        {
            get { return _encryptkey; }
            set { _encryptkey = value; }
        }
        /// <summary>
        /// NickName
        /// </summary>		
        private string _nickname;
        public string NickName
        {
            get { return _nickname; }
            set { _nickname = value; }
        }
        /// <summary>
        /// Sex
        /// </summary>		
        private int _sex;
        public int Sex
        {
            get { return _sex; }
            set { _sex = value; }
        }
        /// <summary>
        /// Language
        /// </summary>		
        private string _language;
        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }
        /// <summary>
        /// City
        /// </summary>		
        private string _city;
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }
        /// <summary>
        /// Province
        /// </summary>		
        private string _province;
        public string Province
        {
            get { return _province; }
            set { _province = value; }
        }
        /// <summary>
        /// Country
        /// </summary>		
        private string _country;
        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }
        /// <summary>
        /// HeadImgUrl
        /// </summary>		
        private string _headimgurl;
        public string HeadImgUrl
        {
            get { return _headimgurl; }
            set { _headimgurl = value; }
        }
        /// <summary>
        /// SubScribeTime
        /// </summary>		
        private DateTime _subscribetime;
        public DateTime SubScribeTime
        {
            get { return _subscribetime; }
            set { _subscribetime = value; }
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
        /// Content
        /// </summary>		
        private string _content;
        public string Content
        {
            get { return _content; }
            set { _content = value; }
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
        /// LoginTokenExpire
        /// </summary>		
        private string _logintokenexpire;
        public string LoginTokenExpire
        {
            get { return _logintokenexpire; }
            set { _logintokenexpire = value; }
        }
        /// <summary>
        /// ShareID
        /// </summary>		
        private string _shareid;
        public string ShareID
        {
            get { return _shareid; }
            set { _shareid = value; }
        }
        /// <summary>
        /// TgID
        /// </summary>		
        private string _tgid;
        public string TgID
        {
            get { return _tgid; }
            set { _tgid = value; }
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
    }
}

