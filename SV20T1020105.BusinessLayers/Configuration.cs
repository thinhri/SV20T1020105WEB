using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020105.BusinessLayers
{
    /// <summary>
    /// Khoi tao luu tru cac thong tin cau hinh cua BusinessLayer
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Chuoi ket thong so ket noi den CSDL
        /// </summary>
        public static string ConnectionString { get; private set; } = "";
        
        /// <summary>
        /// Khoi tao cau hinh cho BusinessLayer
        /// (Ham nay phai duoc goi truoc khi ung dung chay)
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            Configuration.ConnectionString = connectionString;
        }
    }
}

//static class la gi? khac voi class thong thuong cho nao