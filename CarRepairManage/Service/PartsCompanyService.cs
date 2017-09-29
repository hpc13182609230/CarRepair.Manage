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
    public class PartsCompanyService
    {
        public PartsCompanyModel GetByID(long id)
        {
            PartsCompanyModel model = new PartsCompanyModel();
            PartsCompanyRepository repository = new PartsCompanyRepository();
            var res = repository.GetEntityByID(id);
            model = AutoMapperClient.MapTo<PartsCompany,PartsCompanyModel>(res);
            return model;
        }

        public List<PartsCompanyModel> GetByIDs(List<long> ids)
        {
            List<PartsCompanyModel> models = new List<PartsCompanyModel>();
            PartsCompanyRepository repository = new PartsCompanyRepository();
            var res = repository.GetEntities(p=>ids.Contains(p.ID));
            foreach (var item in res)
            {
                PartsCompanyModel model = AutoMapperClient.MapTo<PartsCompany, PartsCompanyModel>(item);
                models.Add(model);
            }          
            return models;
        }

        public long SavePartsCompany(PartsCompanyModel model)
        {
            PartsCompanyRepository repository = new PartsCompanyRepository();
            PartsCompany entity = new PartsCompany();
            entity= AutoMapperClient.MapTo< PartsCompanyModel,PartsCompany > (model);
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

        public  List<PartsCompanyModel> GetListByPage(string keyword, DateTime startTime, DateTime endTime,ref PageInfoModel page)
        {
            int total = 0;
            List<PartsCompanyModel> models = new List<PartsCompanyModel>();
            PartsCompanyRepository repository = new PartsCompanyRepository();
            var entities = repository.GetEntitiesForPaging(ref total,page.PageIndex,page.PageSize,p=>p.Name.Contains(keyword)&&p.CreateTime>=startTime&&p.CreateTime<=endTime,o=>o.ID);
            page.TotalCount = total;
            foreach (var item in entities)
            {
                PartsCompanyModel model = AutoMapperClient.MapTo<PartsCompany, PartsCompanyModel>(item);
                models.Add(model);
            }
            return models;
        }



    }
}
