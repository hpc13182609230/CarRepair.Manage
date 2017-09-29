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
    public class PointsService
    {
        public PointsModel GetByID(long id)
        {
            PointsModel model = new PointsModel();
            PointsRepository repository = new PointsRepository();
            var res = repository.GetEntityByID(id);
            model = AutoMapperClient.MapTo<Points, PointsModel>(res);
            return model;
        }

        public PointsModel GetLastByUserID(long userid)
        {
            PointsModel model = new PointsModel();
            PointsRepository repository = new PointsRepository();
            var res = repository.GetEntityOrder(p=>p.WechatUserID==userid,p=>p.ID,false);
            model = AutoMapperClient.MapTo<Points, PointsModel>(res);
            return model;
        }


        public int DeleteByID(long id)
        {
            PointsRepository repository = new PointsRepository();
            var res = repository.GetEntityByID(id);
            var flag = repository.Delete(res);
            return flag;
        }

        public long Save(PointsModel model)
        {
            PointsRepository repository = new PointsRepository();
            Points entity = new Points();
            entity= AutoMapperClient.MapTo<PointsModel, Points> (model);
            long id = 0;
            if (model.ID==0)
            {
                //用户 车牌号 唯一 不能重复
                Points e = repository.GetEntityOrder(p=>p.WechatUserID==model.WechatUserID,p=>p.ID,false);
                if (e==null)
                {
                    //首次 插入 数据库
                    entity.Remark = "首次获得积分";
                    entity.PointSum = entity.point;
                    id = repository.Insert(entity);
                }
                else
                {
                    entity.PointSum = entity.point+e.PointSum;
                    id = repository.Insert(entity);
                }
            }
            else
            {
                return -2;//订单记录无法修改
            }
            return id;
        }

        /// <summary>
        /// 获取用户 的车库列表
        /// </summary>
        /// <param name="WechatUserID"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public  List<PointsModel> GetListByPage(long WechatUserID, PageInfoModel page)
        {
            List<PointsModel> models = new List<PointsModel>();
            PointsRepository repository = new PointsRepository();
            var entities = repository.GetEntities(p=>p.WechatUserID ==WechatUserID);
            foreach (var item in entities)
            {
                PointsModel model = AutoMapperClient.MapTo<Points, PointsModel>(item);
                models.Add(model);
            }
            return models;
        }



    }
}
