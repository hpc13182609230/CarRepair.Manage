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
    [RoutePrefix("api/Classify")]
    public class ClassifyController : ApiController
    {
        //根据一级分类 获取分类列表
        [Route("GetPartsClassifyList")]
        [HttpGet]
        public DataResultModel GetPartsClassifyList(long OptionID)
        {
            DataResultModel result = new DataResultModel();
            PartsClassifyService service = new PartsClassifyService();
            try
            {
                result.data = service.GetAllByParentID(OptionID);
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }

        //获取子分类 下面的配件商 列表
        [Route("GetPartsClassifyCompanyList")]
        [HttpGet]
        public DataResultModel GetPartsClassifyCompanyList(long partsClassifyID)
        {
            DataResultModel result = new DataResultModel();
            //PageInfoModel page = new PageInfoModel() { Start = start, Offset = offset };
            PartsClassifyCompanyService service = new PartsClassifyCompanyService();
            try
            {
                result.data = service.GetForAPI(partsClassifyID);
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
                result.data = service.GetByID(id);
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
