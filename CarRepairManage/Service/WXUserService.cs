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

namespace Service
{
    public class WXUserService
    {
        WXUserRepository repository = new WXUserRepository();

        public WXUserModel GetByID(long id)
        {
            WXUserModel model = new WXUserModel();
            var res = repository.GetEntityByID(id);
            if (res != null)
            {
                model = AutoMapperClient.MapTo<WXUser, WXUserModel>(res);
            }
            return model;
        }


        public int DeleteByID(long id)
        {
            var res = repository.GetEntityByID(id);
            var flag = repository.Delete(res);
            return flag;
        }

        public long Save(WXUserModel model)
        {
            WXUser entity = new WXUser();
            entity = AutoMapperClient.MapTo<WXUserModel, WXUser>(model);

            WXUserModel user= GetByUnionID(model.Unionid);
            long id = 0;
            if (user.ID == 0)
            {

                id = repository.Insert(entity);
            }
            else
            {
                id = repository.Update(entity);
            }
            return id;
        }



        public WXUserModel GetByUnionID(string unionid)
        {
            WXUserModel model = new WXUserModel();
            var res = repository.GetEntity(p=>p.Unionid==unionid);
            if (res != null)
            {
                model = AutoMapperClient.MapTo<WXUser, WXUserModel>(res);
            }
            return model;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="WechatUserID"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<WXUserModel> GetListByPage(string keyword, DateTime startTime, DateTime endTime, ref PageInfoModel page)
        {
            int total = 0;
            keyword = keyword ?? "";
            List<WXUserModel> models = new List<WXUserModel>();
            var entities = repository.GetEntitiesForPaging(ref total, page.PageIndex, page.PageSize, p => p.NickName.Contains(keyword) && p.CreateTime >= startTime && p.CreateTime <= endTime, p => p.ID);
            page.TotalCount = total;
            foreach (var item in entities)
            {
                WXUserModel model = AutoMapperClient.MapTo<WXUser, WXUserModel>(item);
                models.Add(model);
            }
            return models;
        }


    }
}
