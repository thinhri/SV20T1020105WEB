using SV20T1020105.DataLayers;
using SV20T1020105.DataLayers.SQLServer;
using SV20T1020105.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020105.BusinessLayers
{
    /// <summary>
    /// Cung cap cac chuc nang xu ly du lieu chung
    /// (tinh/thanhh, khach hang, nha cung cap, loai hang, nguoi giao hang, nhan vien)
    /// </summary>
    public static class CommonDataService
    {
        private static readonly ICommonDAL<Province> provinceDB;
        private static readonly ICommonDAL<Customer> customerDB;
        private static readonly ICommonDAL<Category> categoryDB;
        private static readonly ICommonDAL<Supplier> supplierDB;
        private static readonly ICommonDAL<Shipper> shipperDB;
        private static readonly ICommonDAL<Employee> employeeDB;

        /// <summary>
        /// Ctor( cau hoi: static contructor hoat dong nhu the nao?)
        /// </summary>
        static CommonDataService()
        {
            string connectionString = Configuration.ConnectionString;

            provinceDB = new ProvinceDAL(connectionString);
            customerDB = new CustomerDAL(connectionString);
            categoryDB = new CategoryDAL(connectionString);
            shipperDB = new ShipperDAL(connectionString);
            supplierDB = new SupplierDAL(connectionString);
            employeeDB = new EmployeeDAL(connectionString);
        }

        public static List<Province> ListOfProvinces() 
        {
            return provinceDB.List().ToList();
        }

        /// <summary>
        /// Tìm kiếm và lấy danh sách nhà cung cấp
        /// </summary>
        /// <param name="rowCount">Số lượng nhà cung cấp trong danh sách</param>
        /// <param name="page">Trang hiện tại</param>
        /// <param name="pageSize">Số lượng nhà cung cấp trên mỗi trang</param>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <returns>Danh sách nhà cung cấp</returns>
        /// 
        public static List<Supplier> ListOfSuppliers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = supplierDB.Count(searchValue);
            return supplierDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Lấy thông tin một nhà cung cấp theo mã
        /// </summary>
        /// <param name="id">Mã nhà cung cấp</param>
        /// <returns>Thông tin nhà cung cấp</returns>
        public static Supplier? GetSupplier(int id)
        {
            return supplierDB.Get(id);
        }
        /// <summary>
        /// Thêm một nhà cung cấp mới
        /// </summary>
        /// <param name="data">Thông tin nhà cung cấp cần thêm</param>
        /// <returns>Số lượng hàng đã thêm</returns>
        public static int AddSupplier(Supplier supplier)
        {
            return supplierDB.Add(supplier);
        }
        /// <summary>
        /// Cập nhật thông tin một nhà cung cấp
        /// </summary>
        /// <param name="data">Thông tin nhà cung cấp cần cập nhật</param>
        /// <returns>True nếu cập nhật thành công, ngược lại False</returns>
        public static bool UpdateSupplier(Supplier supplier)
        {
            return supplierDB.Update(supplier);
        }
        /// <summary>
        /// Xóa một nhà cung cấp nếu có mã là id
        /// </summary>
        /// <param name="id">Mã nhà cung cấp cần xóa</param>
        /// <returns>True nếu xóa thành công hoặc không có sự sử dụng, ngược lại False</returns>
        public static bool DeleteSupplier(int id)
        {
            if (supplierDB.IsUsed(id))
                return false;
            return supplierDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra xem một nhà cung cấp có được sử dụng hay không
        /// </summary>
        /// <param name="id">Mã nhà cung cấp cần kiểm tra</param>
        /// <returns>True nếu nhà cung cấp được sử dụng, ngược lại False</returns>
        public static bool IsUsedSupplier(int id)
        {
            return supplierDB.IsUsed(id);
        }


        /// <summary>
        /// Tim kiem va lay danh sach khach hang
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        /// 
        public static List<Customer> ListOfCustomers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = customerDB.Count(searchValue);
            return customerDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Lay thong tin cua 1 khach hang theo ma kh
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Customer? GetCustomer(int id)
        {
            return customerDB.Get(id);
        }
        /// <summary>
        /// Bo sung khach hang moi
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static int AddCustomer(Customer customer)
        {
            return customerDB.Add(customer);
        }
        /// <summary>
        /// Cap nhat khach hang
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static bool UpdateCustomer(Customer customer)
        {
            return customerDB.Update(customer);
        }
        /// <summary>
        /// Xoa khach hang co ma la id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteCustomer(int id)
        {
            if (customerDB.IsUsed(id))
                return false;
            return customerDB.Delete(id);
        }
        /// <summary>
        /// Kiem tra xem kh co ma id hien co du lieu lien quan hay khong
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedCustomer(int id)
        { 
            return customerDB.IsUsed(id);
        }

        public static List<Shipper> ListOfShippers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = shipperDB.Count(searchValue);
            return shipperDB.List(page, pageSize, searchValue).ToList();
        }

        public static Shipper? GetShipper(int id)
        {
            return shipperDB.Get(id);
        }

        public static int AddShipper(Shipper shipper)
        {
            return shipperDB.Add(shipper);
        }

        public static bool UpdateShipper(Shipper shipper)
        {
            return shipperDB.Update(shipper);
        }

        public static bool DeleteShipper(int id)
        {
            if (shipperDB.IsUsed(id))
                return false;
            return shipperDB.Delete(id);
        }

        public static bool IsUsedShipper(int id)
        {
            return shipperDB.IsUsed(id);
        }

        /// <summary>
        /// Tìm kiếm và lấy danh sách nhân viên
        /// </summary>
        /// <param name="rowCount">Số lượng nhân viên trong danh sách</param>
        /// <param name="page">Trang hiện tại</param>
        /// <param name="pageSize">Số lượng nhân viên trên mỗi trang</param>
        /// <param name="searchValue">Giá trị tìm kiếm</param>
        /// <returns>Danh sách nhân viên</returns>
        public static List<Employee> ListOfEmployees(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = employeeDB.Count(searchValue);
            return employeeDB.List(page, pageSize, searchValue).ToList();
        }

        /// <summary>
        /// Lấy thông tin một nhân viên theo mã
        /// </summary>
        /// <param name="id">Mã nhân viên</param>
        /// <returns>Thông tin nhân viên</returns>
        public static Employee? GetEmployee(int id)
        {
            return employeeDB.Get(id);
        }

        /// <summary>
        /// Thêm một nhân viên mới
        /// </summary>
        /// <param name="data">Thông tin nhân viên cần thêm</param>
        /// <returns>Số lượng nhân viên đã thêm</returns>
        public static int AddEmployee(Employee data)
        {
            return employeeDB.Add(data);
        }

        /// <summary>
        /// Cập nhật thông tin một nhân viên
        /// </summary>
        /// <param name="data">Thông tin nhân viên cần cập nhật</param>
        /// <returns>True nếu cập nhật thành công, ngược lại False</returns>
        public static bool UpdateEmployee(Employee data)
        {
            return employeeDB.Update(data);
        }

        /// <summary>
        /// Xóa một nhân viên nếu có mã là id
        /// </summary>
        /// <param name="id">Mã nhân viên cần xóa</param>
        /// <returns>True nếu xóa thành công hoặc không có sự sử dụng, ngược lại False</returns>
        public static bool DeleteEmployee(int id)
        {
            if (employeeDB.IsUsed(id))
                return false;
            return employeeDB.Delete(id);
        }

        /// <summary>
        /// Kiểm tra xem một nhân viên có được sử dụng hay không
        /// </summary>
        /// <param name="id">Mã nhân viên cần kiểm tra</param>
        /// <returns>True nếu nhân viên được sử dụng, ngược lại False</returns>
        public static bool IsUsedEmployee(int id)
        {
            return employeeDB.IsUsed(id);
        }


        /// <summary>
        /// Tìm kiếm và lấy danh sách loại hàng
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static List<Category> ListOfCategories(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = categoryDB.Count(searchValue);
            return categoryDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Lấy thông tin 1 loại hàng theo mã
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Category? GetCategory(int id)
        {
            return categoryDB.Get(id);
        }
        /// <summary>
        /// thêm loại hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddCategory(Category category)
        {
            return categoryDB.Add(category);
        }
        /// <summary>
        /// cập nhật loại hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCategory(Category category)
        {
            return categoryDB.Update(category);
        }
        /// <summary>
        /// xoá loại hàng nếu có mã là id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteCategory(int id)
        {
            if (categoryDB.IsUsed(id))
                return false;
            return categoryDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra loại hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedCategory(int id)
        {
            return categoryDB.IsUsed(id);
        }

        public static List<Category> ListOfCategories2(int page = 1, int pageSize = 0, string searchValue = "")
        {
            return categoryDB.List(page, pageSize, searchValue).ToList();
        }

        public static List<Supplier> ListOfSuppliers2(int page = 1, int pageSize = 0, string searchValue = "")
        {
            return supplierDB.List(page, pageSize, searchValue).ToList();
        }

        public static List<Shipper> ListOfShippers(int page = 1, int pageSize = 0, string searchValue = "")
        {
            return shipperDB.List(page, pageSize, searchValue).ToList();
        }
        public static List<Customer> ListOfCustomers2(int page = 1, int pageSize = 0, string searchValue = "")
        {
            return customerDB.List(page, pageSize, searchValue).ToList();
        }
    }

    
}
