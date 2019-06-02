using Em.Future._2017.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CarRepair;
using HelperLib;

namespace Service
{
    public class WechatService
    {

        public static string GetThirdSession(string code,string ShareID)
        {
            WeChatAppDecrypt _WeChatAppDecrypt = new WeChatAppDecrypt();
            OpenIdAndSessionKey _OpenIdAndSessionKey = _WeChatAppDecrypt.DecodeOpenIdAndSessionKey(code);
            string thirdSession = _OpenIdAndSessionKey.session_key;//Guid.NewGuid().ToString();
                                                                   //此处绑定 thirdSession 和 OpenIdAndSessionKey 的关系到数据库

            WechatUserService service = new WechatUserService();
            PointsService _PointsService = new PointsService();
            BaseOptionsService _BaseOptionsService = new BaseOptionsService();
            WechatUserModel _WechatUserModel = service.GetByOpenid(_OpenIdAndSessionKey.openid);
            if (_WechatUserModel == null)
            {
                LogLib.Tracer.RunLog(LogLib.MessageType.WriteInfomation, "", "GetThirdSession",
                    "未能在数据库根据openid获取到用户，说明是新用户" + "\r\n"
                    +"code = " + code + "\r\n"
                    + "ShareID = " + ShareID + "\r\n");
                _WechatUserModel = new WechatUserModel();
                _WechatUserModel.Openid = _OpenIdAndSessionKey.openid;
                _WechatUserModel.SubScribeTime = DateTime.Now;
                _WechatUserModel.LastActiveTime = DateTime.Now;
                _WechatUserModel.ShareID = ShareID;

                _WechatUserModel.LoginToken = thirdSession;

                long id = service.Save(_WechatUserModel);
                if (id > 0 && !string.IsNullOrWhiteSpace(ShareID) && ShareID != "0")
                {
                    try
                    {
                        //获取来源 用户 
                        WechatUserModel sourceWechatUser = service.GetByOpenid(ShareID);
                        long shareOptionID = 7;
                        int todayShareCount = _PointsService.GetTodayShareCount(sourceWechatUser.ID, shareOptionID);
                        LogLib.Tracer.RunLog(LogLib.MessageType.WriteInfomation, "", "GetThirdSession",
                                "今日推荐获得的积分次数" + "\r\n"
                                 + "todayShareCount = " + todayShareCount + "\r\n");
                        if (todayShareCount < 30)
                        {
                            BaseOptionsModel shareOption = _BaseOptionsService.GetByID(shareOptionID);
                            LogLib.Tracer.RunLog(LogLib.MessageType.WriteInfomation, "", "GetThirdSession",
                                  "shareOption = " + TransformHelper.SerializeObject(shareOption) + "\r\n");
                            PointsModel _PointsModel = new PointsModel() { WechatUserID = sourceWechatUser.ID, PointType = Convert.ToInt32(shareOptionID), point = Convert.ToInt32(shareOption.Remark), Note = shareOption.Content, Remark = id.ToString() };
                            long pointid = _PointsService.Save(_PointsModel);
                            LogLib.Tracer.RunLog(LogLib.MessageType.WriteInfomation, "", "GetThirdSession",
                                "pointid = " + pointid + "\r\n");
                        }
                        else
                        {
                            LogLib.Tracer.RunLog(LogLib.MessageType.WriteInfomation, "", "GetThirdSession",
                                "今日推荐获得的积分以及到上限" + "\r\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogLib.Tracer.RunLog(LogLib.MessageType.WriteInfomation, "", "GetThirdSession",
                                "add shareOption  异常" +ex.Message+ "\r\n");
                    }
                }
            }
            else
            {
                _WechatUserModel.LoginToken = thirdSession;
                long id = service.Save(_WechatUserModel);
                //thirdSession = _WechatUserModel.LoginToken;
            }

            return thirdSession;
        }

        public static WechatUserModel GetUserInfo(string encryptedData, string iv, string thirdSession)
        {
            WeChatAppDecrypt _WeChatAppDecrypt = new WeChatAppDecrypt();

            //根据thirdSession 获取 sessionKey0
            string sessionKey = thirdSession;

            WechatUserInfo _WechatUserInfo = _WeChatAppDecrypt.Decrypt(encryptedData, iv, sessionKey);


            WechatUserService service = new WechatUserService();
            WechatUserModel _WechatUserModel = service.GetByOpenid(_WechatUserInfo.openId);
            _WechatUserModel.Unionid = _WechatUserInfo.unionId;
            _WechatUserModel.NickName = _WechatUserInfo.nickName;
            _WechatUserModel.Sex =string.IsNullOrWhiteSpace(_WechatUserInfo.gender)?0:Convert.ToInt32(_WechatUserInfo.gender);
            //_WechatUserModel.Language = _WechatUserInfo.;
            _WechatUserModel.City = _WechatUserInfo.city;
            _WechatUserModel.Country = _WechatUserInfo.country;
            _WechatUserModel.HeadImgUrl = _WechatUserInfo.avatarUrl;
            _WechatUserModel.Remark = encryptedData;
            _WechatUserModel.Content = iv;
            _WechatUserModel.LoginTokenExpire = DateTime.Now.AddMonths(2).ToString("yyyyMMdd HH:mm:ss");
            long id =   service.Save(_WechatUserModel);
            return _WechatUserModel;
        }
    }
}
