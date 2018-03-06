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
    [RoutePrefix("api/Area")]
    public class AreaController : ApiController
    {
        AreaService service = new AreaService();

        // GET: Area
        [Route("GetProvinces")]
        [HttpGet]
        public DataResultModel GetProvinces(string name)
        {
            DataResultModel result = new DataResultModel();
            try
            {
                List<AreaModel> provinceList = service.GetListByParentID("0");
                foreach (var item in provinceList)
                {
                    if (item.name == "广西壮族自治区")
                    {
                        item.name = "广西省";
                    }
                    else
                    {
                        item.name = item.name.Substring(0, 3);
                    }
                }
                if (name== "广西壮族自治区")
                {
                    name = "广西省";
                }
                var  province  = provinceList.Where(p => p.name == name).FirstOrDefault();
                result.data = new { provinceList=provinceList, province = province };
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }

        // GET: Area
        [Route("MatchProvinceName")]
        [HttpGet]
        public DataResultModel MatchProvinceName(string name)
        {
            DataResultModel result = new DataResultModel();
            try
            {
                AreaModel area = service.MatchProvinceName(name);
                area.name = area.name.Substring(0,2);
                result.data = area;
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