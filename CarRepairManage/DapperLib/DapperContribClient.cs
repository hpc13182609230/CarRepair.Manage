using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;
using ViewModels;
using ViewModels.CarRepair;

namespace DapperLib
{
    public class DapperContribClient 
    {
        #region init dbconnection
        private static readonly string connString = ConfigurationManager.ConnectionStrings["CarRepairEntities"].ConnectionString;
        private static IDbConnection _conn;
        public static IDbConnection Conn
        {
            get
            {
                _conn = new SqlConnection(connString);
                _conn.Open();
                return _conn;
            }
        }
        #endregion

        #region CURD

        public static long Insert<T>(T model) where T : BaseModel, new()
        {
            using (var coon = Conn)
            {
                model.CreateTime = DateTime.Now;
                return coon.Insert(model);
            }
        }

        public static long Insert<T>(IEnumerable<T> models) where T : BaseModel,new()
        {
            return Conn.Insert(models);
        }

        public static bool Delete<T>(T model) where T : class
        {
            return Conn.Delete(model);
        }

        public static bool Delete<T>(IEnumerable<T> models) where T : class
        {
            return Conn.Delete(models);
        }

        public static bool DeleteAll<T>() where T : class
        {
            return Conn.DeleteAll<T>();
        }

        public static bool Update<T>(T model) where T : class
        {
            return Conn.Update<T>(model);
        }

        public static bool Update<T>(IEnumerable<T> models) where T : class
        {
            return Conn.Update(models);
        }

        /// <summary>
        /// 根据Id获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetById<T>(long id) where T : class
        {
            using (var coon = Conn)
            {
                return Conn.Get<T>(id);
            }
        }

        /// <summary>
        /// 根据所有对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetAll<T>() where T : class
        {
            Console.WriteLine( "A0 = "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            IEnumerable<T> result = Conn.GetAll<T>();
            Console.WriteLine("A1 = " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            var data = result.ToList();
            Console.WriteLine("A2 = " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));

            return result;
        }

        #endregion
    }
}
