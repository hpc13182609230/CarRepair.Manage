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
using DapperLib;

namespace Service
{
    public class WechatUserService
    {
        private static WechatUserRepository repository = new WechatUserRepository();

        public WechatUserModel GetByID(long id)
        {
            //WechatUserModel model = repository.GetEntityByID(id);
            WechatUserModel model = DapperContribClient.GetById<WechatUserModel>(2);
            return model;
        }


        public WechatUserModel GetByLoginToken(string LoginToken)
        {
            WechatUserModel model = new WechatUserModel();
            WechatUserRepository repository = new WechatUserRepository();
            var res = repository.GetEntity(p=>p.LoginToken== LoginToken);
            model = AutoMapperClient.MapTo<WechatUser, WechatUserModel>(res);
            return model;
        }

        public WechatUserModel GetByOpenid(string Openid)
        {
            WechatUserModel model = new WechatUserModel();
            WechatUserRepository repository = new WechatUserRepository();
            var res = repository.GetEntity(p => p.Openid == Openid);
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
