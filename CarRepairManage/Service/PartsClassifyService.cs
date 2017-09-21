using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModels;
using ViewModels.CarRepair;
using AutoMapperLib;
using Repository;

namespace Service
{
    public class PartsClassifyService
    {
        public PartsClassifyModel GetByID(long id)
        {
            PartsClassifyModel model = new PartsClassifyModel();
            PartsClassifyRepository repository = new PartsClassifyRepository();
            var res = repository.GetEntityByID(id);
            model = AutoMapperClient.MapTo<PartsClassify, PartsClassifyModel>(res);
            return model;
        }

        public long Save(PartsClassifyModel model)
        {
            PartsClassifyRepository repository = new PartsClassifyRepository();
            PartsClassify entity = new PartsClassify();
            entity = AutoMapperClient.MapTo<PartsClassifyModel, PartsClassify>(model);
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

        public List<PartsClassifyModel> GetByParentID(long parentid)
        {
            List<PartsClassifyModel> models = new List<PartsClassifyModel>();
            PartsClassifyRepository repository = new PartsClassifyRepository();
            var res = repository.GetEntitiesByParentID(parentid);
            foreach (var item in res)
            {
                PartsClassifyModel model = new PartsClassifyModel();
                model = AutoMapperClient.MapTo<PartsClassify, PartsClassifyModel>(item);
                models.Add(model);
            }
            return models;
        }
    }
}
