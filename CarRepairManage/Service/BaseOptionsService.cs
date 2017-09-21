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
        public BaseOptionsModel GetByID(long id)
        {
            BaseOptionsModel model = new BaseOptionsModel();
            BaseOptionsRepository repository = new BaseOptionsRepository();
            var res = repository.GetEntityByID(id);
            model = AutoMapperClient.MapTo<BaseOptions, BaseOptionsModel>(res);
            return model;
        }


        public List<BaseOptionsModel> GetByParentID(long parentid)
        {
            List<BaseOptionsModel> models = new List<BaseOptionsModel>();
            BaseOptionsRepository repository = new BaseOptionsRepository();
            var res = repository.GetEntitiesByParentID(parentid);
            foreach (var item in res)
            {
                BaseOptionsModel model = new BaseOptionsModel();
                model = AutoMapperClient.MapTo<BaseOptions, BaseOptionsModel>(item);
                models.Add(model);
            }
            return models;
        }
    }
}
