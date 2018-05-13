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
using LogLib;
using RedisLib;

namespace Service
{
    public class PartsCompanyBindWechatUserService
    {
        PartsCompanyBindWechatUserRepository repository = new PartsCompanyBindWechatUserRepository();
        private static int RedisTTL = 1 * 24 * 60 * 60;//单位是秒

        public PartsCompanyBindWechatUserModel GetByID(long id)
        {
            PartsCompanyBindWechatUserModel model = new PartsCompanyBindWechatUserModel();
            var res = repository.GetEntityByID(id);
            if (res != null)
            {
                model = AutoMapperClient.MapTo<PartsCompanyBindWechatUser, PartsCompanyBindWechatUserModel>(res);
            }
            return model;
        }

        public int DeleteByID(long id)
        {
            var res = repository.GetEntityByID(id);
            var flag = repository.Delete(res);
            return flag;
        }

        public long Save(PartsCompanyBindWechatUserModel model)
        {
            PartsCompanyBindWechatUser entity = new PartsCompanyBindWechatUser();
            entity = AutoMapperClient.MapTo<PartsCompanyBindWechatUserModel, PartsCompanyBindWechatUser>(model);

            PartsCompanyBindWechatUserModel qModel = GetByWechatUserIDAndPartsCompanyID(model.WechatUserID,model.PartsCompanyID);
            Tracer.RunLog(MessageType.WriteInfomation, "", MessageType.Information.ToString(), "qModel = ：" + TransformHelper.SerializeObject(qModel) + "\r\n");
            long id = 0;
            if (qModel.ID == 0)
            {
                id = repository.Insert(entity);
            }
            else
            {
                entity.ID = qModel.ID;
                Tracer.RunLog(MessageType.WriteInfomation, "", MessageType.Information.ToString(), "entity = ：" + TransformHelper.SerializeObject(entity) + "\r\n");
                id = repository.Update(entity);
            }
            return id;
        }

        public PartsCompanyBindWechatUserModel GetByWechatUserIDAndPartsCompanyID(long WechatUserID, long PartsCompanyID)
        {
            PartsCompanyBindWechatUserModel model = new PartsCompanyBindWechatUserModel();
            var res = repository.GetEntity(p=>p.WechatUserID== WechatUserID&&p.PartsCompanyID==PartsCompanyID);
            if (res != null)
            {
                model = AutoMapperClient.MapTo<PartsCompanyBindWechatUser, PartsCompanyBindWechatUserModel>(res);
            }
            return model;
        }

        public List<PartsCompanyBindWechatUserModel> GetListByPage(long WechatUserID, ref PageInfoModel page)
        {
            int total = 0;
            List<PartsCompanyBindWechatUserModel> models = new List<PartsCompanyBindWechatUserModel>();
            var entities = repository.GetEntitiesForPaging(ref total, page.PageIndex, page.PageSize, p => p.WechatUserID == WechatUserID , p => p.UpdateTime);
            page.TotalCount = total;
            foreach (var item in entities)
            {
                PartsCompanyBindWechatUserModel model = AutoMapperClient.MapTo<PartsCompanyBindWechatUser, PartsCompanyBindWechatUserModel>(item);
                models.Add(model);
            }
            return models;
        }

        /// <summary>
        /// 根据时间 获取配件商 的 绑定关系
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<PartsCompanyBindWechatUserModel> GetListByDate(DateTime date)
        {
            string redisKey = CommonUtil.RedisKey.PartsCompanyBindWechatUser_Date.ToString() + ':' + date.ToString("yyyy-MM-dd");
            List<PartsCompanyBindWechatUserModel> models = StackExchangeRedisClient.StringGet<List<PartsCompanyBindWechatUserModel>>(redisKey);
            if (models == null)
            {
                models = new List<PartsCompanyBindWechatUserModel>();
                var entities = repository.GetEntities(p => p.CreateTime >= date);
                foreach (var item in entities)
                {
                    PartsCompanyBindWechatUserModel model = AutoMapperClient.MapTo<PartsCompanyBindWechatUser, PartsCompanyBindWechatUserModel>(item);
                    models.Add(model);
                }
                StackExchangeRedisClient.StringSet(redisKey, models, 0, DateTime.Now.AddSeconds(RedisTTL));
            }
            return models;

        }

    }
}
