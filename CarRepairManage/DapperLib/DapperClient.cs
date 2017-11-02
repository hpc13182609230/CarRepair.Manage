using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;
using ViewModels.CarRepair;

namespace DapperLib
{
    public  class DapperClient
    {
        #region init dbconnection
        private static readonly string connString = ConfigurationManager.ConnectionStrings["PharmacySystem"].ConnectionString;
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


        #region MyRegion

        public static long Create<T>(T model) where T:class
        {
            return Conn.Insert<T>(model);
        }

        #endregion
    }
}
