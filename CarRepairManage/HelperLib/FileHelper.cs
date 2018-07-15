using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HelperLib
{
    public static class FileHelper
    {


        #region Excel

        public static DataSet ExcelToDS(string Path)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = null;
            strExcel = "select * from [sheet1$]";
            myCommand = new OleDbDataAdapter(strExcel, strConn);
            ds = new DataSet();
            myCommand.Fill(ds, "table1");
            return ds;
        }

        #endregion


        #region 文件 通用

        public static byte[] FileToByte(HttpPostedFileBase file)
        {
            //string fileName = Path.GetFileName(file.FileName);
            byte[] bytes = new byte[file.ContentLength];
            using (BinaryReader reader = new BinaryReader(file.InputStream, Encoding.UTF8))
            {
                bytes = reader.ReadBytes(file.ContentLength);
            }
            //将byte数组转换成字符串
            //string fileAsString = Convert.ToBase64String(bytes);
            return bytes;
        }

        #endregion
    }
}
