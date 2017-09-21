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
    }
}
