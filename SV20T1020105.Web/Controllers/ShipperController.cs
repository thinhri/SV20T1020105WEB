using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020105.BusinessLayers;
using SV20T1020105.DomainModels;
using SV20T1020105.Web.AppCodes;
using SV20T1020105.Web.Models;
using System.Text.RegularExpressions;

namespace SV20T1020105.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class ShipperController : Controller
    {
        private const int PAGE_SIZE = 9;

        private const string SHIPPER_SEARCH = "shipper_search";
        public IActionResult Index()
        {
            PaginationSearchInput input = ApplicationContext.GetSessionData<PaginationSearchInput>(SHIPPER_SEARCH);
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

        public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfShippers(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new ShipperSearchResult()
            {
                Data = data,
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
            };
            ApplicationContext.SetSessionData(SHIPPER_SEARCH, input);
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Thêm mới nhà vận chuyển";
            Shipper model = new Shipper()
            {
                ShipperID = 0
            };
            return View("Edit", model);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin nhà vận chuyển";
            Shipper model = CommonDataService.GetShipper(id);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
        private bool IsPhoneNumberValid(string phoneNumber)
        {
            // Kiểm tra xem chuỗi có chứa ký tự đặc biệt không
            var regex = new Regex("^[0-9]*$");
            return regex.IsMatch(phoneNumber);
        }

        [HttpPost]
        public IActionResult Save(Shipper data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.ShipperName))
                {
                    ModelState.AddModelError(nameof(data.ShipperName), "Tên không được để trống!");
                }
                if (string.IsNullOrWhiteSpace(data.Phone))
                {
                    ModelState.AddModelError(nameof(data.Phone), "Vui lòng nhập số được thoại!");
                }
                else if (data.Phone.Length > 11)
                {
                    ModelState.AddModelError(nameof(data.Phone), "Số điện thoại chỉ được tối đa là 11 chữ số");
                }
                else
                {
                    if (!IsPhoneNumberValid(data.Phone))
                        ModelState.AddModelError(nameof(data.Phone), "Số điện thoại không hợp lệ");
                }
                if (!ModelState.IsValid)
                {
                    ViewBag.Title = data.ShipperID == 0 ? "Bổ sung giao hàng" : "Cập nhật giao hàng";
                    return View("Edit", data);
                }

                if (data.ShipperID == 0)
                {
                    int id = CommonDataService.AddShipper(data);
                }
                else
                {
                    bool result = CommonDataService.UpdateShipper(data);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "Không thể lưu được dữ liệu. Vui lòng thử lại trong vài phút!");
                return View("Edit", data);
            }
            
        }
        public ActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteShipper(id);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetShipper(id);
            if(model == null)
                return RedirectToAction("Index");

            ViewBag.AllowDelete = !CommonDataService.IsUsedShipper(id);
           
            return View(model);
        }
    }

}
