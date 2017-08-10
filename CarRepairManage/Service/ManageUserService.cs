using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelperLib;
using ViewModels;
using EntityModels;

namespace Service
{
    public class ManageUserService
    {
        public int UserInfo_Save(ManageUserModel model)
        {
            using (var entity = new CarRepairEntities())
            {
                ManageUser dbmodel = entity.ManageUser.Where(p => p.ID == model.id).FirstOrDefault();
                if (dbmodel == null)
                {
                    dbmodel = new ManageUser();
                    TransformHelper.ConvertBToA(dbmodel, model);
                    dbmodel.CreateTime = DateTime.Now;
                    entity.ManageUser.Add(dbmodel);
                    var id = entity.SaveChanges();
                }
                else
                {
                    DateTime create = dbmodel.CreateTime;
                    int ID = dbmodel.ID;
                    TransformHelper.ConvertBToA(dbmodel, model);
                    dbmodel.CreateTime = create;
                    dbmodel.ID= ID;
                    entity.SaveChanges();
                }
                return dbmodel.ID;
            }

        }

        public bool UserInfo_Delete(int id)
        {
            using (var entity = new CarRepairEntities())
            {
                ManageUser dbmodel = entity.ManageUser.Where(p => p.ID == id).FirstOrDefault();
                if (dbmodel != null)
                {
                    entity.ManageUser.Remove(dbmodel);
                    entity.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }


        public ManageUserModel WUserInfo_GetByID(int id)
        {
            ManageUserModel model = new ManageUserModel();
            using (var entity = new CarRepairEntities())
            {
                // 保存【被添加自动回复  或者 消息自动回复】
                ManageUser dbmodel = entity.ManageUser.Where(p => p.ID == id).FirstOrDefault();
                if (dbmodel != null)
                {
                    TransformHelper.ConvertBToA(model, dbmodel);
                }
            }
            return model;
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


        public int UserInfo_Update_LastLoginTime(int id)
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
    }
}
