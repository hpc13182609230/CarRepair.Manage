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
    [RoutePrefix("api/UserCars")]
    public class UserCarsController : ApiController
    {
        // GET: UserCars

        /// <summary>
        /// 保存 用户新增的 车信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("SaveUserCar")]
        [HttpPost]
        public DataResultModel SaveUserCar(UserCarsModel model)
        {
            DataResultModel result = new DataResultModel();
            UserCarsService service = new UserCarsService();
            try
            {
                result.data = service.Save(model);
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }

        [Route("GetCarList")]
        [HttpGet]
        public DataResultModel GetCarList(long id,string keyword  ,int pageIndex = 1, int pageSize = 10)
        {
            DataResultModel result = new DataResultModel();
            PageInfoModel page = new PageInfoModel() { PageIndex=pageIndex,PageSize=pageSize};
            UserCarsService service = new UserCarsService();
            try
            {
                result.data = service.GetListByPage(id, keyword, ref page);
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }

        [Route("DeleteCarByID")]
        [HttpPost]
        public DataResultModel DeleteCarByID(long id)
        {
            DataResultModel result = new DataResultModel();
            UserCarsService service = new UserCarsService();
            try
            {
                result.data = service.DeleteByID(id);
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }


        [Route("GetCarByID")]
        [HttpGet]
        public DataResultModel GetCarByID(long id)
        {
            DataResultModel result = new DataResultModel();
            UserCarsService service = new UserCarsService();
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