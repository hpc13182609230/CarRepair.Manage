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
        public RepairOrderModel GetByID(long id)
        {
            RepairOrderModel model = new RepairOrderModel();
            RepairOrderRepository repository = new RepairOrderRepository();
            var res = repository.GetEntityByID(id);
            model = AutoMapperClient.MapTo<RepairOrder, RepairOrderModel>(res);
            return model;
        }

        public int DeleteByID(long id)
        {

            RepairOrderRepository repository = new RepairOrderRepository();
            var res = repository.GetEntityByID(id);
            var flag = repository.Delete(res);
            return flag;
        }

        public long Save(RepairOrderModel model)
        {
            RepairOrderRepository repository = new RepairOrderRepository();
            RepairOrder entity = new RepairOrder();
            entity= AutoMapperClient.MapTo<RepairOrderModel, RepairOrder> (model);
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
        public  List<RepairOrderModel> GetListByPage(long WechatUserID, PageInfoModel page)
        {
            List<RepairOrderModel> models = new List<RepairOrderModel>();
            RepairOrderRepository repository = new RepairOrderRepository();
            var entities = repository.GetEntities(p=>p.WechatUserID ==WechatUserID);
            foreach (var item in entities)
            {
                RepairOrderModel model = AutoMapperClient.MapTo<RepairOrder, RepairOrderModel>(item);
                models.Add(model);
            }
            return models;
        }



    }
}
