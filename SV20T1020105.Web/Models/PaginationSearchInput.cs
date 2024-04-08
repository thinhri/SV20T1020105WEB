namespace SV20T1020105.Web.Models
{
    /// <summary>
    /// Dau vao tim kiem du lieu de nhan du lieu duoi dang phan trang
    /// </summary>
    public class PaginationSearchInput
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 0;
        public string SearchValue { get; set; } = "";
    }

    /// <summary>
    /// Dau vao su dung cho tim kiem mat hang
    /// </summary>
    public class ProductSearchInput : PaginationSearchInput
    {
        public int CategoryID { get; set; } = 0;
        public int SupplierID { get; set; } = 0;
    }
}
