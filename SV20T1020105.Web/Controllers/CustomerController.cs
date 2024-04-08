using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020105.BusinessLayers;
using SV20T1020105.DomainModels;
using SV20T1020105.Web.AppCodes;
using SV20T1020105.Web.Models;

namespace SV20T1020105.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    //Bo sung thong bao khong duoc phep truy cap AccessDenine
    public class CustomerController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string CUSTOMER_SEARCH = "customer_search";
        public IActionResult Index()
        {
            ///Lay dau vao tim kiem hien dang luu lai trong session
            PaginationSearchInput input = ApplicationContext.GetSessionData<PaginationSearchInput>(CUSTOMER_SEARCH);
            ///truong hop trong session chua co dieu kien thi tao dieu kien moi
            if (input == null)
            {
                input = new PaginationSearchInput()
                { 
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }
            
            return View(input);
        }
        /// <summary>
        /// View thuc hien tim kiem ma ko load lai trang
        /// </summary>
        /// <returns></returns>
        public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfCustomers(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new CustomerSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            //Luu lai dien kien tim kiem vao trong Session tai database
            ApplicationContext.SetSessionData(CUSTOMER_SEARCH, input);

            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung khách hàng";
            Customer model = new Customer()
            {
                CustomerID = 0
            };
            return View("Edit", model);
        }

        public IActionResult Edit(int id = 0) 
        {
            ViewBag.Title = "Cập nhật thông tin khách hàng";
            Customer model = CommonDataService.GetCustomer(id);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Customer data) 
        {
            try
            {
                //Kiem soat dau vao va dua cac thong bao loi vao trong ModelState (neu co)
                if (string.IsNullOrWhiteSpace(data.CustomerName))
                    ModelState.AddModelError("CustomerName", "Tên không được để trống");
                if (string.IsNullOrWhiteSpace(data.ContactName))
                    ModelState.AddModelError("ContactName", "Tên giao dịch không được để trống"); 
                if (string.IsNullOrWhiteSpace(data.Email))
                    ModelState.AddModelError("Email", "Vui lòng nhập Email của khách hàng");
                if (string.IsNullOrEmpty(data.Province))
                    ModelState.AddModelError(nameof(data.Province), "Vui lòng chọn tỉnh thành");//Su dung nameof de ten khop

                //Thong bao thuoc tinh IsValid cua ModelState de kiem tra xem co ton tai loi khong
                if (!ModelState.IsValid)
                {
                    ViewBag.Title = data.CustomerID == 0 ? "Bổ sung khách hàng" : "Cập nhật thông tin khách hàng";
                    return View("Edit", data);
                }
                

                if (data.CustomerID == 0)
                {
                    int id = CommonDataService.AddCustomer(data);  
                    if (id <= 0 )
                    {
                        ModelState.AddModelError(nameof(data.CustomerID), "Địa chỉ email bị trùng");
                        return View("Edit", data);
                    }
                }
                else
                {
                    bool result = CommonDataService.UpdateCustomer(data);
                    if (!result)
                    {
                        ModelState.AddModelError(nameof(data.Email), "Địa chỉ email bị trùng với khách hàng khác");
                        return View("Edit", data);
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "Không thể lưu được dữ liêu");
                return View("Edit", data);
            }
            
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteCustomer(id);
                return RedirectToAction("Index");

            }

            var model = CommonDataService.GetCustomer(id);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.AllowDelete = !CommonDataService.IsUsedCustomer(id);
            return View(model);
        }
    }
}
