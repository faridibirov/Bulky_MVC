using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Globalization;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers;


[Area("Customer")]
[Authorize]
public class CartController : Controller
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IEmailSender _emailSender;
    private readonly BankPaymentService _bankPaymentService;


    [BindProperty]
	public ShoppingCartVM ShoppingCartVM { get; set; }

	public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender, BankPaymentService bankPaymentService)
	{
		_unitOfWork = unitOfWork;
		_emailSender = emailSender;
        _bankPaymentService = bankPaymentService;
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
		var claimsIdentity = (ClaimsIdentity)User.Identity;
		var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

		ShoppingCartVM = new()
		{
			ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
			includeProperties: "Product"),
			OrderHeader = new()
		};

		IEnumerable<ProductImage> productImages = _unitOfWork.ProductImage.GetAll();

		foreach (var cart in ShoppingCartVM.ShoppingCartList)
		{
			cart.Product.ProductImages = productImages.Where(u => u.ProductId == cart.Product.Id).ToList();
			cart.Price = GetPriceBasedOnQuantity(cart);
			ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
		}


		return View(ShoppingCartVM);

	}

	public IActionResult Summary()
	{
		var claimsIdentity = (ClaimsIdentity)User.Identity;
		var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

		ShoppingCartVM = new()
		{
			ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
			includeProperties: "Product"),
			OrderHeader = new()
		};

		ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

		ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
		ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
		ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
		ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
		ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
		ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

		foreach (var cart in ShoppingCartVM.ShoppingCartList)
		{
			cart.Price = GetPriceBasedOnQuantity(cart);
			ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
		}


		return View(ShoppingCartVM);
	}

    [HttpPost]
    [ActionName("Summary")]
    public async Task<IActionResult> SummaryPOST()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product");

        ShoppingCartVM.OrderHeader.ApplicationUserId = userId;
        ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;

        ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

        foreach (var cart in ShoppingCartVM.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }

        if (applicationUser.CompanyId.GetValueOrDefault() == 0)
        {
            ShoppingCartVM.OrderHeader.PaymentStatusEN = SD.PaymentStatusPendingEN;
            ShoppingCartVM.OrderHeader.PaymentStatusRU = SD.PaymentStatusPendingRU;
            ShoppingCartVM.OrderHeader.OrderStatusEN = SD.StatusPendingEN;
            ShoppingCartVM.OrderHeader.OrderStatusRU = SD.StatusPendingRU;
        }
        else
        {
            ShoppingCartVM.OrderHeader.PaymentStatusEN = SD.PaymentStatusDelayedPaymentEN;
            ShoppingCartVM.OrderHeader.PaymentStatusRU = SD.PaymentStatusDelayedPaymentRU;
            ShoppingCartVM.OrderHeader.OrderStatusEN = SD.StatusApprovedEN;
            ShoppingCartVM.OrderHeader.OrderStatusRU = SD.StatusApprovedRU;
        }

        _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
        _unitOfWork.Save();

        foreach (var cart in ShoppingCartVM.ShoppingCartList)
        {
            OrderDetail orderDetail = new()
            {
                ProductId = cart.ProductId,
                OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                Price = cart.Price,
                Count = cart.Count
            };

            _unitOfWork.OrderDetail.Add(orderDetail);
            _unitOfWork.Save();
        }

        if (applicationUser.CompanyId.GetValueOrDefault() == 0)
        {
            var domain = $"{Request.Scheme}://{Request.Host}/";
            var paymentResult = await _bankPaymentService.ProcessPayment(ShoppingCartVM.OrderHeader, domain);

            if (paymentResult.IsSuccess)
            {
                return Redirect(paymentResult.RedirectUrl);
            }
            //else
            //{
            //    ModelState.AddModelError(string.Empty, paymentResult.ErrorMessage);
            //    return View(ShoppingCartVM);
            //}
        }

        return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.OrderHeader.Id });
    }


public IActionResult OrderConfirmation(int id)
    {
        OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");

        if (orderHeader.PaymentStatusEN != SD.PaymentStatusDelayedPaymentEN)
        {
            // Verify payment status with the bank if necessary.
        }

        _emailSender.SendEmailAsync(orderHeader.ApplicationUser.Email, "New Order - Bulky Book",
            $"<p>New Order Created - {orderHeader.Id}</p>");

        List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart
            .GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

        _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
        _unitOfWork.Save();

        return View(id);
    }


    public IActionResult Plus(int cartId)
	{
		var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

		cartFromDb.Count++;
		_unitOfWork.ShoppingCart.Update(cartFromDb);
		_unitOfWork.Save();

		return RedirectToAction(nameof(Index));
	}

	public IActionResult Minus(int cartId)
	{
		var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId, tracked: true);
		if (cartFromDb.Count <= 1)
		{
            HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart
				.GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);
            _unitOfWork.ShoppingCart.Remove(cartFromDb);
		}
		else
		{
			cartFromDb.Count--;
			_unitOfWork.ShoppingCart.Update(cartFromDb);

		}
		_unitOfWork.Save();
		return RedirectToAction(nameof(Index));
	}

	public IActionResult Remove(int cartId)
	{
		var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId, tracked:true);
        HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart
			.GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);
        _unitOfWork.ShoppingCart.Remove(cartFromDb);
		_unitOfWork.Save();
		return RedirectToAction(nameof(Index));
	}

	private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
	{
		if (shoppingCart.Count < 50)
		{
			return shoppingCart.Product.Price;
		}
		else
		{
			if (shoppingCart.Count <= 100)
			{
				return shoppingCart.Product.Price50;
			}
			else
				return shoppingCart.Product.Price100;
		}
	}
}
