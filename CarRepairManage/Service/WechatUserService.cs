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
    public class WechatUserService
    {
        public WechatUserModel GetByID(long id)
        {
            WechatUserModel model = new WechatUserModel();
            WechatUserRepository repository = new WechatUserRepository();
            var res = repository.GetEntityByID(id);
            model = AutoMapperClient.MapTo<WechatUser, WechatUserModel>(res);
            return model;
        }

        public int DeleteByID(long id)
        {
            WechatUserRepository repository = new WechatUserRepository();
            var res = repository.GetEntityByID(id);
            var flag = repository.Delete(res);
            return flag;
        }

        public long Save(WechatUserModel model)
        {
            WechatUserRepository repository = new WechatUserRepository();
            WechatUser entity = AutoMapperClient.MapTo<WechatUserModel, WechatUser> (model);
            long id = 0;
            if (model.ID==0)
            {
                WechatUser e = repository.GetEntity(p=>p.Openid==model.Openid);
                if (e!=null)
                {
                    id = repository.Update(entity); //该数据 已存在 数据库
                }
                else
                {
                    id = repository.Insert(entity);
                }
            }
            else
            {
                id = repository.Update(entity);
            }
            return id;
        }

        /// <summary>
        /// 获取用户 的车库列表
        /// </summary>
        /// <param name="WechatUserID"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public  List<WechatUserModel> GetListByPage(PageInfoModel page)
        {
            List<WechatUserModel> models = new List<WechatUserModel>();
            WechatUserRepository repository = new WechatUserRepository();
            var entities = repository.GetEntities();
            foreach (var item in entities)
            {
                WechatUserModel model = AutoMapperClient.MapTo<WechatUser, WechatUserModel>(item);
                models.Add(model);
            }
            return models;
        }



    }
}
