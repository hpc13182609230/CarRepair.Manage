using Em.Future._2017.Common;
using HelperLib;
using Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ViewModels;
using ViewModels.CarRepair;

namespace CarRepairAPI.Controllers
{
    [RoutePrefix("api/Media")]
    public class MediaController : ApiController 
    {
        #region 基本类 实例化
        ShortVideoService _ShortVideoService = new ShortVideoService();

        #endregion
       
        #region 短视频
        [Route("GetShortVideoList")]
        [HttpGet]
        public DataResultModel GetShortVideoList(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            keyword = keyword ?? "";
            PageInfoModel page = new PageInfoModel() { PageIndex = pageIndex, PageSize = pageSize };
            DateTime start = new DateTime(2017, 1, 1, 0, 00, 00, 001);
            DataResultModel result = new DataResultModel();
            try
            {
                result.data = _ShortVideoService.GetListByPage(keyword,start,DateTime.Now,ref page);
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
