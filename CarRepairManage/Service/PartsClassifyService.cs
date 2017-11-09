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

namespace Service
{
    public class PartsClassifyService
    {
        public PartsClassify GetByID(long id)
        {
            ViewModels.CarRepair.PartsClassify model = new ViewModels.CarRepair.PartsClassify();
            PartsClassifyRepository repository = new PartsClassifyRepository();
            var res = repository.GetEntityByID(id);
            model = AutoMapperClient.MapTo<EntityModels.PartsClassify, ViewModels.CarRepair.PartsClassify>(res);
            return model;
        }

        public int DeleteByID(long id)
        {
            PartsClassifyRepository repository = new PartsClassifyRepository();
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

        public long Save(ViewModels.CarRepair.PartsClassify model)
        {
            PartsClassifyRepository repository = new PartsClassifyRepository();
            EntityModels.PartsClassify entity = new EntityModels.PartsClassify();
            entity = AutoMapperClient.MapTo<ViewModels.CarRepair.PartsClassify, EntityModels.PartsClassify>(model);
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

        public List<ViewModels.CarRepair.PartsClassify> GetAllByParentID(long parentid)
        {
            List<ViewModels.CarRepair.PartsClassify> models = new List<ViewModels.CarRepair.PartsClassify>();
            PartsClassifyRepository repository = new PartsClassifyRepository();
            var res = repository.GetEntitiesByParentID(parentid);
            foreach (var item in res)
            {
                ViewModels.CarRepair.PartsClassify model = new ViewModels.CarRepair.PartsClassify();
                model = AutoMapperClient.MapTo<EntityModels.PartsClassify, ViewModels.CarRepair.PartsClassify>(item);
                models.Add(model);
            }
            return models;
        }


        public List<ViewModels.CarRepair.PartsClassify> GetAllByParentIDThenOrder(long parentid)
        {
            List<ViewModels.CarRepair.PartsClassify> models = new List<ViewModels.CarRepair.PartsClassify>();
            PartsClassifyRepository repository = new PartsClassifyRepository();
            int total = 0;
            var res = repository.GetEntitiesForPaging(ref total,1,1000,p=>p.OptionID==parentid,p=>p.Order,false);
            foreach (var item in res)
            {
                ViewModels.CarRepair.PartsClassify model = new ViewModels.CarRepair.PartsClassify();
                model = AutoMapperClient.MapTo<EntityModels.PartsClassify, ViewModels.CarRepair.PartsClassify>(item);
                models.Add(model);
            }
            return models;
        }

        public List<ViewModels.CarRepair.PartsClassify> SearchAllByParentIDThenOrder(long parentid,string  keyword)
        {
            keyword = string.IsNullOrWhiteSpace(keyword) ? "" : keyword;
            List<ViewModels.CarRepair.PartsClassify> models = new List<ViewModels.CarRepair.PartsClassify>();
            PartsClassifyRepository repository = new PartsClassifyRepository();
            int total = 0;
            var res = repository.GetEntitiesForPaging(ref total, 1, 1000, p => p.OptionID == parentid &&p.Content.Contains(keyword), p => p.Order, false);
            foreach (var item in res)
            {
                ViewModels.CarRepair.PartsClassify model = new ViewModels.CarRepair.PartsClassify();
                model = AutoMapperClient.MapTo<EntityModels.PartsClassify, ViewModels.CarRepair.PartsClassify>(item);
                models.Add(model);
            }
            return models;
        }



        public List<ViewModels.CarRepair.PartsClassify> GetByParentIDPage(long parentid, string keyword, DateTime startTime, DateTime endTime, ref PageInfoModel page)
        {
            int total = 0;

            List<ViewModels.CarRepair.PartsClassify> models = new List<ViewModels.CarRepair.PartsClassify>();
            PartsClassifyRepository repository = new PartsClassifyRepository();
            var entities = repository.GetEntitiesForPaging(ref total, page.PageIndex, page.PageSize, p => p.OptionID== parentid &&p.Content.Contains(keyword) && p.CreateTime >= startTime && p.CreateTime <= endTime, o => o.ID,true);
            page.TotalCount = total;
            foreach (var item in entities)
            {
                ViewModels.CarRepair.PartsClassify model = new ViewModels.CarRepair.PartsClassify();
                model = AutoMapperClient.MapTo<EntityModels.PartsClassify, ViewModels.CarRepair.PartsClassify>(item);
                models.Add(model);
            }
            return models;
        }

    }
}
