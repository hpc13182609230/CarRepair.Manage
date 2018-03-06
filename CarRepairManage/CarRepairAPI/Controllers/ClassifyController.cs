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
    [RoutePrefix("api/Classify")]
    public class ClassifyController : ApiController
    {
        //根据一级分类 获取分类列表
        [Route("GetPartsClassifyList")]
        [HttpGet]
        public DataResultModel GetPartsClassifyList(long OptionID,string keyword)
        {
            DataResultModel result = new DataResultModel();
            PartsClassifyService service = new PartsClassifyService();
            try
            {
                List<PartsClassifyModel> models = service.SearchAllByParentIDThenOrder(OptionID,keyword);
                result.data = models;
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }

        //获取子分类 下面的配件商 
        [Route("GetPartsClassifyCompanyList")]
        [HttpGet]
        public DataResultModel GetPartsClassifyCompanyList(long partsClassifyID, string keyword,string codeID="370000")
        {
            keyword = keyword ?? "";
            PageInfoModel page = new PageInfoModel() { PageIndex=1,PageSize=100};
            DateTime start= new DateTime(2017, 1, 1, 0, 00, 00, 001); //正确
            DataResultModel result = new DataResultModel();
            PartsCompanyService service = new PartsCompanyService();
            try
            {
                List<PartsCompanyModel> data = service.GetListByPage(keyword, partsClassifyID, start, DateTime.Now, codeID,ref page);
                foreach (var item in data)
                {
                    item.Content = HtmlHelper.HTML_RemoveTag(item.Content);
                }
                result.data = data;
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }

        //获取配件商 的详情
        [Route("GetPartsCompany")]
        [HttpGet]
        public DataResultModel GetPartsCompany(long id)
        {
            DataResultModel result = new DataResultModel();
            PartsCompanyService service = new PartsCompanyService();
            try
            {
                PartsCompanyModel model = service.GetByID(id);
                result.data = model;
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }


        /// <summary>
        /// 配件商 搜索
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="codeID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Route("SearchPartsCompany")]
        [HttpGet]
        public DataResultModel SearchPartsCompany(string keyword, string codeID = "370000", int pageIndex = 1, int pageSize = 10)
        {
            DataResultModel result = new DataResultModel();
            PartsCompanyService service = new PartsCompanyService();
            PageInfoModel page = new PageInfoModel() { PageIndex = pageIndex, PageSize = pageSize };
            try
            {
                List<PartsCompanyModel> models = service.GetListByPage(keyword, codeID, new DateTime(2017,1,1), DateTime.Now, ref page);
                foreach (var item in models)
                {
                    item.Content = HtmlHelper.HTML_RemoveTag(item.Content);
                }
                result.data = models;
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }



    }
}
