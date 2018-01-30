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
        PartsCompanyRepository repository = new PartsCompanyRepository();
        AreaService _AreaService = new AreaService();


        public PartsCompanyModel GetByID(long id)
        {
            PartsCompanyModel model = new PartsCompanyModel();
            var res = repository.GetEntityByID(id);
            model = AutoMapperClient.MapTo<PartsCompany,PartsCompanyModel>(res);
            return model;
        }

        public List<PartsCompanyModel> GetByIDs(List<long> ids)
        {
            List<PartsCompanyModel> models = new List<PartsCompanyModel>();
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

        public  List<PartsCompanyModel> GetListByPage(string keyword, string codeID, DateTime startTime, DateTime endTime, ref PageInfoModel page)
        {
            int total = 0;
            List<PartsCompanyModel> models = new List<PartsCompanyModel>();
            AreaModel areas = _AreaService.GetListByParentID("0").Where(p=>p.codeID==codeID).FirstOrDefault();
            var entities = repository.GetEntitiesForPaging(ref total,page.PageIndex,page.PageSize,p=>p.Name.Contains(keyword)&&p.codeID==codeID&&p.CreateTime>=startTime&&p.CreateTime<=endTime,o=>o.ID,false);
            page.TotalCount = total;
            foreach (var item in entities)
            {
                PartsCompanyModel model = AutoMapperClient.MapTo<PartsCompany, PartsCompanyModel>(item);
                model.PicURL = ConfigureHelper.Get("ImageShowURL") + model.PicURL;
                model.Address = areas.name;
                models.Add(model);
            }
            return models;
        }

        public List<PartsCompanyModel> GetListByPage(string keyword,long partsClassifyID, DateTime startTime, DateTime endTime,string codeID, ref PageInfoModel page)
        {
            keyword = string.IsNullOrWhiteSpace(keyword)?"":keyword;
            int total = 0;
            List<PartsCompanyModel> models = new List<PartsCompanyModel>();
            var entities = repository.GetEntitiesForPaging(ref total, page.PageIndex, page.PageSize, p => (p.Name.Contains(keyword)||p.Content.Contains(keyword)) && p.CreateTime >= startTime && p.CreateTime <= endTime&&p.PartsClassifyID==partsClassifyID&&p.codeID==codeID, o => o.Order, false);
            page.TotalCount = total;
            foreach (var item in entities)
            {
                PartsCompanyModel model = AutoMapperClient.MapTo<PartsCompany, PartsCompanyModel>(item);
                models.Add(model);
            }
            return models;
        }

        /// <summary>
        /// 根据配件商的分类值 重新排序
        /// </summary>
        /// <param name="PartsClassifyID"></param>
        /// <returns></returns>
        public string ResetCompanyOrder(string codeID)
        {
            try
            {
                List<string> provinceCodeIDs = new List<string>() { codeID };
                if (string.IsNullOrWhiteSpace(codeID))
                {
                    provinceCodeIDs = _AreaService.GetListByParentID("0").Select(p => p.codeID).ToList();
                }

                foreach (var item in provinceCodeIDs)
                {
                    var entities = repository.GetEntities(p => p.codeID == item).ToList();
                    int count = entities.Count();
                    if (count != 0)
                    {
                        List<int> rands = RandomHelper.GetRandomArray(count, 0, count - 1).ToList();
                        for (int i = 0; i < count; i++)
                        {
                            PartsCompanyModel model = AutoMapperClient.MapTo<PartsCompany, PartsCompanyModel>(entities[i]);
                            model.Order = rands[i];
                            SavePartsCompany(model);
                        }
                    }
                }
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
            
        }




    }
}
