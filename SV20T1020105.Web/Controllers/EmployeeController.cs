using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020105.BusinessLayers;
using SV20T1020105.DomainModels;
using SV20T1020105.Web.AppCodes;
using SV20T1020105.Web.Models;
using System.Text.RegularExpressions;

namespace SV20T1020105.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]
    public class EmployeeController : Controller
    {
        private const int PAGE_SIZE = 10;
        private const string EMPLOYEE_SEARCH = "employee_search";

        public IActionResult Index()
        {
            ///Lay dau vao tim kiem hien dang luu lai trong session
            PaginationSearchInput input = ApplicationContext.GetSessionData<PaginationSearchInput>(EMPLOYEE_SEARCH);
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

        public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfEmployees(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new EmployeeSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            //Luu lai dien kien tim kiem vao trong Session tai database
            ApplicationContext.SetSessionData(EMPLOYEE_SEARCH, input);

            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Tạo nhân viên mới";
            Employee model = new Employee()
            {
                EmployeeID = 0,
                BirthDate = new DateTime(2002, 1, 1),
                Photo = "nophoto.png"
            };
            return View("Edit", model);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin nhân viên";
            Employee model = CommonDataService.GetEmployee(id);
            if (model == null)
                return RedirectToAction("Index");

            if (string.IsNullOrEmpty(model.Photo))
                model.Photo = "nophoto.png";

            return View(model);
        }
		private bool IsPhoneNumberValid(string phoneNumber)
		{
			// Kiểm tra xem chuỗi có chứa ký tự đặc biệt không
			var regex = new Regex("^[0-9]*$");
			return regex.IsMatch(phoneNumber);
		}


		[HttpPost]
        public IActionResult Save(Employee data, string birthDateInput, IFormFile? uploadPhoto) 
        {
            //Xu ly Date
            DateTime? birthDate = birthDateInput.ToDateTime();
            if (birthDate.HasValue)
                data.BirthDate = birthDate.Value;

            //Xu ly photo
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";//dat ten anh co thoi gian de tranh trung
                string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\employees"); // duong dan den thu muc luu file anh
                string filePath = Path.Combine(folder, fileName);//Duong dan den file can luu D:\images\employees\photo.png
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }
            try
            {
                //Kiem soat dau vao va dua cac thong bao loi vao trong ModelState (neu co)
                if (string.IsNullOrWhiteSpace(data.FullName))
                    ModelState.AddModelError(nameof(data.FullName), "Tên không được để trống");
				if (string.IsNullOrWhiteSpace(data.Email))
					ModelState.AddModelError(nameof(data.Email), "Vui lòng nhập Email của nhân viên");
				else if (!data.Email.Contains("@"))
					ModelState.AddModelError(nameof(data.Email), "Email phải chứa ký tự '@'");
				if (string.IsNullOrWhiteSpace(data.Phone))
                    ModelState.AddModelError(nameof(data.Phone), "Vui lòng nhập số điện thoại");//Su dung nameof de ten khop
				else if(data.Phone.Length >11)
                {
					ModelState.AddModelError(nameof(data.Phone), "Số điện thoại chỉ được tối đa là 11 chữ số");
				}
                else
				{
					// Kiểm tra và thêm thông báo lỗi nếu số điện thoại không phải là số hoặc chứa ký tự đặc biệt
					if (!IsPhoneNumberValid(data.Phone))
						ModelState.AddModelError(nameof(data.Phone), "Số điện thoại không hợp lệ");
				}
				//Thong bao thuoc tinh IsValid cua ModelState de kiem tra xem co ton tai loi khong
				if (!ModelState.IsValid)
                {
                    ViewBag.Title = data.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật thông tin nhân viên ";
                    return View("Edit", data);
                }

                if (data.EmployeeID == 0)
                {
                    int id = CommonDataService.AddEmployee(data);
                    if (id <= 0)
                    {
                        ModelState.AddModelError(nameof(data.Email), "Địa chỉ email bị trùng");
                        return View("Edit", data);
                    }
                }
                else
                {
                    bool result = CommonDataService.UpdateEmployee(data);
                    if (!result)
                    {
                        ModelState.AddModelError(nameof(data.Email), "Địa chỉ email bị trùng với nhân viên khác");
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
            // Display confirmation interface for deleting an employee
            if (Request.Method == "POST")
            {
                bool result = CommonDataService.DeleteEmployee(id);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetEmployee(id);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.AllowDelete = !CommonDataService.IsUsedEmployee(id);
            return View(model);
        }

        
    }
}
