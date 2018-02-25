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
    public class ZTestService
    {
        private static ZTestRepository repository = new ZTestRepository();

        public ZTestModel GetByID(long id)
        {
            ZTestModel model = new ZTestModel();
            var res = repository.GetEntityByID(id);
            if (res!=null)
            {
                model = AutoMapperClient.MapTo<ZTest, ZTestModel>(res);
            }
            return model;
        }


        public int DeleteByID(long id)
        {
            var res = repository.GetEntityByID(id);
            var flag = repository.Delete(res);
            return flag;
        }

        public long Save(ZTestModel model)
        {
            ZTest entity = new ZTest();
            entity= AutoMapperClient.MapTo<ZTestModel, ZTest> (model);
            long id = 0;
            if (model.ID==0)
            {

                id = repository.Insert(entity);
            }
            else
            {
                id = repository.Update(entity);
            }
            return id;
        }

        /// <summary>
        /// 获取 列表
        /// </summary>
        /// <param name="WechatUserID"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        //public  List<UserCarsModel> GetListByPage(long WechatUserID,string keyword , ref PageInfoModel page)
        //{
        //    int total = 0;
        //    keyword = keyword ?? "";
        //    List<UserCarsModel> models = new List<UserCarsModel>();
        //    UserCarsRepository repository = new UserCarsRepository();
        //    var entities = repository.GetEntitiesForPaging(ref total,page.PageIndex,page.PageSize,p=>p.WechatUserID==WechatUserID&p.CarNO.Contains(keyword),p=>p.ID);
        //    page.TotalCount = total;
        //    foreach (var item in entities)
        //    {
        //        UserCarsModel model = AutoMapperClient.MapTo<UserCars, UserCarsModel>(item);
        //        models.Add(model);
        //    }
        //    return models;
        //}



    }
}
