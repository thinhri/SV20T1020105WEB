using SV20T1020105.DomainModels;
namespace SV20T1020105.Web.Models
{
    /// <summary>
    /// Lop cha cho cac lop bieu dien du lieu ket qua tim kiem, phan trang
    /// </summary>
    public abstract class BasePaginationResult
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchValue { get; set; } = "";
        public int RowCount { get; set; }
        public int PageCount
        {
            get
            {
                if (PageSize == 0)
                    return 1;
                int c = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                    c += 1;
                return c;
            }
        }
    }
    /// <summary>
    /// Ket qua tim kiem va lay danh sach khach hang
    /// </summary>
    public class CustomerSearchResult : BasePaginationResult
    {
        public List<Customer> Data { get; set; }
    }
    /// <summary>
    /// Ket qua tim kiem va lay danh sach loai hang
    /// </summary>
    public class CategorySearchResult : BasePaginationResult 
    { 
        public List<Category> Data { get; set; }
    }
    /// <summary>
    /// Ket qua tim kiem va lay danh sach nguoi giao hang
    /// </summary>
    public class ShipperSearchResult : BasePaginationResult
    {
        public List<Shipper> Data { get; set; }
    }
    /// <summary>
    /// Ket qua tim kiem va lay danh sach nha cung cap
    /// </summary>
    public class SupplierSearchResult : BasePaginationResult
    {
        public List<Supplier> Data { get; set; }
    }
    /// <summary>
    /// Ket qua tim kiem va lay danh sach nhan vien
    /// </summary>
    public class EmployeeSearchResult : BasePaginationResult
    {
        public List<Employee> Data { get; set; }
    }

    public class ProductSearchResult : BasePaginationResult
    {
        public List<Product> Data { get; set; }
        public List<Category> Categories { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public List<ProductPhoto> DataPhoto { get; set; }
        public List<ProductAttribute> DataAttribute { get; set; }

    }
}
