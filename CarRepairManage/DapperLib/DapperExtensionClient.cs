using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ViewModels;
using DapperExtensions;

namespace DapperLib
{
    public class DapperExtensionClient
    {
        #region init dbconnection
        private static readonly string connString = ConfigurationManager.ConnectionStrings["CarRepairEntities"].ConnectionString;
        private static IDbConnection _conn;
        public static IDbConnection Conn
        {
            get
            {
                _conn = new SqlConnection(connString);
                return _conn;
            }
        }
        #endregion


        #region CURD

        public static long Insert<T>(T model) where T : BaseModel, new()
        {
            using (var coon = Conn)
            {
                coon.Open();
                model.CreateTime = DateTime.Now;
                model.UpdateTime= DateTime.Now;
                long id = coon.Insert(model);
                coon.Close();
                return id;
            }
        }

        public static void Insert<T>(IEnumerable<T> models) where T : BaseModel, new()
        {
            using (var coon = Conn)
            {
                coon.Open();
                Conn.Insert(models);
                coon.Close();
            }
           
        }

        public static bool Delete<T>(T model) where T :  BaseModel, new()
        {
            using (var coon = Conn)
            {
                coon.Open();
                var result = Conn.Delete(model);
                coon.Close();
                return result;
            }
            
        }

        public static void Delete<T>(IEnumerable<T> models) where T :   BaseModel, new()
        {
            using (var coon = Conn)
            {
                coon.Open();
                Conn.Delete(models);
                coon.Close();
            }
        }

        public static bool DeleteByID<T>(long id) where T : BaseModel, new()
        {
            using (var coon = Conn)
            {
                coon.Open();
                var model = Conn.Get<T>(id);
                var result = Conn.Delete(model);
                coon.Close();
                return result;
            }

        }


        public static bool Update<T>(T model) where T : BaseModel, new()
        {
            using (var coon = Conn)
            {
                coon.Open();
                var result = Conn.Update<T>(model);
                coon.Close();
                return result;
            }
        }

        public static bool Update<T>(IEnumerable<T> models) where T : BaseModel, new()
        {
            using (var coon = Conn)
            {
                coon.Open();
                var result = Conn.Update(models);
                coon.Close();
                return result;
            }
        }

        /// <summary>
        /// 根据Id获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetByID<T>(long id) where T : BaseModel, new()
        {
            using (var coon = Conn)
            {
                coon.Open();
                var result = Conn.Get<T>(id);
                coon.Close();
                return result;
            }
        }

        /// <summary>
        /// 根据所有对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetAll<T>() where T : BaseModel, new()
        {
            using (var coon = Conn)
            {
                coon.Open();
                IEnumerable<T> result = Conn.GetList<T>();
                coon.Close();
                return result;
            }          
        }

        #endregion


    }
}
