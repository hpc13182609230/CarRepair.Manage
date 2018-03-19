using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModels;
using ViewModels.CarRepair;
using AutoMapperLib;
using Repository;
using ViewModels;
using WechatAppLib;
using HelperLib;
using LogLib;

namespace Service
{
    public class WXMessageTemplateService
    {
        WXMessageTemplateRepository repository = new WXMessageTemplateRepository();

        WechatUserService _WechatUserService = new WechatUserService();
        PartsCompanyService _PartsCompanyService = new PartsCompanyService();
        PartsCompanyBindWechatUserService _PartsCompanyBindWechatUserService = new PartsCompanyBindWechatUserService();

        #region CURD
        public WXMessageTemplateModel GetByID(long id)
        {
            WXMessageTemplateModel model = new WXMessageTemplateModel();
           
            var res = repository.GetEntityByID(id);
            if (res != null)
            {
                model = AutoMapperClient.MapTo<WXMessageTemplate, WXMessageTemplateModel>(res);
            }
            return model;
        }

        public int DeleteByID(long id)
        {
            var res = repository.GetEntityByID(id);
            var flag = repository.Delete(res);
            return flag;
        }

        public long Save(WXMessageTemplateModel model)
        {
            WXMessageTemplate entity = new WXMessageTemplate();
            entity = AutoMapperClient.MapTo<WXMessageTemplateModel, WXMessageTemplate>(model);
            long id = 0;
            if (model.ID == 0)
            {
                id = repository.Insert(entity);
            }
            else
            {
                id = repository.Update(entity);
            }
            return id;
        }

        public List<WXMessageTemplateModel> GetListByPage(string keyword, DateTime startTime, DateTime endTime, ref PageInfoModel page)
        {
            int total = 0;
            keyword = keyword ?? "";
            List<WXMessageTemplateModel> models = new List<WXMessageTemplateModel>();
            var entities = repository.GetEntitiesForPaging(ref total, page.PageIndex, page.PageSize, p => p.Touser.Contains(keyword) && p.CreateTime >= startTime && p.CreateTime <= endTime, p => p.ID);
            page.TotalCount = total;
            foreach (var item in entities)
            {
                WXMessageTemplateModel model = AutoMapperClient.MapTo<WXMessageTemplate, WXMessageTemplateModel>(item);
                models.Add(model);
            }
            return models;
        }
        #endregion


        #region Wechat message template business logic

        public bool WX_Message_Template_Send(string touser, string template_id, string data)
        {
            string msgid = WeChatServiceHelper.WX_Message_Template_Send(touser, template_id, data);
            return string.IsNullOrWhiteSpace(msgid) ? false : true;
        }

        /// <summary>
        /// 服务号 电话 推送
        /// </summary>
        /// <param name="wechatID">小程序 用户 id</param>
        /// <param name="partsCompanyID">配件商 id</param>
        /// <param name="touser">服务号 用户 openid</param>
        /// <param name="template_id">推送的模版消息id</param>
        /// <returns></returns>
        public bool WX_MessageTemplate_Tel_Push(long wechatID, long partsCompanyID, string template_id = "CwhLNFzWeJ7qmtxOH_Jz2QW1kHA_Yq_q7U1ME26vxrk")
        {
            try
            {
                WechatUserModel wechatUser = _WechatUserService.GetByID(wechatID);
                PartsCompanyModel _PartsCompanyModel = _PartsCompanyService.GetByID(partsCompanyID);
                if (string.IsNullOrWhiteSpace(_PartsCompanyModel.Contract))
                {
                    Tracer.RunLog(MessageType.WriteInfomation, "", MessageType.Error.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name + " 推送失败 partsCompanyID= ：" + partsCompanyID +"未关注微信服务号"+ "\r\n");
                    return false;
                }
                else
                {
                    object dataObj = new
                    {
                        first = new { value = "尊敬的修车必备认证商户，您即将接到来自平台修理厂用户的电话！", color = "#FF0000" },
                        keyword1 = new { value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), color = "#173177" },
                        keyword2 = new { value = "【" + wechatUser.NickName + "】" + "正在拨打您的电话,，请保持电话畅通，并提供专业、高效、优质的服务，祝生意兴隆！【" + _PartsCompanyModel.Name + "】", color = "#173177" },
                        remark = new { value = "此功能仅限平台来电，不包含广告书和其他渠道！", color = "#173177" }
                    };
                    string data = TransformHelper.SerializeObject(dataObj);
                    string msgid = WeChatServiceHelper.WX_Message_Template_Send(_PartsCompanyModel.Contract, template_id, data);
                    if (!string.IsNullOrWhiteSpace(msgid))
                    {
                        WXMessageTemplateModel model = new WXMessageTemplateModel();
                        model.WechatUserID = wechatID;
                        model.PartsCompanyID = partsCompanyID;
                        model.Touser = _PartsCompanyModel.Contract;
                        model.Template_id = template_id;
                        model.BaseOptionsID = 9;
                        model.Data = data;
                        model.Remark = "msgid=" + msgid;
                        long id = Save(model);
                    }
                    else
                    {
                        Tracer.RunLog(MessageType.WriteInfomation, "", MessageType.Error.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name + " 推送失败 = ：" + wechatID + "\r\n");
                    }
                    return string.IsNullOrWhiteSpace(msgid) ? false : true;
                }
            }
            catch (Exception ex)
            {
                Tracer.RunLog(MessageType.WriteInfomation, "", MessageType.Fatal.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name+ " ex = ：" + ex.Message + "\r\n");
                return false;
            }
           
        }

        /// <summary>
        /// 服务号  绑定会员推送
        /// </summary>
        /// <param name="WechatUserID">小程序 用户 id</param>
        /// <param name="touser">服务号 用户 openid</param>
        /// <param name="template_id">推送的模版消息id</param>
        /// <returns></returns>
        public bool WX_MessageTemplate_BindMember_Push(long WechatUserID, long PartsCompanyID, string template_id = "GCFutkEI-SDZWYu7gNNNX1bGHw93EW_yjMr7iCEaJCs")
        {
            try
            {
                WechatUserModel wechatUser = _WechatUserService.GetByID(WechatUserID);
                PartsCompanyModel _PartsCompanyModel = _PartsCompanyService.GetByID(PartsCompanyID);
                #region 添加 修理厂 和 配件商之间的绑定关系
                PartsCompanyBindWechatUserModel _PartsCompanyBindWechatUserModel = new PartsCompanyBindWechatUserModel();
                _PartsCompanyBindWechatUserModel.WechatUserID = WechatUserID;
                _PartsCompanyBindWechatUserModel.PartsCompanyID = PartsCompanyID;
                _PartsCompanyBindWechatUserService.Save(_PartsCompanyBindWechatUserModel);
                #endregion
                if (string.IsNullOrWhiteSpace(_PartsCompanyModel.Contract))
                {
                    Tracer.RunLog(MessageType.WriteInfomation, "", MessageType.Warning.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name + " 推送失败 partsCompanyID= ：" + PartsCompanyID + "未关注微信服务号" + "\r\n");
                }
                else
                {
                    object dataObj = new
                    {
                        first = new { value = "将您的页面推荐给修理厂，系统会自动绑定您与修理厂用户之间的业务关系，从而优先显示您的信息！", color = "#1C86EE" },
                        keyword1 = new { value = wechatUser.NickName, color = "#173177" },
                        keyword2 = new { value = "绑定成功！", color = "#173177" },
                        remark = new { value = "修车必备独有的[金矿系统]能源源不断的帮您开发新客户，稳固老客户！→详情请点击←", color = "#173177" }
                    };
                    string data = TransformHelper.SerializeObject(dataObj);
                    string msgid = WeChatServiceHelper.WX_Message_Template_Send(_PartsCompanyModel.Contract, template_id, data);
                    if (!string.IsNullOrWhiteSpace(msgid))
                    {
                        WXMessageTemplateModel model = new WXMessageTemplateModel();
                        model.WechatUserID = WechatUserID;
                        model.PartsCompanyID = PartsCompanyID;
                        model.Touser = _PartsCompanyModel.Contract;
                        model.Template_id = template_id;
                        model.BaseOptionsID = 10;
                        model.Data = data;
                        model.Remark = "msgid=" + msgid;
                        long id = Save(model);
                    }
                    else
                    {
                        Tracer.RunLog(MessageType.WriteInfomation, "", MessageType.Error.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name + " 推送失败 = ：" + _PartsCompanyModel.Contract + "\r\n");
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Tracer.RunLog(MessageType.WriteInfomation, "", MessageType.Fatal.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name + " ex = ：" + ex.Message + "\r\n");
                return false;
            }
            
        }

        /// <summary>
        /// 配件商 登录 获取验证码
        /// </summary>
        /// <param name="WechatUserID">小程序 用户 id</param>
        /// <param name="template_id">推送的模版消息id</param>
        /// <returns></returns>
        public bool WX_MessageTemplate_PartsClient_GetLoginCode(PartsCompanyModel _PartsCompanyModel, ref int code, string template_id = "AcVAnRbbuKfMIbobAAMfqy7Oz-hL5o_iYbonaXSTbd4")
        {
            DateTime current = DateTime.Now;
            try
            {
                code = new Random().Next(100000,1000000);
                object dataObj = new
                {
                    first = new { value = "【修车必备】提醒您收到一条来电宝的单点登录验证码，5分钟内有效，请勿泄露！", color = "#1C86EE" },
                    keyword1 = new { value = code, color = "#173177" },
                    keyword2 = new { value = current.ToString("yyyy-MM-dd HH:mm:ss"), color = "#173177" },
                    remark = new { value = "快速识别修理厂来电信息，降低不良采购的风险！提升业务成交效率！【修车必备@青岛火力科技】", color = "#173177" }
                };
                string data = TransformHelper.SerializeObject(dataObj);
                string msgid = WeChatServiceHelper.WX_Message_Template_Send(_PartsCompanyModel.Contract, template_id, data);
                if (!string.IsNullOrWhiteSpace(msgid))
                {
                    WXMessageTemplateModel model = new WXMessageTemplateModel();
                    //model.WechatUserID = WechatUserID;
                    model.PartsCompanyID = _PartsCompanyModel.ID;
                    model.Touser = _PartsCompanyModel.Contract;
                    model.Template_id = template_id;
                    model.BaseOptionsID = 12;
                    model.Data = data;
                    model.Remark = "msgid =" + msgid;
                    model.Note = code.ToString() ;
                    long id = Save(model);
                }
                else
                {
                    Tracer.RunLog(MessageType.WriteInfomation, "", MessageType.Error.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name + " 推送失败 = ：" + _PartsCompanyModel.Contract + "\r\n");
                }
                return true;
            }
            catch (Exception ex)
            {
                Tracer.RunLog(MessageType.WriteInfomation, "", MessageType.Fatal.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name + " ex = ：" + ex.Message + "\r\n");
                return false;
            }

        }

        #endregion


        public WXMessageTemplateModel GetByPartsCompanyIDAndOptionID(long PartsCompanyID,long BaseOptionsID)
        {
            WXMessageTemplateModel model = new WXMessageTemplateModel();

            var res = repository.GetEntityOrder(p=>p.PartsCompanyID==PartsCompanyID&&p.BaseOptionsID== BaseOptionsID,p=>p.ID);
            if (res != null)
            {
                model = AutoMapperClient.MapTo<WXMessageTemplate, WXMessageTemplateModel>(res);
            }
            return model;
        }

    }
}
