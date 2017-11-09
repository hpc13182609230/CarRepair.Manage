using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CarRepair;
using AutoMapperLib;
using Repository;
using ViewModels;
using DapperLib;
using System.Data.SqlClient;
using DapperExtensions;

namespace Service
{
    public class WechatUserService
    {
        private static WechatUserRepository repository = new WechatUserRepository();

        public WechatUser GetByID(long id)
        {
            return repository.GetByID(id);
        }

        public WechatUser GetByLoginToken(string LoginToken)
        {
            return repository.GetByLoginToken(LoginToken);
        }

        public WechatUser GetByOpenid(string Openid)
        {
            return repository.GetByOpenid(Openid);
        }

        public long Save(WechatUser model)
        {
            WechatUserRepository repository = new WechatUserRepository();
            long id = 0;
            if (model.ID==0)
            {
                WechatUser e = repository.GetByOpenid(model.Openid);
                if (e!=null)
                {
                    repository.Update(model); //该数据 已存在 数据库
                    id = model.ID;
                }
                else
                {
                    id = repository.Insert(model);
                }
            }
            else
            {
                repository.Update(model);
                id = model.ID;
            }
            return id;
        }


        //public  List<WechatUser> GetListByPage(PageInfoModel page)
        //{
        //    List<ViewModels.CarRepair.WechatUser> models = new List<ViewModels.CarRepair.WechatUser>();
        //    WechatUserRepository repository = new WechatUserRepository();
        //    var entities = repository.GetEntities();
        //    foreach (var item in entities)
        //    {
        //        ViewModels.CarRepair.WechatUser model = AutoMapperClient.MapTo<EntityModels.WechatUser, ViewModels.CarRepair.WechatUser>(item);
        //        models.Add(model);
        //    }
        //    return models;
        //}



    }
}
