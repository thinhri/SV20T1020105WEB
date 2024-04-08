using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020105.DataLayers
{
    /// <summary>
    /// Mo ta cac phep xu ly du lieu chung
    /// </summary>
    public interface ICommonDAL<T> where T : class
    {
        /// <summary>
        /// Tim kiem va lay danh sach du lieu duoi dang phan trang
        /// </summary>
        /// <param name="page">Trang can hien thi</param>
        /// <param name="pageSize">So dong hien thi tren moi trang (=0 neu khong phan trang du lieu)</param>
        /// <param name="searchValue">Gia tri can tim kiem( chuoi rong neu lay toan bo du lieu)</param>
        /// <returns></returns>
        IList<T> List(int page = 1, int pageSize = 0, string searchValue = "");
        /// <summary>
        /// Dem so long du lieu tim duoc
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue = "");
        /// <summary>
        /// Bo sung du lieu vao co so du lieu. Ham tra ve ID cua du lieu duoc bo sung
        /// (tra ve 0 neu viec bo sung khong thanh cong)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(T data);
        /// <summary>
        /// Cap nhat du lieu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(T data);
        /// <summary>
        /// Xoa du lieu dua tren id (ma)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id);
        /// <summary>
        /// Lay 1 bang ghi du lieu dua vao id (tra ve null neu du lieu khong ton tai)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T? Get(int id);
        /// <summary>
        /// Kiem tra xem ban ghi du lieu ma id hien dang co duoc su dung boi cac du lieu khac hay khong?
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsUsed(int id);
    }
}
