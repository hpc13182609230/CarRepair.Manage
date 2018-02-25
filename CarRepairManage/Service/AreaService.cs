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
using ViewModels.Utility;

namespace Service
{
    public class AreaService
    {
        AreaRepository repository = new AreaRepository();
        int cacheTTL = 7 * 24 * 60 * 60;

        public AreaModel GetByID(long id)
        {
            AreaModel model = new AreaModel();
            var res = repository.GetEntityByID(id);
            model = AutoMapperClient.MapTo<Area, AreaModel>(res);
            return model;
        }

        public int DeleteByID(long id)
        {
            var res = repository.GetEntityByID(id);
            var flag = repository.Delete(res);
            return flag;
        }


        public List<AreaModel> GetListByParentID(string ParentID)
        {
            List<AreaModel> models = CacheHelper.GetCache<List<AreaModel>>(CacheModel.CacheKey_Area_Province);
            if (models==null)
            {
                models = GetListByParentID_DB("0");
                CacheHelper.SetCache<List<AreaModel>>(CacheModel.CacheKey_Area_Province, models, cacheTTL);
            }
            return models;
        }

        public AreaModel MatchProvinceName(string name)
        {
            List<AreaModel> provinces = GetListByParentID("0");
            return provinces.Where(p=>p.name==name).FirstOrDefault();
        }



        #region private
        /// <summary>
        ///  根据 parentID获取 子类集合[直接从数据库中读取]
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        private List<AreaModel> GetListByParentID_DB(string ParentID)
        {
            List<AreaModel> models = new List<AreaModel>();
            var entities = repository.GetEntities(p => p.parentID == ParentID);
            foreach (var item in entities)
            {
             
                AreaModel model = AutoMapperClient.MapTo<Area, AreaModel>(item);
                models.Add(model);
            }
            return models;
        }
        #endregion




    }
}
