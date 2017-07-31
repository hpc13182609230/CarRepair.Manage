using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ViewModel;

namespace CarRepairAPI.Controllers
{
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {
        [Route("Wechat_Login")]
        [HttpGet]
        public DataResultModel Wechat_Login (string thirdSession ="",string code="")
        {
            DataResultModel result = new DataResultModel();
            try
            {

            }
            catch (Exception ex)
            {
                result.rs = 0;
                result.em = ex.Message;
            }
            return result;
        }

    }
}