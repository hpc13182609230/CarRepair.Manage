using Em.Future._2017.Common;
using HelperLib;
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
        PointsService _PointsService = new PointsService();
        PartsCompanyBindWechatUserService _PartsCompanyBindWechatUserService = new PartsCompanyBindWechatUserService();
        WXMessageTemplateService _WXMessageTemplateService = new WXMessageTemplateService();
        PartsCompanyService _PartsCompanyService = new PartsCompanyService();

        #region 测试
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
           
            try
            {
                result.data = _PointsService.GetListByPage(userid, ref page);
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }
        #endregion

        #region 获取用户 绑定的 商户
      

        /// <summary>
        /// 小程序首页展示 已经绑定过的 配件商的列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Route("GetBindPartsCompanys")]
        [HttpGet]
        public DataResultModel GetBindPartsCompanys(long userid, int pageIndex = 1, int pageSize = 10)
        {
            DataResultModel result = new DataResultModel();
            PageInfoModel page = new PageInfoModel() { PageIndex = pageIndex, PageSize = pageSize };
            try
            {
                List<PartsCompanyBindWechatUserModel> _PartsCompanyBindWechatUserList= _PartsCompanyBindWechatUserService.GetListByPage(userid, ref page);
                var datas= _PartsCompanyService.GetByIDs(_PartsCompanyBindWechatUserList.Select(p => p.PartsCompanyID).ToList());


                foreach (var item in datas)
                {
                    item.Content = HtmlHelper.HTML_RemoveTag(item.Content);
                    item.UpdateTime = _PartsCompanyBindWechatUserList.Where(p => p.PartsCompanyID == item.ID).FirstOrDefault().UpdateTime;

                }
                datas = datas.OrderByDescending(p => p.UpdateTime).ToList();
                result.data = datas;
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
