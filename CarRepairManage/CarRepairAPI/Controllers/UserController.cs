using Em.Future._2017.Common;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ViewModels;
using ViewModels.CarRepair;

namespace CarRepairAPI.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        #region points 相关
        [Route("Log")]
        [HttpGet]
        public DataResultModel TestLog()
        {
            DataResultModel result = new DataResultModel();
            try
            {
                DateTime insurance = new DateTime(2017,10,1);
                insurance = insurance.AddDays((insurance.Day - 1) * (-1));

                LogLib.Tracer.RunLog(LogLib.MessageType.WriteInfomation, "", "log", "TestLog"  + "\r\n");
 
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }

        #endregion


        #region points 相关
        [Route("GetPointList")]
        [HttpGet]
        public DataResultModel GetPointList(long userid, int pageIndex = 1, int pageSize = 10)
        {
            DataResultModel result = new DataResultModel();
            PageInfoModel page = new PageInfoModel() { PageIndex = pageIndex, PageSize = pageSize };
            PointsService service = new PointsService();
            try
            {
                result.data = service.GetListByPage(userid, ref page);
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }

        #endregion

    }
}
