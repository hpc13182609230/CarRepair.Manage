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
    public class PartsCallRecordService
    {
        private static PartsCallRecordRepository repository = new PartsCallRecordRepository();
        private static PartsCompanyRepository _PartsCompanyRepository = new PartsCompanyRepository();
        PartsCompanyService _PartsCompanyService = new PartsCompanyService();

        public PartsCallRecordModel GetByID(long id)
        {
            PartsCallRecordModel model = new PartsCallRecordModel();
            var res = repository.GetEntityByID(id);
            if (res!=null)
            {
                model = AutoMapperClient.MapTo<PartsCallRecord, PartsCallRecordModel>(res);
            }
            return model;
        }

        public PartsCallRecordModel GetByCallID(long CallID)
        {
            PartsCallRecordModel model = new PartsCallRecordModel();
            var res = repository.GetEntity(p=>p.CallID== CallID);
            if (res != null)
            {
                model = AutoMapperClient.MapTo<PartsCallRecord, PartsCallRecordModel>(res);
            }
            return model;
        }

        public int DeleteByID(long id)
        {
            var res = repository.GetEntityByID(id);
            var flag = repository.Delete(res);
            return flag;
        }

        public long Save(PartsCallRecordModel model)
        {
            PartsCallRecord entity = new PartsCallRecord();
            entity= AutoMapperClient.MapTo<PartsCallRecordModel, PartsCallRecord> (model);
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

        /// <summary>
        /// 获取 列表
        /// </summary>
        /// <param name="WechatUserID"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<PartsCallRecordModel> GetListByPage(string keyword, DateTime startTime, DateTime endTime, ref PageInfoModel page)
        {
            int total = 0;
            string openid = "";
            List<PartsCallRecordModel> models = new List<PartsCallRecordModel>();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                PartsCompany company =  _PartsCompanyRepository.GetEntity(p=>p.Name== keyword);
                if (company!=null)
                {
                    openid = company.Contract;
                    var entities = repository.GetEntitiesForPaging(ref total, page.PageIndex, page.PageSize, p => p.CreateTime >= startTime && p.CreateTime <= endTime && p.Openid==openid, p => p.ID);
                    page.TotalCount = total;
                    foreach (var item in entities)
                    {
                        PartsCallRecordModel model = AutoMapperClient.MapTo<PartsCallRecord, PartsCallRecordModel>(item);
                        model.PartsName = company.Name;
                        models.Add(model);
                    }
                }
            }
            else
            {
                var entities = repository.GetEntitiesForPaging(ref total, page.PageIndex, page.PageSize, p => p.CreateTime >= startTime && p.CreateTime<= endTime, p => p.ID);
                List<string> Openids = entities.Where(p=>p.Openid!=null).Select(p => p.Openid).Distinct().ToList();
                List<PartsCompanyModel> companys = _PartsCompanyService.GetByOpenids(Openids).ToList();
                page.TotalCount = total;
                foreach (var item in entities)
                {
                    PartsCallRecordModel model = AutoMapperClient.MapTo<PartsCallRecord, PartsCallRecordModel>(item);

                    var cpmpany = companys.Where(p => p.Contract == model.Openid).FirstOrDefault();
                    model.PartsName = cpmpany==null?"": cpmpany.Name;

                    models.Add(model);
                }
            }
            return models;
        }


    }
}
