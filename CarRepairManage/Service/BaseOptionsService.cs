using AutoMapperLib;
using EntityModels;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CarRepair;

namespace Service
{
    public class BaseOptionsService
    {
        public BaseOptions GetByID(long id)
        {
            ViewModels.CarRepair.BaseOptions model = new ViewModels.CarRepair.BaseOptions();
            BaseOptionsRepository repository = new BaseOptionsRepository();
            var res = repository.GetEntityByID(id);
            model = AutoMapperClient.MapTo<EntityModels.BaseOptions, ViewModels.CarRepair.BaseOptions>(res);
            return model;
        }


        public List<ViewModels.CarRepair.BaseOptions> GetByParentID(long parentid)
        {
            List<ViewModels.CarRepair.BaseOptions> models = new List<ViewModels.CarRepair.BaseOptions>();
            BaseOptionsRepository repository = new BaseOptionsRepository();
            var res = repository.GetEntitiesByParentID(parentid);
            foreach (var item in res)
            {
                ViewModels.CarRepair.BaseOptions model = new ViewModels.CarRepair.BaseOptions();
                model = AutoMapperClient.MapTo<EntityModels.BaseOptions, ViewModels.CarRepair.BaseOptions>(item);
                models.Add(model);
            }
            return models;
        }
    }
}
