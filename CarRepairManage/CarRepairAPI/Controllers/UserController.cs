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
        /// <returns></returns>0
        [Route("GetBindPartsCompanys")]
        [HttpGet]
        public DataResultModel GetBindPartsCompanys(long userid,string codeID="370000", int pageIndex = 1, int pageSize = 10)
        {
            LogLib.Tracer.RunLog(LogLib.MessageType.WriteInfomation, "", "GetBindPartsCompanys", "codeID = " + codeID + "\r\n" );
            DataResultModel result = new DataResultModel();
            PageInfoModel page = new PageInfoModel() { PageIndex = pageIndex, PageSize = pageSize };
            try
            {
                List<PartsCompanyBindWechatUserModel> _PartsCompanyBindWechatUserList= _PartsCompanyBindWechatUserService.GetListByPage(userid, ref page);
                List<PartsCompanyModel> datas= _PartsCompanyService.GetByIDs(_PartsCompanyBindWechatUserList.Select(p => p.PartsCompanyID).ToList());
                foreach (var item in datas)
                {
                    item.Content = HtmlHelper.HTML_RemoveTag(item.Content);
                    item.UpdateTime = _PartsCompanyBindWechatUserList.Where(p => p.PartsCompanyID == item.ID).FirstOrDefault().UpdateTime;
                }
                datas = datas.OrderByDescending(p => p.UpdateTime).ToList();
                //如果用户 绑定的 配件商 较少，则 分区 取 推荐的 配件商
                if (datas.Count < 5)
                {
                    #region 显示 默认 逻辑  的配件商
                    //获取一个月内  的绑定 关系
                    List<PartsCompanyBindWechatUserModel> defalutBinds = _PartsCompanyBindWechatUserService.GetListByDate(DateTime.Now.Date.AddMonths(-1));
                    List<PartsCompanyModel> partsCompanys = _PartsCompanyService.GetListByCodeID(codeID);
                    List<long> partsCompanyIDs = partsCompanys.Select(p => p.ID).ToList();
                    var ls = defalutBinds.GroupBy(a => a.PartsCompanyID).Select(g => (new { PartsCompanyID = g.Key, Ccount = g.Count() }));
                    ls = ls.Where(p => partsCompanyIDs.Contains(p.PartsCompanyID)).OrderByDescending(p => p.Ccount).ToList();
                    List<PartsCompanyModel> defalutdatas = _PartsCompanyService.GetByIDs(ls.Select(p => p.PartsCompanyID).Take(5).ToList());
                    foreach (var item in defalutdatas)
                    {
                        item.Content = HtmlHelper.HTML_RemoveTag(item.Content);
                    }
                    datas.AddRange(defalutdatas);
                    #endregion 
                }

                result.data = datas;
            }
            catch (Exception ex)
            {
                LogLib.Tracer.RunLog(LogLib.MessageType.WriteInfomation, "", "GetBindPartsCompanys", "ex = " + ex.Message + "\r\n");
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }

        #endregion

       

    }
}
