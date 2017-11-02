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
    public class UserCarsService
    {
        public UserCarsModel GetByID(long id)
        {
            UserCarsModel model = new UserCarsModel();
            UserCarsRepository repository = new UserCarsRepository();
            var res = repository.GetEntityByID(id);
            if (res!=null)
            {
                model = AutoMapperClient.MapTo<UserCars, UserCarsModel>(res);
            }
            return model;
        }

        public int CountByUserID(long id)
        {
            UserCarsRepository repository = new UserCarsRepository();
            var res = repository.GetEntitiesCount(p=>p.WechatUserID==id);
            return res;
        }

        public int DeleteByID(long id)
        {
            UserCarsRepository repository = new UserCarsRepository();
            var res = repository.GetEntityByID(id);
            var flag = repository.Delete(res);
            return flag;
        }

        public long Save(UserCarsModel model)
        {
            UserCarsRepository repository = new UserCarsRepository();
            UserCars entity = new UserCars();
            entity= AutoMapperClient.MapTo<UserCarsModel, UserCars> (model);
            long id = 0;
            if (model.ID==0)
            {
                //用户 车牌号 唯一 不能重复
                UserCars e = repository.GetEntity(p=>p.WechatUserID==model.WechatUserID&&p.CarNO==model.CarNO&&p.Attribution==model.Attribution);
                if (e!=null)
                {
                    return -1;//该数据 已存在 数据库
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
        public  List<UserCarsModel> GetListByPage(long WechatUserID,string keyword , ref PageInfoModel page)
        {
            int total = 0;
            keyword = keyword ?? "";
            List<UserCarsModel> models = new List<UserCarsModel>();
            UserCarsRepository repository = new UserCarsRepository();
            var entities = repository.GetEntitiesForPaging(ref total,page.PageIndex,page.PageSize,p=>p.WechatUserID==WechatUserID&p.CarNO.Contains(keyword),p=>p.ID);
            page.TotalCount = total;
            foreach (var item in entities)
            {
                UserCarsModel model = AutoMapperClient.MapTo<UserCars, UserCarsModel>(item);
                models.Add(model);
            }
            return models;
        }

        /// <summary>
        /// 获取 保险销售 的 列表
        /// </summary>
        /// <param name="WechatUserID"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<UserCarsModel> GetListByPage(long WechatUserID, string keyword, DateTime insurance, ref PageInfoModel page)
        {
            int total = 0;
            keyword = keyword ?? "";
            insurance = insurance.AddDays((insurance.Day-1) *(-1)) ;
            DateTime insuranceStart = insurance.AddMonths(-1);

            List<UserCarsModel> models = new List<UserCarsModel>();
            UserCarsRepository repository = new UserCarsRepository();
            var entities = repository.GetEntitiesForPaging(ref total, page.PageIndex, page.PageSize,
                p => p.WechatUserID == WechatUserID && p.CarNO.Contains(keyword) && p.InsuranceTime >= insuranceStart && p.InsuranceTime < insurance
                , p => p.ID);
            page.TotalCount = total;
            foreach (var item in entities)
            {
                UserCarsModel model = AutoMapperClient.MapTo<UserCars, UserCarsModel>(item);
                models.Add(model);
            }
            return models;
        }


    }
}
