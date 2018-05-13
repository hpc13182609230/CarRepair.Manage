using CarRepairAPI.Models;
using Em.Future._2017.Common;
using HelperLib;
using Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using ViewModels;
using ViewModels.CarRepair;
using CarRepairAPI.Filter;
using System.Web;

namespace CarRepairAPI.Controllers
{
    [RoutePrefix("api/Parts")]
    [CustomActionFilter]
    public class PartsController : ApiController
    {
        GarageService _GarageService = new GarageService();
        PartsCompanyService _PartsCompanyService = new PartsCompanyService();
        PartsCallRecordService _PartsCallRecordService = new PartsCallRecordService();
        WXMessageTemplateService _WXMessageTemplateService = new WXMessageTemplateService();

        #region 登录相关的接口

        [Route("Login")]
        [HttpPost]
        public DataResultModel Login(APIRequestModels_Login loginModel)
        {
            DataResultModel result = new DataResultModel();
            StringBuilder data = new StringBuilder();
            DateTime current = DateTime.Now;
            long currentUnix = TransformHelper.Convert_DateTime2Int64(current);

            NameValueCollection request = HttpContext.Current.Request.Form;

            string msg = "";
            #region  执行业务逻辑
            if (string.IsNullOrWhiteSpace(loginModel.LoginToken))//首次登录
            {
                msg = CheckRequestData_Login(loginModel);
                if (!string.IsNullOrWhiteSpace(msg))//验证不通过
                {
                    result.result = 0;
                    result.message = msg;
                }
                else
                {
                    PartsCompanyModel _PartsCompanyModel = _PartsCompanyService.GetByPhone(loginModel.UserName);
                    if (_PartsCompanyModel.ID == 0)
                    {
                        result.result = 0;
                        result.message = "登录账号无效,请联系管理员";
                    }
                    else
                    {
                        WXMessageTemplateModel _WXMessageTemplateModel = _WXMessageTemplateService.GetByPartsCompanyIDAndOptionID(_PartsCompanyModel.ID,12);
                        if (loginModel.Password != _WXMessageTemplateModel.Note)
                        {
                            result.result = 0;
                            result.message = "验证码不正确";
                        }
                        else
                        {
                            if (current> _WXMessageTemplateModel.CreateTime.AddMinutes(5))
                            {
                                result.result = 0;
                                result.message = "验证码已过期,请重新发送";
                            }
                            else
                            {
                                _PartsCompanyModel.LoginToken = _PartsCompanyService.CreateLoginToken(_PartsCompanyModel, loginModel.Password);
                                _PartsCompanyService.Save(_PartsCompanyModel);
                                result.data = _PartsCompanyModel.LoginToken;
                            }
                        }
                    }
                }
            }
            else//已登录过,使用LoginToken自动登录
            {
                PartsCompanyModel _PartsCompanyModel = _PartsCompanyService.GetByLoginToken(loginModel.LoginToken);
                if (_PartsCompanyModel.ID == 0)
                {
                    result.result = -1;
                    result.message = "登录凭证已过期,请重新登录";
                }
                result.data = new { PicURLShow=_PartsCompanyModel.PicURLShow, Name = _PartsCompanyModel.Name, Phone = _PartsCompanyModel.Phone };
            }
            #endregion
            return result;
        }

        /// <summary>
        /// 配件商 登录 获取验证码
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [Route("GetVerificationCode")]
        [HttpPost]
        public DataResultModel GetVerificationCode()
        {
            DataResultModel result = new DataResultModel(); 
            StringBuilder data = new StringBuilder();
            DateTime current = DateTime.Now;
            NameValueCollection request = HttpContext.Current.Request.Form;

            string userName  = request["userName"] ??"";
            #region  执行业务逻辑
            PartsCompanyModel _PartsCompanyModel = _PartsCompanyService.GetByPhone(userName);
            if (_PartsCompanyModel.ID==0)
            {
                result.result = 0;
                result.message = "未能查询用户";
            }
            else
            {
                int code = 0;
                if (!_WXMessageTemplateService.WX_MessageTemplate_PartsClient_GetLoginCode(_PartsCompanyModel,ref code))
                {
                    result.result = 0;
                    result.message = "验证码发送失败";
                }
                else
                {
                    //result.data = code;
                }
            }
            #endregion
            return result;
        }

        #endregion


        #region 电话相关接口
        /// <summary>
        /// 通话开始
        /// </summary>
        /// <returns></returns>
        [Route("PartsCallIn")]
        [HttpPost]
        public DataResultModel PartsCallIn(APIRequestModels_Call requestData)
        {
            DataResultModel result =new DataResultModel();
            string msg = "";
            StringBuilder data = new StringBuilder();
            DateTime current = DateTime.Now;
            long currentUnix = TransformHelper.Convert_DateTime2Int64(current);

            #region  执行业务逻辑
            //验证接口 请求 业务逻辑 参数
            msg = CheckRequestData_Call(requestData);
            if (!string.IsNullOrWhiteSpace(msg))
            {
                result.result = 0;
                result.message = msg;
            }
            else
            {
                PartsCompanyModel _PartsCompanyModel = _PartsCompanyService.GetByLoginToken(requestData.LoginToken);
                if (_PartsCompanyModel.ID==0)
                {
                    result.result = -1;
                    result.message = "LoginToken无效，请重新登录";
                    return result;
                }
                GarageModel _GarageModel = _GarageService.GetAndSaveByNumber(requestData.Number);
                PartsCallRecordModel _PartsCallRecordModel = new PartsCallRecordModel();      
                _PartsCallRecordModel.CallID = requestData.CallID;
                //_PartsCallRecordModel.CallTime = currentUnix;//通话时间
                _PartsCallRecordModel.Openid = _PartsCompanyModel.Contract;
                _PartsCallRecordModel.GarageID = _GarageModel.ID;
                _PartsCallRecordModel.Remark = requestData.Number;
                _PartsCallRecordModel.Phone = _GarageModel.Phone;
                _PartsCallRecordService.Save(_PartsCallRecordModel);

                result.data = _GarageModel;
            }
            #endregion

            return result; 
        }


        /// <summary>
        /// 通话挂断
        /// </summary>
        /// <returns></returns>
        [Route("PartsCallEnd")]
        [HttpPost]
        public DataResultModel PartsCallEnd(APIRequestModels_Call requestData)
        {
            DataResultModel result = new DataResultModel();
            string msg = "";
            StringBuilder data = new StringBuilder();
            DateTime current = DateTime.Now;
            long currentUnix = TransformHelper.Convert_DateTime2Int64(current);

            #region  执行业务逻辑
            //验证接口 请求 业务逻辑 参数
            msg = CheckRequestData_Call(requestData);
            if (!string.IsNullOrWhiteSpace(msg))
            {
                result.result = 0;
                result.message = msg;
            }
            else
            {
                PartsCallRecordModel _PartsCallRecordModel = _PartsCallRecordService.GetByCallID(requestData.CallID);
                _PartsCallRecordModel.CallID = requestData.CallID;
                _PartsCallRecordModel.CallTime = currentUnix - _PartsCallRecordModel.CallTime;
                long id=  _PartsCallRecordService.Save(_PartsCallRecordModel);
            }
            #endregion

            return result;
        }




        #endregion


        #region 

        private string CheckRequestData_Login(APIRequestModels_Login requestData)
        {
            #region 验证  UserName   参数
            if (string.IsNullOrWhiteSpace(requestData.UserName))
            {
                return "UserName not null";
            }
            #endregion

            #region 验证  Password   参数
            if (string.IsNullOrWhiteSpace(requestData.Password))
            {
                
                return "Password not null";
            }

            #endregion
            return "";
        }

        private string CheckRequestData_Call(APIRequestModels_Call requestData)
        {
            #region 验证  CallID   参数
            if (requestData.CallID == 0)
            {
                return "CallID not null";
            }
            #endregion

            #region 验证  PartsSession   参数
            if (string.IsNullOrWhiteSpace(requestData.LoginToken))
            {
                return "PartsSession not null";
            }
            #endregion

            #region 验证  Number   参数
            if (string.IsNullOrWhiteSpace(requestData.Number))
            {
                return "Number not null";
            }
            #endregion

            return "";
        }
        #endregion



    }
}
