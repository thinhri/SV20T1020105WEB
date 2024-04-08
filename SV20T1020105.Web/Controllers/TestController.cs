using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace SV20T1020105.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Create()
        {
            var model = new Models.Person()
            {
                Name = "Dat oc cho",
                BirthDate = DateTime.Parse("2023-01-05"),
                Salary = 10.25m
            };
            return View(model);
        }

        public IActionResult Save(Models.Person model, string birthDateInput = "")
        {
            //chuyen birthdateinput sang gia tri kieu ngay
            DateTime? dValue = StringToDateTime(birthDateInput);
            if (dValue.HasValue)
            {
                model.BirthDate = dValue.Value;
            }
            return Json(model);
        }

        private DateTime? StringToDateTime(string s, string formats = "d/M/yyyy;d-M-yyyy;d.M.yyyy")
        {
            try
            {
                return DateTime.ParseExact(s, formats.Split(';'), CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }
    }
}
