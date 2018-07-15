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
    public class ShortVideoService
    {
        ShortVideoRepository repository = new ShortVideoRepository();
        const double RedisTTL = 30 * 60;

        public ShortVideoModel GetByID(long id)
        {
            ShortVideoModel model = new ShortVideoModel();
            var res = repository.GetEntityByID(id);
            model = AutoMapperClient.MapTo<ShortVideo, ShortVideoModel>(res);
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

        public long Save(ShortVideoModel model)
        {
            ShortVideo entity = new ShortVideo();
            entity = AutoMapperClient.MapTo<ShortVideoModel, ShortVideo>(model);
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


        public List<ShortVideoModel> GetListByPage(ref PageInfoModel page)
        {
            int total = 0;
            List<ShortVideoModel> models = new List<ShortVideoModel>();
            var entities = repository.GetEntitiesForPaging(ref total, page.PageIndex, page.PageSize, null, p => p.ID);
            page.TotalCount = total;
            foreach (var item in entities)
            {
                ShortVideoModel model = AutoMapperClient.MapTo<ShortVideo, ShortVideoModel>(item);
                models.Add(model);
            }
            return models;
        }

        public List<ShortVideoModel> GetListByPage(string keyword, DateTime startTime, DateTime endTime, ref PageInfoModel page)
        {
            int total = 0;
            List<ShortVideoModel> models = new List<ShortVideoModel>();
            var entities = repository.GetEntitiesForPaging(ref total, page.PageIndex, page.PageSize, p => p.Title.Contains(keyword) &&  p.CreateTime >= startTime && p.CreateTime <= endTime, o => o.ID, false);
            page.TotalCount = total;
            foreach (var item in entities)
            {
                ShortVideoModel model = AutoMapperClient.MapTo<ShortVideo, ShortVideoModel>(item);
                models.Add(model);
            }
            return models;
        }

    }
}
