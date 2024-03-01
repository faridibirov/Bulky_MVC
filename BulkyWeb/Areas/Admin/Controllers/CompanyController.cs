using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class CompanyController: Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CompanyController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();

        return View(objCompanyList);
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


	public IActionResult Upsert(int? id)
	{

		if (id == null || id == 0)
		{
			//create
			return View(new Company());
		}
		else
		{
			//update
			Company companyObj = _unitOfWork.Company.Get(u => u.Id == id);
			return View(companyObj);

		}
	}


	[HttpPost]
    public IActionResult Upsert(Company CompanyObj)
    {

        if (ModelState.IsValid)
        {
           
            if (CompanyObj.Id == 0)
            {
                _unitOfWork.Company.Add(CompanyObj);

            }

            else
            {
                _unitOfWork.Company.Update(CompanyObj);

            }

            _unitOfWork.Save();
			TempData["success"] = GetCurrentCulture() == "en" ? "Company created successfully" : "Компания успешно создана";

			return RedirectToAction("Index");
        }
        else
        {
            return View(CompanyObj);
        }
    }



    #region API CALLS

    [HttpGet]
    public IActionResult GetAll()
    {
        List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();

        return Json(new { data = objCompanyList });
    }

	[HttpDelete]
	public IActionResult Delete(int? id)
	{
		var companyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);

		if (companyToBeDeleted == null)
		{
			return Json(new { success = false, message = GetCurrentCulture() == "en" ? "Error while deleting" : "Ошибка при удалении"
		});
		}


		_unitOfWork.Company.Remove(companyToBeDeleted);
		_unitOfWork.Save();

		List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();

		return Json(new { success = true, message = GetCurrentCulture() == "en" ? "Delete Successful" : "Успешно Удалено" });
	}


	#endregion

}
