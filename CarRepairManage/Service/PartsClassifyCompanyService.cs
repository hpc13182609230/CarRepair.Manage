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
    public class PartsClassifyCompanyService
    {
        public PartsClassifyCompanyModel GetByID(long id)
        {
            PartsClassifyCompanyModel model = new PartsClassifyCompanyModel();
            PartsClassifyCompanyRepository repository = new PartsClassifyCompanyRepository();
            var res = repository.GetEntityByID(id);
            model = AutoMapperClient.MapTo<PartsClassifyCompany, PartsClassifyCompanyModel>(res);
            return model;
        }

        public PartsClassifyCompanyModel GetByPartsCompanyID(long PartsCompanyID)
        {
            PartsClassifyCompanyModel model = new PartsClassifyCompanyModel();
            PartsClassifyCompanyRepository repository = new PartsClassifyCompanyRepository();
            var res = repository.GetEntities(p=>p.PartsCompanyID==PartsCompanyID).FirstOrDefault();
            if (res != null)
            {
                model = AutoMapperClient.MapTo<PartsClassifyCompany, PartsClassifyCompanyModel>(res);
            }
            return model;
        }

        public long Save(PartsClassifyCompanyModel model)
        {
            PartsClassifyCompanyRepository repository = new PartsClassifyCompanyRepository();
            PartsClassifyCompany entity = new PartsClassifyCompany();
            entity= AutoMapperClient.MapTo<PartsClassifyCompanyModel, PartsClassifyCompany> (model);
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

        public List<PartsClassifyCompanyModel> GetAllByClassifyID(string ClassifyID)
        {
            List<PartsClassifyCompanyModel> models = new List<PartsClassifyCompanyModel>();
            PartsClassifyCompanyRepository repository = new PartsClassifyCompanyRepository();
            var res = repository.GetEntities(p => p.PartsClassifyIDs.Contains(ClassifyID));
            foreach (var item in res)
            {
                PartsClassifyCompanyModel model = AutoMapperClient.MapTo<PartsClassifyCompany, PartsClassifyCompanyModel>(item);
                models.Add(model);
            }
            return models;
        }

        public List<PartsCompanyModel> GetForAPI(long partsClassifyID)
        {
            //int start = page.Start;
            //int offset = page.Offset;
            //long total = 0; 

            List<PartsClassifyCompanyModel> _PartsClassifyCompanyModels = GetAllByClassifyID(partsClassifyID.ToString());
            List<long> companyIDs = _PartsClassifyCompanyModels.Select(p => p.PartsCompanyID).ToList();
            PartsCompanyService _PartsCompanyService = new PartsCompanyService();
            List<PartsCompanyModel> models = _PartsCompanyService.GetByIDs(companyIDs);

            return models;
        }

    }
}
