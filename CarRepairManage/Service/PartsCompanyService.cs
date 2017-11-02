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

        public int DeleteByID(long id)
        {
            PartsCompanyRepository repository = new PartsCompanyRepository();
            var res = repository.GetEntityByID(id);
            if (res != null)
            {
                var flag = repository.Delete(res);
                return flag;
            }
            else
            {
                return 0;
            }
        }

        public  List<PartsCompanyModel> GetListByPage(string keyword, DateTime startTime, DateTime endTime,ref PageInfoModel page)
        {
            int total = 0;
            List<PartsCompanyModel> models = new List<PartsCompanyModel>();
            PartsCompanyRepository repository = new PartsCompanyRepository();
            var entities = repository.GetEntitiesForPaging(ref total,page.PageIndex,page.PageSize,p=>p.Name.Contains(keyword)&&p.CreateTime>=startTime&&p.CreateTime<=endTime,o=>o.ID,false);
            page.TotalCount = total;
            foreach (var item in entities)
            {
                PartsCompanyModel model = AutoMapperClient.MapTo<PartsCompany, PartsCompanyModel>(item);
                model.PicURL = ConfigureHelper.Get("ImageShowURL") + model.PicURL;
                models.Add(model);
            }
            return models;
        }

        public List<PartsCompanyModel> GetListByPage(string keyword,long partsClassifyID, DateTime startTime, DateTime endTime, ref PageInfoModel page)
        {
            keyword = string.IsNullOrWhiteSpace(keyword)?"":keyword;
            int total = 0;
            List<PartsCompanyModel> models = new List<PartsCompanyModel>();
            PartsCompanyRepository repository = new PartsCompanyRepository();
            var entities = repository.GetEntitiesForPaging(ref total, page.PageIndex, page.PageSize, p => (p.Name.Contains(keyword)||p.Content.Contains(keyword)) && p.CreateTime >= startTime && p.CreateTime <= endTime&&p.PartsClassifyID==partsClassifyID, o => o.ID, false);
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
