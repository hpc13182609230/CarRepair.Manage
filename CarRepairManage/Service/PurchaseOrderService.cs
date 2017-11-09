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
    public class RepairOrderService
    {
        public RepairOrder GetByID(long id)
        {
            ViewModels.CarRepair.RepairOrder model = new ViewModels.CarRepair.RepairOrder();
            RepairOrderRepository repository = new RepairOrderRepository();
            var res = repository.GetEntityByID(id);
            if (res!=null)
                model = AutoMapperClient.MapTo<EntityModels.RepairOrder, ViewModels.CarRepair.RepairOrder>(res);

            return model;
        }

        public int DeleteByID(long id)
        {

            RepairOrderRepository repository = new RepairOrderRepository();
            var res = repository.GetEntityByID(id);
            var flag = repository.Delete(res);
            return flag;
        }

        public long Save(ViewModels.CarRepair.RepairOrder model)
        {
            RepairOrderRepository repository = new RepairOrderRepository();
            EntityModels.RepairOrder entity = new EntityModels.RepairOrder();
            entity= AutoMapperClient.MapTo<ViewModels.CarRepair.RepairOrder, EntityModels.RepairOrder> (model);
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
        public  List<ViewModels.CarRepair.RepairOrder> GetListByPage(long WechatUserID,ref PageInfoModel page)
        {
            int total = 0;
            List<ViewModels.CarRepair.RepairOrder> models = new List<ViewModels.CarRepair.RepairOrder>();
            RepairOrderRepository repository = new RepairOrderRepository();
            var entities = repository.GetEntitiesForPaging(ref total, page.PageIndex, page.PageSize, p => p.WechatUserID == WechatUserID , p => p.ID);
            page.TotalCount = total;
            foreach (var item in entities)
            {
                ViewModels.CarRepair.RepairOrder model = AutoMapperClient.MapTo<EntityModels.RepairOrder, ViewModels.CarRepair.RepairOrder>(item);
                models.Add(model);
            }
            return models;
        }


        public int CountByUserID(long id)
        {
            RepairOrderRepository repository = new RepairOrderRepository();
            var res = repository.GetEntitiesCount(p => p.WechatUserID == id);
            return res;
        }
    }
}
