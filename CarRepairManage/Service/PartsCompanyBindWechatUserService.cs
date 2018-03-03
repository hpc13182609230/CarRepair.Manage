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

namespace Service
{
    public class PartsCompanyBindWechatUserService
    {
        PartsCompanyBindWechatUserRepository repository = new PartsCompanyBindWechatUserRepository();

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



    }
}
