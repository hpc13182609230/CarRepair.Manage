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
    public class ManageMenuService
    {
        private static ManageMenuRepository repository = new ManageMenuRepository();

        public ManageMenuModel GetByID(long id)
        {
            ManageMenuModel model = new ManageMenuModel();
            var res = repository.GetEntityByID(id);
            if (res!=null)
            {
                model = AutoMapperClient.MapTo<ManageMenu, ManageMenuModel>(res);
            }
            return model;
        }


        public int DeleteByID(long id)
        {
            var res = repository.GetEntityByID(id);
            var flag = repository.Delete(res);
            return flag;
        }

        public long Save(ManageMenuModel model)
        {
            ManageMenu entity = new ManageMenu();
            entity= AutoMapperClient.MapTo<ManageMenuModel, ManageMenu> (model);
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

        public List<ManageMenuModel> GetByIDs(List<long> ids)
        {
            List<ManageMenuModel> models = new List<ManageMenuModel>();
            var res = repository.GetEntities(p => ids.Contains(p.ID));

            foreach (var item in res)
            {
                ManageMenuModel model = new ManageMenuModel();
                model = AutoMapperClient.MapTo<ManageMenu, ManageMenuModel>(item);
                models.Add(model);
            }
            return models;
        }

    }
}
