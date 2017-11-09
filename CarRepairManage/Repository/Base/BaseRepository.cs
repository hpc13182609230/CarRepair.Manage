using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ViewModels.CarRepair;
using ViewModels;
using DapperLib;
using LogLib;

namespace Repository
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseModel,new()
    {

        #region 基础增删改

        public bool Insert(IEnumerable<T> models)
        {
            try
            {
                DapperExtensionClient.Insert<T>(models);
                return true;
            }
            catch (Exception ex)
            {
                LogLib.Tracer.RunLog(LogLib.MessageType.WriteInfomation, "", "Exception", "BaseRepository Insert" + " ex = " + ex.Message + "\r\n");
                return false;
            } 
        }

        public long Insert(T model)
        {
            return DapperExtensionClient.Insert<T>(model);
        }

        public bool Delete(T model)
        {
           return  DapperExtensionClient.Delete<T>(model);
        }

        public bool Delete(IEnumerable<T> models)
        {
            try
            {
                DapperExtensionClient.Delete<T>(models);
                return true;
            }
            catch (Exception ex)
            {
                LogLib.Tracer.RunLog(LogLib.MessageType.WriteInfomation, "", "Exception", "BaseRepository Delete" + " ex = " + ex.Message + "\r\n");
                return false;
            }  
        }

        public bool DeleteByID(long id)
        {
            return DapperExtensionClient.DeleteByID<T>(id);
        }

        public bool Update(IEnumerable<T> models)
        {
            return DapperExtensionClient.Update<T>(models);
        }


        public bool Update(T model)
        {
            return DapperExtensionClient.Update<T>(model);
        }

        public T GetByID(long id)
        {
            return DapperExtensionClient.GetByID<T>(id);
        }

        public IEnumerable<T> GetAll()
        {
            return DapperExtensionClient.GetAll<T>();
        }

        

        #endregion
    }
}
