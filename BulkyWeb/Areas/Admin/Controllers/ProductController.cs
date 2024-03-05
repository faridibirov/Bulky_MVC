using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class ProductController : Controller
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IWebHostEnvironment _webHostEnvironment;

	public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
	{
		_unitOfWork = unitOfWork;
		_webHostEnvironment = webHostEnvironment;
	}

	[HttpPost]
	public IActionResult CultureManagement(string culture, string returnUrl)
	{
		Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
			CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
			new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) });

		return LocalRedirect(returnUrl);
	}

	private string GetCurrentCulture()
	{
		return CultureInfo.CurrentCulture.Name;
	}

	public IActionResult Index()
	{
		List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();

		return View(objProductList);
	}

	public IActionResult Upsert(int? id)
	{
		

		ProductVM productVM = new()
		{
			CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
			{
				
				Text = GetCurrentCulture() == "en" ? u.NameEN : u.NameRU,
				Value = u.Id.ToString()
			}),
			Product = new Product()
		};
		if (id == null || id == 0)
		{
			//create
			return View(productVM);
		}
		else
		{
			//update
			productVM.Product = _unitOfWork.Product.Get(u => u.Id == id, includeProperties:"ProductImages");
			return View(productVM);

		}
	}

	[HttpPost]
	public IActionResult Upsert(ProductVM productVM, List<IFormFile> files)
	{

		if (ModelState.IsValid)
		{
			if (productVM.Product.Id == 0)
			{
				_unitOfWork.Product.Add(productVM.Product);

			}

			else
			{
				_unitOfWork.Product.Update(productVM.Product);

			}

			_unitOfWork.Save();

			string wwwRootPath = _webHostEnvironment.WebRootPath;
			if(files!=null)
			{

				foreach(var file in files)
				{
				string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
				string productPath = @"image\products\product-" + productVM.Product.Id;
				string finalPath = Path.Combine(wwwRootPath, productPath);

					if(!Directory.Exists(finalPath))
					{
						Directory.CreateDirectory(finalPath);
					}

					using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
					{
						file.CopyTo(fileStream);
					}

					ProductImage productImage = new()
					{
						ImageUrl = @"\" + productPath + @"\" + fileName,
						ProductId=productVM.Product.Id,
					};

					if(productVM.Product.ProductImages==null)
					{
						productVM.Product.ProductImages = new List<ProductImage>();
					}

					productVM.Product.ProductImages.Add(productImage);
				}

				_unitOfWork.Product.Update(productVM.Product);
				_unitOfWork.Save();

			}

			
			TempData["success"] = GetCurrentCulture() == "en" ? "Product created/updated successfully" : "Продукт успешно добавлен/обновлен";
			return RedirectToAction("Index");
		}
		else
		{

			productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
			{
				Text = GetCurrentCulture() == "en" ? u.NameEN : u.NameRU,
				Value = u.Id.ToString()
			});
			return View(productVM);
		}
	}

	public IActionResult DeleteImage(int imageId)
	{
		var imageTobeDeleted = _unitOfWork.ProductImage.Get(u=>u.Id == imageId);
		if(imageTobeDeleted != null)
		{
			if(!string.IsNullOrEmpty(imageTobeDeleted.ImageUrl))
			{
				var oldImagePath =
						Path.Combine(_webHostEnvironment.WebRootPath,
						imageTobeDeleted.ImageUrl.TrimStart('\\'));

				if (System.IO.File.Exists(oldImagePath))
				{
					System.IO.File.Delete(oldImagePath);
				}
			}

			_unitOfWork.ProductImage.Remove(imageTobeDeleted);
			_unitOfWork.Save();

			TempData["success"] = GetCurrentCulture() == "en" ? "Deleted successfully" : "Успешно удалено" ;

		}

		return RedirectToAction(nameof(Upsert), new { id = imageTobeDeleted.ProductId });
	}

	

	#region API CALLS

	[HttpGet]
	public IActionResult GetAll()
	{
        List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

		

		var localizedProductList = objProductList.Select(p => new
		{
			title = GetCurrentCulture() == "en" ? p.TitleEN : p.TitleRU,
			isbn = p.ISBN,
			listPrice = p.ListPrice,
			author = GetCurrentCulture() == "en" ? p.AuthorEN : p.AuthorRU,
			categoryName = GetCurrentCulture() == "en" ? p.Category.NameEN : p.Category.NameRU,
			id = p.Id
		}).ToList();


		return Json(new { data = localizedProductList });
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var	productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);

		if(productToBeDeleted==null)
		{
            return Json(new { success=false, message= GetCurrentCulture() == "en" ? "Error while deleting" : "Ошибка при удалении" });
        }

		string productPath = @"image\products\product-" + id;
		string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);

		if (Directory.Exists(finalPath))
		{
			string[] filePaths = Directory.GetFiles(finalPath);

			foreach(string filePath in filePaths)
			{
				System.IO.File.Delete(filePath);
			}

			Directory.Delete(finalPath);
		}

	

		_unitOfWork.Product.Remove(productToBeDeleted);
		_unitOfWork.Save();

        List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

        return Json(new { success = true, message = GetCurrentCulture() == "en" ? "Delete Successful" : "Успешно удалено" });
    }

    #endregion

}

