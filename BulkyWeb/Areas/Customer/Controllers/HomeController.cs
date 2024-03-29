using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    private string GetCurrentCulture()
    {
        return CultureInfo.CurrentCulture.Name;
    }

    public IActionResult Index()
    {
        

        IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,ProductImages");
        return View(productList);
    }

    public IActionResult Details(int Id)
    {
        if (Id == 0)
        {
            return RedirectToAction("Index");
        }

        ShoppingCart shoppingCart = new()
        {
            Product = _unitOfWork.Product.Get(u => u.Id == Id, includeProperties: "Category,ProductImages"),
            Count = 1,
            ProductId = Id
        };

        return View(shoppingCart);
    }

    [HttpPost]
    public IActionResult CultureManagement(string culture, string returnUrl)
    {
        Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) });

        return LocalRedirect(returnUrl);
    }


    [HttpPost]
    [Authorize]
    public IActionResult Details(ShoppingCart shoppingCart)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        shoppingCart.ApplicationUserId = userId;

        ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId &&
        u.ProductId == shoppingCart.ProductId);

        if (cartFromDb != null)
        {
            //shopping cart exists
            cartFromDb.Count += shoppingCart.Count;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();

        }

        else
        {
            //add cart record
            _unitOfWork.ShoppingCart.Add(shoppingCart);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.SessionCart,
             _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
        }

        TempData["success"] = GetCurrentCulture() == "en" ? "Cart updated successfully" : "������� ������� ���������";


        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
