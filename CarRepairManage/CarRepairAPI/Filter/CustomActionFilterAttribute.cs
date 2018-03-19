using HelperLib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ViewModels;

namespace CarRepairAPI.Filter
{
    public class CustomActionFilterAttribute : ActionFilterAttribute
    {

        private static string ApiKey = "ApiKey";
        private static string ApiSecret = "ApiSecret";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            NameValueCollection request = HttpContext.Current.Request.Form;
            string msg = CheckRequest(request);
            if (!string.IsNullOrWhiteSpace(msg))//验证不通过
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, new DataResultModel {result=0,message= msg });
            }
        }

        #region 接口调用的 基本 参数 验证的 相关逻辑
        private string CheckRequest(NameValueCollection request)
        {
            #region 验证  ApiKey   参数
            if (string.IsNullOrWhiteSpace(request["ApiKey"]))
            {
                return "ApiKey not null";
            }
            if (!request["ApiKey"].Equals(ApiKey))
            {
                return "ApiKey is invalid ";
            }
            #endregion

            #region 验证  ApiSecret   参数
            if (string.IsNullOrWhiteSpace(request["ApiSecret"]))
            {
                return "ApiSecret not null";
            }
            if (!request["ApiSecret"].Equals(ApiSecret))
            {
                return "ApiSecret is invalid ";
            }
            #endregion

            #region 验证date参数
            //if (string.IsNullOrWhiteSpace(request["Date"]))
            //{
            //    return "Date not null";
            //}
            //DateTime date = DateTime.MinValue;
            //date = TransformHelper.Convert_TimeStamp2DateTime(request["Date"]);
            //TimeSpan ts = DateTime.Now.Subtract(date);
            //if (ts.TotalSeconds > 600 || ts.TotalSeconds < -600)
            //{
            //    return "Date 不能超过 10分钟 ";
            //}
            #endregion

            #region 验证签名

            if (string.IsNullOrWhiteSpace(request["Sign"]))
            {
                return "Sign not null";
            }
            bool IsSignaturePass = EncryptHelper.ECOSmd5IsSuccess(request, request["Sign"].ToString());
            //if (!IsSignaturePass)
            //{
            //    return "Sign 不合法";
            //}
            #endregion

            return "";
        }
        #endregion

    }
}