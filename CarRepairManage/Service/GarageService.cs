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
using System.Data;
using LogLib;

namespace Service
{
    public class GarageService
    {
        private static GarageRepository repository = new GarageRepository();

        public GarageModel GetByID(long id)
        {
            GarageModel model = new GarageModel();
            var res = repository.GetEntityByID(id);
            if (res!=null)
            {
                model = AutoMapperClient.MapTo<Garage, GarageModel>(res);
            }
            return model;
        }


        public int DeleteByID(long id)
        {
            var res = repository.GetEntityByID(id);
            var flag = repository.Delete(res);
            return flag;
        }

        public long Save(GarageModel model)
        {
            Garage original = repository.GetEntity(p => p.Phone == model.Phone||p.Mobile==model.Mobile);
            long id = 0;
            if (original == null)
            {
                Garage entity = AutoMapperClient.MapTo<GarageModel, Garage>(model);
                id = repository.Insert(entity);
            }
            else
            {
                original.CompanyName = model.CompanyName;
                original.BossName = model.BossName;
                original.Mobile = model.Mobile;
                original.IsCheck = model.IsCheck;
                id = repository.Update(original);
            }
            return id;
        }

        /// <summary>
        /// 获取 列表
        /// </summary>
        /// <param name="WechatUserID"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<GarageModel> GetListByPage(string keyword, DateTime startTime, DateTime endTime, ref PageInfoModel page)
        {
            int total = 0;
            List<GarageModel> models = new List<GarageModel>();
            var entities = repository.GetEntitiesForPaging(ref total, page.PageIndex, page.PageSize, p => (p.CompanyName.Contains(keyword) || p.BossName.Contains(keyword)) && p.CreateTime >= startTime && p.CreateTime <= endTime, o => o.ID, false);
            page.TotalCount = total;
            foreach (var item in entities)
            {
                GarageModel model = AutoMapperClient.MapTo<Garage, GarageModel>(item);
                models.Add(model);
            }
            return models;
        }


        public GarageModel GetAndSaveByNumber(string number)
        {
            GarageModel model = new GarageModel();
            var res = repository.GetEntity(p=>p.Phone== number||p.Mobile.Contains(number));
            if (res != null)
            {
                model = AutoMapperClient.MapTo<Garage, GarageModel>(res);
            }
            else
            {
                if (number.Length==11||number.Length==12)
                {
                    model.Phone = number;
                }
                else
                {
                    model.Mobile = number;
                    model.IsCheck = 0;
                }
                model.ID=Save(model);
            }
            return model;
        }


        public bool  ExportDataFromDataset(DataSet ds)
        {
            try
            {
                var table = ds.Tables[0];
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    GarageModel model = new GarageModel();
                    model.CompanyName = table.Rows[i][0].ToString();
                    model.BossName = table.Rows[i][1].ToString();               
                    model.Phone = table.Rows[i][2].ToString();
                    model.Mobile = table.Rows[i][3].ToString();
                    model.IsCheck =Convert.ToInt32(table.Rows[i][4].ToString());
                    Save(model);
                }
            }
            catch (Exception ex)
            {
                Tracer.RunLog(MessageType.WriteInfomation, "", MessageType.Error.ToString(), "ExportDataFromDataset  ex=  ：" + ex.Message + "\r\n");
                return false;
            }
            return true;
        }



    }
}
