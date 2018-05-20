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
    [RoutePrefix("api/Order")]
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
                model.Remark = string.IsNullOrWhiteSpace(model.Remark) ?model.OrderTime.ToString("yyyyMMdd")+model.Name:model.Remark;
                model.Statu = string.IsNullOrWhiteSpace(model.PicURL) ?0:1;
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
                var flag= service.UpdateStatu(orderid, statu);
                if (flag>0)
                {
                    result.data = flag;
                }
                else if (flag==-1)
                {
                    result.result = -1;
                    result.message = "为上传图片的订单的无法进行确认!";
                }
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }



        [Route("GetPurchaseOrderByID")]
        [HttpGet]
        public DataResultModel GetPurchaseOrderByID(long id)
        {
            DataResultModel result = new DataResultModel();
            PurchaseOrderService service = new PurchaseOrderService();
            try
            {
                PurchaseOrderModel model = service.GetByID(id);
                if (model.ID == 0)
                {
                    model.OrderTime = DateTime.Now;
                }
                result.data = model;
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }



        [Route("DelPurchaseOrderByID")]
        [HttpGet]
        public DataResultModel DelPurchaseOrderByID(long id)
        {
            DataResultModel result = new DataResultModel();
            PurchaseOrderService service = new PurchaseOrderService();
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

        /// <summary>
        /// 获取 采购订单 列表
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("GetPurchaseOrderList")]
        [HttpGet]
        public DataResultModel GetPurchaseOrderList(long userid, int pageIndex = 1, int pageSize = 10)
        {
            DataResultModel result = new DataResultModel();
            PageInfoModel page = new PageInfoModel() { PageIndex = pageIndex, PageSize = pageSize };
            PurchaseOrderService service = new PurchaseOrderService();
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


        [Route("GetPurchaseOrderListForSearch")]
        [HttpGet]
        public DataResultModel GetPurchaseOrderListForSearch(long userid,string keyword, int pageIndex = 1, int pageSize = 10)
        {
            DataResultModel result = new DataResultModel();
            PageInfoModel page = new PageInfoModel() { PageIndex = pageIndex, PageSize = pageSize };
            PurchaseOrderService service = new PurchaseOrderService();
            try
            {
                List<PurchaseOrderModel> models = service.GetListByPage(userid, keyword, ref page);
                result.data = models.Select(p=>new{
                    ID=p.ID,
                    Remark = p.Remark
                });
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


        [Route("SaveRepairOrder")]
        [HttpPost]
        public DataResultModel SaveRepairOrder(RepairOrderModel model)
        {
            DataResultModel result = new DataResultModel();
            RepairOrderService service = new RepairOrderService();
            try
            {
                //model.RepairTime = Convert.ToDateTime(model.RepairTimeFormat);
                result.data = service.Save(model);
            }
            catch (Exception ex)
            {
                result.result = 0; 
                result.message = ex.Message;
            }
            return result;
        }


        [Route("GetRepairOrderByID")]
        [HttpGet]
        public DataResultModel GetRepairOrderByID(long id)
        {
            DataResultModel result = new DataResultModel();
            RepairOrderService service = new RepairOrderService();
            PurchaseOrderService _PurchaseOrderService = new PurchaseOrderService();
            try
            {
                RepairOrderModel model = service.GetByID(id);
                if (model.ID == 0)
                {
                    model.RepairTime = DateTime.Now;
                }
                if (!string.IsNullOrWhiteSpace(model.PurchaseOrderID))
                {
                    model.PurchaseOrder = _PurchaseOrderService.GetByID(Convert.ToInt64(model.PurchaseOrderID));
                }
                result.data = model;
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }



        [Route("DelRepairOrderByID")]
        [HttpGet]
        public DataResultModel DelRepairOrderByID(long id)
        {
            DataResultModel result = new DataResultModel();
            RepairOrderService service = new RepairOrderService();
            PurchaseOrderService _PurchaseOrderService = new PurchaseOrderService();
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

        [Route("GetRepairOrderList")]
        [HttpGet]
        public DataResultModel GetRepairOrderList(long userid,int? UserCarID =0, int pageIndex = 1, int pageSize = 10)
        {
            DataResultModel result = new DataResultModel();
            PageInfoModel page = new PageInfoModel() { PageIndex = pageIndex, PageSize = pageSize };
            RepairOrderService service = new RepairOrderService();
            try
            {
                result.data = service.GetListByPage(userid, UserCarID, ref page);
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }



        #endregion

        #region 图片上传

        //此处的数据格式不在是json 格式，因为上传的类型 是图片
        [Route("UpLoad")]
        [HttpPost]
        public DataResultModel UpLoad()
        {
            DataResultModel result = new DataResultModel();
            PurchaseOrderService service = new PurchaseOrderService();

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context
            HttpRequestBase request = context.Request;//定义传统request对象
            HttpFileCollectionBase imgFiles = request.Files;


            try
            {
                string Url_Path = "";
                //string fileName = System.IO.Path.GetFileName(upImg.FileName);
                Url_Path = UpLoad_Image(imgFiles[0]);
                result.data = Url_Path;
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }

        //将File文件保存到本地，返回物理的相对地址
        private string UpLoad_Image(HttpPostedFileBase file)
        {
            string rootPath = ConfigureHelper.Get("ImageSavePath"); //HttpContext.Request.PhysicalApplicationPath;
            DateTime date = DateTime.Now;
            string directory = rootPath + "Image\\" + date.ToString("yyyyMM");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string filename = directory + "\\" + date.ToString("yyyyMMddHHmmssffff") + ".jpg";
            file.SaveAs(filename);
            string Url_Show = "\\" + filename.Replace(rootPath, "");
            return Url_Show;
        }
        #endregion

    }
}
