namespace EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class WXUser : BaseEntity
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
        /// Headimgurl
        /// </summary>		
        private string _headimgurl;
        public string Headimgurl
        {
            get { return _headimgurl; }
            set { _headimgurl = value; }
        }
        /// <summary>
        /// Subscribe_time
        /// </summary>		
        private DateTime _subscribe_time;
        public DateTime Subscribe_time
        {
            get { return _subscribe_time; }
            set { _subscribe_time = value; }
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
        /// Token
        /// </summary>		
        private string _token;
        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }
        /// <summary>
        /// TokenExpire
        /// </summary>		
        private DateTime _tokenexpire;
        public DateTime TokenExpire
        {
            get { return _tokenexpire; }
            set { _tokenexpire = value; }
        }
        /// <summary>
        /// Groupid
        /// </summary>		
        private int _groupid;
        public int Groupid
        {
            get { return _groupid; }
            set { _groupid = value; }
        }
        /// <summary>
        /// Tagid_list
        /// </summary>		
        private string _tagid_list;
        public string Tagid_list
        {
            get { return _tagid_list; }
            set { _tagid_list = value; }
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
        private int _tgid;
        public int TgID
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

