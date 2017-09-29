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
            model = AutoMapperClient.MapTo<PurchaseOrder, PurchaseOrderModel>(res);
            return model;
        }

        public int DeleteByID(long id)
        {
            PurchaseOrderRepository repository = new PurchaseOrderRepository();
            var res = repository.GetEntityByID(id);
            var flag = repository.Delete(res);
            return flag;
        }

        public int UpdateStatu(long id ,int statu)
        { 
            PurchaseOrderRepository repository = new PurchaseOrderRepository();
            var entity = repository.GetEntityByID(id);
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
        public  List<PurchaseOrderModel> GetListByPage(long WechatUserID, PageInfoModel page)
        {
            List<PurchaseOrderModel> models = new List<PurchaseOrderModel>();
            PurchaseOrderRepository repository = new PurchaseOrderRepository();
            var entities = repository.GetEntities(p=>p.WechatUserID ==WechatUserID);
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
