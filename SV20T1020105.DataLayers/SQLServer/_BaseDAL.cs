using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020105.DataLayers.SQLServer
{
    /// <summary>
    /// Lop cha cho cac lop cai dat cac phep xu ly du lieu tren SQL Server
    /// </summary>
    public abstract class _BaseDAL
    {
        protected string _connectionString = "";

        /// <summary>
        /// Ctor: ham tao
        /// </summary>
        /// <param name="connectionString"></param>
        public _BaseDAL(string connectionString)
        {
            _connectionString = connectionString;
        }
        /// <summary>
        /// Tao va mo ket noi den co so du lieu
        /// </summary>
        /// <returns></returns>
        protected SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(); ;
            connection.ConnectionString = _connectionString;
            connection.Open();
            return connection;
        }
    }
}
