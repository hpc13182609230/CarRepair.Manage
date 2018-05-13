using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class WXMessageTemplateModel : BaseModel
    {
        /// <summary>
        /// WechatUserID  发送方 小程序的  WechatUser 的主键 id
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
        /// Touser 接收方下 微信服务号 openid 
        /// </summary>		
        private string _touser;
        public string Touser
        {
            get { return _touser; }
            set { _touser = value; }
        }
        /// <summary>
        /// Template_id
        /// </summary>		
        private string _template_id;
        public string Template_id
        {
            get { return _template_id; }
            set { _template_id = value; }
        }
        /// <summary>
        /// Data
        /// </summary>		
        private string _data;
        public string Data
        {
            get { return _data; }
            set { _data = value; }
        }
        /// <summary>
        /// BaseOptionsID
        /// </summary>		
        private long _baseoptionsid;
        public long BaseOptionsID
        {
            get { return _baseoptionsid; }
            set { _baseoptionsid = value; }
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


        /// <summary>
        /// 发送方 小程序的 用户 昵称  （根据 WechatUser 的主键 id ） 
        /// </summary>
        public string  WechatUserNiceNanme { get; set; }

        /// <summary>
        /// 配件商 名称
        /// </summary>
        public string CompanyNanme { get; set; }

        /// <summary>
        /// 配件商  的 微信 昵称
        /// </summary>
        public string CompanyWXNiceNanme { get; set; }

        /// <summary>
        /// 推送类型
        /// </summary>
        public string PushType { get; set; }

    }
}