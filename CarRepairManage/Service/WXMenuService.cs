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
    public class WXMenuService
    {
        private static WXMenuRepository repository = new WXMenuRepository();

        public WXMenuModel GetByID(long id)
        {
            WXMenuModel model = new WXMenuModel();
            var res = repository.GetEntityByID(id);
            if (res!=null)
            {
                model = AutoMapperClient.MapTo<WXMenu, WXMenuModel>(res);
            }
            return model;
        }


        public int DeleteByID(long id)
        {
            var res = repository.GetEntityByID(id);
            var flag = repository.Delete(res);
            return flag;
        }

        public long Save(WXMenuModel model)
        {
            WXMenu entity = new WXMenu();
            entity= AutoMapperClient.MapTo<WXMenuModel, WXMenu> (model);
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

        public List<WXMenuModel> WXMenu_GetAll()
        {
            List<WXMenuModel> models = new List<WXMenuModel>();
            var res = repository.GetEntities(null,p=>p.ID);
            models = AutoMapperClient.MapToList<WXMenu, WXMenuModel>(res);
            //foreach (var item in res)
            //{
            //    WXMenuModel model  = AutoMapperClient.MapTo<WXMenu, WXMenuModel>(item);
            //    models.Add(model);
            //}
            return models;
        }



    }
}
