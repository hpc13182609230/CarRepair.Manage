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
    public class ManageUserService
    {
        private static ManageUserRepository repository = new ManageUserRepository();
        private static AreaService _AreaService = new AreaService();

        public ManageUserModel GetByID(long id)
        {
            ManageUserModel model = new ManageUserModel();
            var res = repository.GetEntityByID(id);
            if (res != null)
            {
                model = AutoMapperClient.MapTo<ManageUser, ManageUserModel>(res);
            }
            return model;
        }


        public int DeleteByID(long id)
        {
            var res = repository.GetEntityByID(id);
            var flag = repository.Delete(res);
            return flag;
        }

        public long Save(ManageUserModel model)
        {
            ManageUser entity = new ManageUser();
            entity = AutoMapperClient.MapTo<ManageUserModel, ManageUser>(model);
            long id = 0;
            if (model.ID == 0)
            {

                id = repository.Insert(entity);
            }
            else
            {
                id = repository.Update(entity);
            }
            return id;
        }

        public ManageUserModel UserInfo_CheckLogin(string loginName, string password, ref string msg)
        {
            ManageUserModel model = new ManageUserModel();
            using (var entity = new CarRepairEntities())
            {
                // 保存【被添加自动回复  或者 消息自动回复】
                ManageUser dbmodel = entity.ManageUser.Where(p => p.LoginName == loginName).FirstOrDefault();
                if (dbmodel == null)
                {
                    //TransformHelper.ConvertBToA(model, dbmodel);
                    msg = "该用户不存在";
                    return null;
                }
                else
                {
                    string vcSalt = dbmodel.EncryptKey;//加密的关键字

                    string pwd_encryption = EncryptHelper.Md5EncryptStr32((EncryptHelper.Md5EncryptStr32(password) + vcSalt));
                    if (pwd_encryption != dbmodel.Password)
                    {
                        msg = "密码不正确";
                        return null;
                    }
                    else
                    {
                        TransformHelper.ConvertBToA(model, dbmodel);
                        msg = "success";
                        return model;
                    }
                }
            }

        }


        public long UserInfo_Update_LastLoginTime(long id)
        {
            using (var entity = new CarRepairEntities())
            {
                ManageUser dbmodel = entity.ManageUser.Where(p => p.ID == id).FirstOrDefault();
                if (dbmodel == null)
                {
                    return 0;
                }
                else
                {
                    dbmodel.LastActiveTime = DateTime.Now;
                    entity.SaveChanges();
                }
                return dbmodel.ID;
            }

        }

        public List<ManageUserModel> GetAll()
        {
            List<ManageUserModel> models = new List<ManageUserModel>();
            var res = repository.GetEntities();

            foreach (var item in res)
            {
                ManageUserModel model = new ManageUserModel();
                model = AutoMapperClient.MapTo<ManageUser, ManageUserModel>(item);
                if (!string.IsNullOrWhiteSpace(item.AreaCodeID) && item.AreaCodeID!="0")
                {
                    model.Province = _AreaService.GetByCodeID(item.AreaCodeID).name;
                }
              
                models.Add(model);
            }
            return models;
        }
    }
}
