﻿using System;
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
            if (res!=null)
            {
                model = AutoMapperClient.MapTo<Points, PointsModel>(res);
            }
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
                Points e = repository.GetEntityOrder(p=>p.WechatUserID==model.WechatUserID,p=>p.ID,false);
                if (e==null)
                {
                    entity.PointSum = entity.point;                 
                }
                else
                {
                    entity.PointSum = entity.point + e.PointSum;
                }
                id = repository.Insert(entity);
            }
            else
            {
                return -2;//订单记录无法修改
            }
            return id;
        }

        /// <summary>
        /// 获取用户 积分列表
        /// </summary>
        /// <param name="WechatUserID"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public  List<PointsModel> GetListByPage(long WechatUserID,ref PageInfoModel page)
        {
            int total = 0;
            List<PointsModel> models = new List<PointsModel>();
            PointsRepository repository = new PointsRepository();
            var entities = repository.GetEntitiesForPaging(ref total, page.PageIndex, page.PageSize, p => p.WechatUserID == WechatUserID, p => p.ID);
            page.TotalCount = total;
            foreach (var item in entities)
            {
                PointsModel model = AutoMapperClient.MapTo<Points, PointsModel>(item);
                models.Add(model);
            }
            return models;
        }

        //获取 今天 活动获得的 几次的次数
        public int GetTodayShareCount(long userid,long options)
        {
            DateTime start = DateTime.Now.Date;
            DateTime end = DateTime.Now.Date.AddDays(1);
            PointsModel model = new PointsModel();
            PointsRepository repository = new PointsRepository();
            var res = repository.GetEntitiesCount(p => p.WechatUserID == userid&&p.PointType==options&&p.CreateTime>=start&&p.CreateTime<end);
            return res;
        }

    }
}
