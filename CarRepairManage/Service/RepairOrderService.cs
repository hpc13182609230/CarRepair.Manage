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
        public PurchaseOrder GetByID(long id)
        {
            ViewModels.CarRepair.PurchaseOrder model = new ViewModels.CarRepair.PurchaseOrder();
            PurchaseOrderRepository repository = new PurchaseOrderRepository();
            var res = repository.GetEntityByID(id);
            if (res!=null)
            {
                model = AutoMapperClient.MapTo<EntityModels.PurchaseOrder, ViewModels.CarRepair.PurchaseOrder>(res);
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

        public int UpdateStatu(long id ,int statu)
        { 
            PurchaseOrderRepository repository = new PurchaseOrderRepository();
            var entity = repository.GetEntityByID(id);
            if (string.IsNullOrWhiteSpace(entity.PicURL))
                return -1;
            entity.Statu = statu;
            return repository.Update(entity);
        }

        public long Save(ViewModels.CarRepair.PurchaseOrder model)
        {
            PurchaseOrderRepository repository = new PurchaseOrderRepository();
            EntityModels.PurchaseOrder entity = new EntityModels.PurchaseOrder();
            entity= AutoMapperClient.MapTo<ViewModels.CarRepair.PurchaseOrder, EntityModels.PurchaseOrder> (model);
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
        public  List<ViewModels.CarRepair.PurchaseOrder> GetListByPage(long WechatUserID,ref PageInfoModel page)
        {
            int total = 0;
            List<ViewModels.CarRepair.PurchaseOrder> models = new List<ViewModels.CarRepair.PurchaseOrder>();
            PurchaseOrderRepository repository = new PurchaseOrderRepository();
            var entities = repository.GetEntitiesForPaging(ref total, page.PageIndex, page.PageSize, p => p.WechatUserID == WechatUserID, p => p.ID);
            page.TotalCount = total;
            foreach (var item in entities)
            {
                ViewModels.CarRepair.PurchaseOrder model = AutoMapperClient.MapTo<EntityModels.PurchaseOrder, ViewModels.CarRepair.PurchaseOrder>(item);
                models.Add(model);
            }
            return models;
        }


        public List<ViewModels.CarRepair.PurchaseOrder> GetListByPage(long WechatUserID, string keyword,ref PageInfoModel page)
        {
            keyword = string.IsNullOrWhiteSpace(keyword) ?"": keyword.Trim();
            int total = 0;
            List<ViewModels.CarRepair.PurchaseOrder> models = new List<ViewModels.CarRepair.PurchaseOrder>();
            PurchaseOrderRepository repository = new PurchaseOrderRepository();
            var entities = repository.GetEntitiesForPaging(ref total, page.PageIndex, page.PageSize, p => p.WechatUserID == WechatUserID&&p.Remark.Contains(keyword), p => p.ID);
            page.TotalCount = total;
            foreach (var item in entities)
            {
                ViewModels.CarRepair.PurchaseOrder model = AutoMapperClient.MapTo<EntityModels.PurchaseOrder, ViewModels.CarRepair.PurchaseOrder>(item);
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
