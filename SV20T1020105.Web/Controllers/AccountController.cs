using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020105.BusinessLayers;
using SV20T1020105.Web.AppCodes;
using Microsoft.AspNetCore.Authentication.Cookies;
using SV20T1020105.DomainModels;
using SV20T1020105.DataLayers.SQLServer;

namespace SV20T1020105.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username = "", string password = "")
        {
            //tra lai username ve giao dien loagin khi nhap sai
            ViewBag.Username = username;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("Error", "Tên, mật khẩu không được để trống");
                return View();
            }

            var userAccount = UserAccountService.Authorize(username, password);
            if (userAccount == null)
            {
				ModelState.AddModelError("Error", "Đăng nhập thất bại");
				return View();
			}

            ///Dang nhap thanh cong, tao du lieu de luu thong tin dang nhap
            var userData = new WebUserData()
            {
                UserId = userAccount.UserID,
                UserName = userAccount.UserName,
                DisplayName = userAccount.FullName,
                Email = userAccount.Email,
                Photo = userAccount.Photo,
                ClientIP = HttpContext.Connection.RemoteIpAddress?.ToString(),
                SessionId = HttpContext.Session.Id,
                AdditionalData = "",
                Roles = userAccount.RoleNames.Split(',').ToList(),

            };
            //Thiet lap phien dang nhap cho tai khoan
            await HttpContext.SignInAsync(userData.CreatePrincipal());

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        public IActionResult ChangePassword(string username)
        {
            ViewBag.username = username;
            return View();
        }

        [HttpPost]
        public IActionResult SavePassword(string username, string oldPassword, string newPassword, string newPassword1)
        {
            ViewBag.username = username;
            try
            {
                if (string.IsNullOrWhiteSpace(oldPassword))
                {
                    ModelState.AddModelError("oldPassword", "Mật khẩu cũ không được để trống");
                }
                if (string.IsNullOrWhiteSpace(newPassword))
                {
                    ModelState.AddModelError("newPassword", "Mật khẩu mới không được để trống");
                }
                if (string.IsNullOrWhiteSpace(newPassword1))
                {
                    ModelState.AddModelError("newPassword1", "Mật khẩu nhập lại không được để trống");
                }

                var userAccount = UserAccountService.Authorize(username, oldPassword);
                if (userAccount == null)
                {
                    ModelState.AddModelError("oldPassword", "Mật khẩu cũ không chính xác!");
                }
                if (newPassword != null && newPassword.Length != 6)
                {
                    ModelState.AddModelError("newPassword", "Mật khẩu mới phải đúng 6 ký tự!");
                }
                if (newPassword1 != newPassword)
                {
                    ModelState.AddModelError("newPassword1", "Mật khẩu nhập lại không chính xác!");
                }
                if (oldPassword == newPassword)
                {
                    ModelState.AddModelError("newPassword", "Mật khẩu mới trùng với mật khẩu cũ!");
                }
                if (!ModelState.IsValid)
                {
                    ViewBag.oldPassword = oldPassword;
                    ViewBag.newPassword = newPassword;
                    ViewBag.newPassword1 = newPassword1;

                    return View("ChangePassword");
                }
                bool check = UserAccountService.ChangePassword(username, oldPassword, newPassword);
                if (!check)
                {
                    ModelState.AddModelError("success", "Đổi mật khẩu thành công!");
                    return View("ChangePassword");
                }
                else
                {
                    ModelState.AddModelError("error", "Không thể đổi mật khẩu!");
                    return View("ChangePassword");
                }
        
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", "Đã xảy ra lỗi khi đổi mật khẩu.");
                return View("ChangePassword");
            }
        }

        public IActionResult AccessDenined()
        {
            ModelState.AddModelError("Error", "Tài khoản của bạn không có quyền truy cập vào chức năng quản lý nhân viên!");
            return View();
        }
        /*public IActionResult ChangePassword(int id = 0)
        {
            ViewBag.Title = "Cập nhật mật khẩu người dùng";
            UserAccount model = UserAccountService.GetUser(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }*/
    }
}
