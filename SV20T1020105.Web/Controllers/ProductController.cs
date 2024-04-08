using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020105.BusinessLayers;
using SV20T1020105.DomainModels;
using SV20T1020105.Web.AppCodes;
using SV20T1020105.Web.Models;

namespace SV20T1020105.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 15;
		private const string PRODUCT_SEARCH = "Product_search";
		public IActionResult Index()
		{
			ProductSearchInput input = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH);
			if (input == null)
			{
				input = new ProductSearchInput()
				{
					Page = 1,
					PageSize = PAGE_SIZE,
					SearchValue = "",
					CategoryID = 0,
					SupplierID = 0,
				};
			}
			return View(input);
		}

		public IActionResult Search(ProductSearchInput input)
		{
			int rowCount = 0;

			var data = ProductDataService.ListProducts(out rowCount, input.Page, input.PageSize,
			     input.SearchValue ?? "", input.CategoryID, input.SupplierID
			     );
			var model = new ProductSearchResult()
			{
				Data = data,
				Page = input.Page,
				PageSize = input.PageSize,
				SearchValue = input.SearchValue ?? "",
				RowCount = rowCount,
				Categories = CommonDataService.ListOfCategories(out rowCount, 1, input.PageSize, ""),
				Suppliers = CommonDataService.ListOfSuppliers(out rowCount, 1, input.PageSize, "")
			};
			ApplicationContext.SetSessionData(PRODUCT_SEARCH, input);
			return View(model);
		}

		public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung mặt hàng";
            ViewBag.IsEdit = false; // viết tạm để làm giao diện
            Product model = new Product()
            {
                ProductID = 0,
                Photo = "nophotos.png"
            };
            return View("Edit", model);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin mặt hàng";
            ViewBag.IsEdit = true;
            Product model = ProductDataService.GetProduct(id);
            if (model == null)
                return RedirectToAction("Index");

            if (string.IsNullOrEmpty(model.Photo))
                model.Photo = "nophotos.png";

            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Product data, IFormFile? uploadPhoto)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(data.ProductName))
                {
                    ModelState.AddModelError(nameof(data.ProductName), "Tên không được để trống!");
                }
                if (string.IsNullOrWhiteSpace(data.Unit))
                {
                    ModelState.AddModelError(nameof(data.Unit), "Đơn vị tính không được để trống!");
                }
                if (data.Price == 0)
                {
                    ModelState.AddModelError(nameof(data.Price), "Giá hàng không được bằng 0!");
                }
                if (data.CategoryID == 0)
                {
                    ModelState.AddModelError(nameof(data.CategoryID), "Loại hàng không được để trống!");
                }
                if (data.SupplierID == 0)
                {
                    ModelState.AddModelError(nameof(data.SupplierID), "Nhà cung cấp không được để trống!");
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.Title = data.ProductID == 0 ? "Bổ sung mặt hàng" : "Cập nhật mặt hàng";
                    ViewBag.IsEdit = false;
                    return View("Edit", data);
                }

                if (uploadPhoto != null)
                {
                    //tránh việc trùng tên file nên thêm time trước tên
                    string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                    string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath,@"images\products");//đường dẫn đến thư mục
                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        uploadPhoto.CopyTo(stream);
                    }
                    data.Photo = fileName;
                }
                if (data.ProductID == 0)
                {
                    int id = ProductDataService.AddProduct(data);
                }
                else
                {
                    bool result = ProductDataService.UpdateProduct(data);
                }
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "Không thể lưu được dữ liệu. Vui lòng thử lại trong vài phút!");
                return View("Edit", data);
            }
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool result = ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }

            var model = ProductDataService.GetProduct(id);
            if(model == null)
                return RedirectToAction("Index");

            ViewBag.AllowDelete = !ProductDataService.InUsedProduct(id);
            return View(model);
        }

        public IActionResult Photo(int id, string method, int photoId = 0)
        {
            var model = new ProductPhoto();
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh";
                    model = new ProductPhoto()
                    {
                        PhotoID = 0,
                        ProductID = id,
                        Photo = "nophotos.png"
                    };
                    return View("Photo", model);
                case "edit":
                    ViewBag.Title = "Thay đổi ảnh";
                    model = ProductDataService.GetPhoto(photoId);
                    if (model == null) return RedirectToAction("Index");
                    
                    return View("Photo", model);
                case "delete":
                    ViewBag.IsEdit = true;
                    //xóa ảnh trực tiếp, ko cần hỏi
                    var data = ProductDataService.GetProduct(id);
                    ProductDataService.DeletePhoto(photoId);
                    
                    return View("Edit", data);
                default:
                    return RedirectToAction("Index");
            }
        }

        public IActionResult Attribute(int id, string method, int attributeId = 0)
        {
            var model = new ProductAttribute();
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính";
                    model = new ProductAttribute
                    {
                        AttributeID = 0,
                        ProductID = id,
                    };
                    return View("Attribute", model);
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính";
                    model = ProductDataService.GetAttribute(attributeId);
                    if (model == null)
                        return RedirectToAction("Index");
                    return View("Attribute", model);
                case "delete":
                    //xóa ảnh trực tiếp, ko cần hỏi
                    ViewBag.IsEdit = true;
                    //xóa ảnh trực tiếp, ko cần hỏi
                    var data = ProductDataService.GetProduct(id);
                    ProductDataService.DeleteAttribute(attributeId);
                    return View("Edit", data);
                default:
                    return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult SavePhoto(ProductPhoto data, IFormFile? uploadPhoto)
        {
            if (uploadPhoto != null)
            {
                //tránh việc trùng tên file nên thêm time trước tên
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\products");//đường dẫn đến thư mục
                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }
            try
            {
                if (data.Photo.Equals("nophotos.png"))
                {
                    ModelState.AddModelError(nameof(data.Photo), "Ảnh không được để trống!");
                }
                if (!ModelState.IsValid)
                {
                    ViewBag.Title = data.ProductID == 0 ? "Bổ sung ảnh" : "Cập nhật ảnh";
                    return View("Photo", data);
                }
                if (data.PhotoID == 0)
                {
                    long id = ProductDataService.AddPhoto(data);
					if (id <= 0)
					{
                        ViewBag.Title = data.ProductID == 0 ? "Bổ sung ảnh" : "Cập nhật ảnh";
                        ModelState.AddModelError(nameof(data.DisplayOrder), "Số thứ tự này đã tồn tại!");
						return View("Photo", data);
					}
				}
                else
                {
                    bool result = ProductDataService.UpdatePhoto(data);
					if (!result)
					{
						ModelState.AddModelError(nameof(data.DisplayOrder), "Số thứ tự này đã tồn tại!");
						return View("Photo", data);
					}
				}
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "Không thể lưu được dữ liệu. Vui lòng thử lại trong vài phút!");
                return View("Edit", data);
            }
            return RedirectToAction("Edit", new { id = data.ProductID });
        }

        public IActionResult SaveAttribute(ProductAttribute data)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(data.AttributeName))
                {
                    ModelState.AddModelError(nameof(data.AttributeName), "Tên thuộc tính không được để trống!");
                }
                if (string.IsNullOrWhiteSpace(data.AttributeValue))
                {
                    ModelState.AddModelError(nameof(data.AttributeValue), "Giá trị thuộc tính không được để trống!");
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.Title = data.ProductID == 0 ? "Bổ sung thuộc tính" : "Cập nhật thuộc tính";
                    return View("Attribute", data);
                }
                if (data.AttributeID == 0)
                {
                    long id = ProductDataService.AddAttribute(data);
                    if (id <= 0)
                    {
                        ModelState.AddModelError(nameof(data.DisplayOrder), "Số thứ tự này đã tồn tại!");
                        return View("Attribute", data);
                    }
                }
                else
                {
                    bool result = ProductDataService.UpdateAttribute(data);
                    if (!result)
                    {
                        ModelState.AddModelError(nameof(data.DisplayOrder), "Số thứ tự này đã tồn tại!");
                        return View("Attribute", data);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "Không thể lưu được dữ liệu. Vui lòng thử lại trong vài phút!");
                return View("Edit", data);
            }
            return RedirectToAction("Edit", new { id = data.ProductID });
        }
    }
}
