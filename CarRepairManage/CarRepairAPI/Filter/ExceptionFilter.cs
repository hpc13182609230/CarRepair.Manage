using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using ViewModels;

namespace CarRepairAPI
{
    public class ExceptionFilter: ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //记录错误日志
            Tracer.RunLog(MessageType.Error, "", MessageType.Error.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name + "ex = ：" + actionExecutedContext.Exception.Message + "\r\n");
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK, new DataResultModel { result = 0, message = "服务器被外星人拐跑了！" });

        }
    }
}