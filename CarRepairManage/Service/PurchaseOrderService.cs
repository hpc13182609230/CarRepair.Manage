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
using System.Linq.Expressions;

namespace Service
{
    public class RepairOrderService
    {
        RepairOrderRepository repository = new RepairOrderRepository();
        public RepairOrderModel GetByID(long id)
        {
            RepairOrderModel model = new RepairOrderModel();
            var res = repository.GetEntityByID(id);
            if (res!=null)
                model = AutoMapperClient.MapTo<RepairOrder, RepairOrderModel>(res);

            return model;
        }

        public int DeleteByID(long id)
        {
            var res = repository.GetEntityByID(id);
            var flag = repository.Delete(res);
            return flag;
        }

        public long Save(RepairOrderModel model)
        {
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
        public  List<RepairOrderModel> GetListByPage(long WechatUserID, int? UserCarID, ref PageInfoModel page)
        {
            int total = 0;
            List<RepairOrderModel> models = new List<RepairOrderModel>();
            Expression<Func<RepairOrder, bool>> query = (p => p.WechatUserID == WechatUserID);
            if (UserCarID != null && UserCarID != 0)
            {
                query = (p => p.WechatUserID == WechatUserID && p.UserCarID == UserCarID);
            }
            var entities = repository.GetEntitiesForPaging(ref total, page.PageIndex, page.PageSize, query, p => p.ID);
            page.TotalCount = total;
            foreach (var item in entities)
            {
                RepairOrderModel model = AutoMapperClient.MapTo<RepairOrder, RepairOrderModel>(item);
                models.Add(model);
            }
            return models;
        }


        public int CountByUserID(long id)
        {
            var res = repository.GetEntitiesCount(p => p.WechatUserID == id);
            return res;
        }

        
        public List<RepairOrderModel> GetList(long WechatUserID, DateTime startData, DateTime endData)
        {
            List<RepairOrderModel> models = new List<RepairOrderModel>();
            Expression<Func<RepairOrder, bool>> query = (p => p.WechatUserID == WechatUserID && p.RepairTime >= startData && p.RepairTime < endData);
            var entities = repository.GetEntities(query, p => p.ID);
            foreach (var item in entities)
            {
                RepairOrderModel model = AutoMapperClient.MapTo<RepairOrder, RepairOrderModel>(item);
                models.Add(model);
            }
            return models;
        }
    }
}
