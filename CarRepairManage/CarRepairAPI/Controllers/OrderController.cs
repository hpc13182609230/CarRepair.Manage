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
    public class OrderController : ApiController
    {
        #region 采购订单  PurchaseOrder
        /// <summary>
        /// 保存 采购订单
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("SavePurchaseOrder")]
        [HttpPost]
        public DataResultModel SavePurchaseOrder(PurchaseOrderModel model)
        {
            DataResultModel result = new DataResultModel();
            PurchaseOrderService service = new PurchaseOrderService();
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

        /// <summary>
        /// 更改 采购订单  状态
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("ChangePurchaseOrderStatu")]
        [HttpPost]
        public DataResultModel ChangePurchaseOrderStatu (long  orderid ,int statu)
        {
            DataResultModel result = new DataResultModel();
            PurchaseOrderService service = new PurchaseOrderService();
            try
            {
                result.data = service.UpdateStatu(orderid,statu);
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }


        /// <summary>
        /// 获取 采购订单 列表
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("SavePurchaseOrder")]
        [HttpGet]
        public DataResultModel GetPurchaseOrderList(long id, string keyword, int start = 0, int offset = 10)
        {
            DataResultModel result = new DataResultModel();
            PageInfoModel page = new PageInfoModel() { Start = start, Offset = offset };
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

        #endregion


        #region 维修订单  RepairOrder
        /// <summary>
        /// 保存 用户新增的 车信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("SaveUserCar")]
        [HttpPost]
        public DataResultModel SaveRepairOrder(UserCarsModel model)
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
        #endregion

    }
}
