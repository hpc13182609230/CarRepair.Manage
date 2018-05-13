using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModels;
using ViewModels.CarRepair;
using AutoMapperLib;
using Repository;
using ViewModels;

namespace Service
{
    public class PurchaseOrderService
    {
        public PurchaseOrderModel GetByID(long id)
        {
            PurchaseOrderModel model = new PurchaseOrderModel();
            PurchaseOrderRepository repository = new PurchaseOrderRepository();
            var res = repository.GetEntityByID(id);
            if (res!=null)
            {
                model = AutoMapperClient.MapTo<PurchaseOrder, PurchaseOrderModel>(res);
            }
            return model;
        }

        public int DeleteByID(long id)
        {
            PurchaseOrderRepository repository = new PurchaseOrderRepository();
            var res = repository.GetEntityByID(id);
            var flag = repository.Delete(res);
            return flag;
        }

        public long UpdateStatu(long id ,int statu)
        { 
            PurchaseOrderRepository repository = new PurchaseOrderRepository();
            var entity = repository.GetEntityByID(id);
            if (string.IsNullOrWhiteSpace(entity.PicURL))
                return -1;
            entity.Statu = statu;
            return repository.Update(entity);
        }

        public long Save(PurchaseOrderModel model)
        {
            PurchaseOrderRepository repository = new PurchaseOrderRepository();
            PurchaseOrder entity = new PurchaseOrder();
            entity= AutoMapperClient.MapTo<PurchaseOrderModel, PurchaseOrder> (model);
            long id = 0;
            if (model.ID==0)
            {
                id = repository.Insert(entity);
            }
            else
            {
                id = repository.Update(entity);
            }
            return id;
        }

        /// <summary>
        /// 获取用户 的车库列表
        /// </summary>
        /// <param name="WechatUserID"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public  List<PurchaseOrderModel> GetListByPage(long WechatUserID,ref PageInfoModel page)
        {
            int total = 0;
            List<PurchaseOrderModel> models = new List<PurchaseOrderModel>();
            PurchaseOrderRepository repository = new PurchaseOrderRepository();
            var entities = repository.GetEntitiesForPaging(ref total, page.PageIndex, page.PageSize, p => p.WechatUserID == WechatUserID, p => p.ID);
            page.TotalCount = total;
            foreach (var item in entities)
            {
                PurchaseOrderModel model = AutoMapperClient.MapTo<PurchaseOrder, PurchaseOrderModel>(item);
                models.Add(model);
            }
            return models;
        }


        public List<PurchaseOrderModel> GetListByPage(long WechatUserID, string keyword,ref PageInfoModel page)
        {
            keyword = string.IsNullOrWhiteSpace(keyword) ?"": keyword.Trim();
            int total = 0;
            List<PurchaseOrderModel> models = new List<PurchaseOrderModel>();
            PurchaseOrderRepository repository = new PurchaseOrderRepository();
            var entities = repository.GetEntitiesForPaging(ref total, page.PageIndex, page.PageSize, p => p.WechatUserID == WechatUserID&&p.Remark.Contains(keyword), p => p.ID);
            page.TotalCount = total;
            foreach (var item in entities)
            {
                PurchaseOrderModel model = AutoMapperClient.MapTo<PurchaseOrder, PurchaseOrderModel>(item);
                models.Add(model);
            }
            return models;
        }

        public int CountByUserID(long id)
        {
            PurchaseOrderRepository repository = new PurchaseOrderRepository();
            var res = repository.GetEntitiesCount(p => p.WechatUserID == id);
            return res;
        }

    }
}
