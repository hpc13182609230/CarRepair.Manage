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
using HelperLib;
using RedisLib;

namespace Service
{
    public class VehicleTypeService
    {
        VehicleTypeRepository repository = new VehicleTypeRepository();
        const double RedisTTL = 24 * 60 * 60;

        public VehicleTypeModel GetByID(long id)
        {
            VehicleTypeModel model = new VehicleTypeModel();
            var res = repository.GetEntityByID(id);
            model = AutoMapperClient.MapTo<VehicleType, VehicleTypeModel>(res);
            return model;
        }

        public int DeleteByID(long id)
        {
            var res = repository.GetEntityByID(id);
            if (res!=null)
            {
                var flag = repository.Delete(res);
                return flag;
            }
            else
            {
                return 0;
            }
        }

        public long Save(VehicleTypeModel model)
        {
            VehicleType entity = new VehicleType();
            entity = AutoMapperClient.MapTo<VehicleTypeModel, VehicleType>(model);
            long id = 0;
            if (model.ID == 0)
            {
                id = repository.Insert(entity);
            }
            else
            {
                id = repository.Update(entity);
            }
            return id;
        }

        public List<VehicleTypeModel> GetAll()
        {
            string redisKey = CommonUtil.RedisKey.VehicleType_ALL.ToString();
            List<VehicleTypeModel> models = StackExchangeRedisClient.StringGet<List<VehicleTypeModel>>(redisKey);
            if (models == null)
            {
                models = new List<VehicleTypeModel>();
                var entities = repository.GetEntities();
                foreach (var item in entities)
                {
                    VehicleTypeModel model = AutoMapperClient.MapTo<VehicleType, VehicleTypeModel>(item);
                    models.Add(model);
                }
                StackExchangeRedisClient.StringSet(redisKey, models, 0, DateTime.Now.AddSeconds(RedisTTL));
            }
            return models;
        }

        public List<VehicleTypeModel> GetListByPage(string keyword, DateTime startTime, DateTime endTime, ref PageInfoModel page)
        {
            int total = 0;
            List<VehicleTypeModel> models = new List<VehicleTypeModel>();
            var entities = repository.GetEntitiesForPaging(ref total, page.PageIndex, page.PageSize, p => p.Name.Contains(keyword) && p.CreateTime >= startTime && p.CreateTime <= endTime, o => o.ID, false);
            page.TotalCount = total;
            foreach (var item in entities)
            {
                VehicleTypeModel model = AutoMapperClient.MapTo<VehicleType, VehicleTypeModel>(item);
                models.Add(model);
            }
            return models;
        }

    }
}
