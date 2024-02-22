using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class UserController : Controller
{
	private readonly IUnitOfWork _unitOfWork;

	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly UserManager<IdentityUser> _userManager;

	public UserController(IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
	{
		_unitOfWork = unitOfWork;
		_roleManager = roleManager;
		_userManager = userManager;
	}

	public IActionResult Index()
	{
		return View();
	}


	public IActionResult RoleManagement(string id)
	{
		RoleManagementVM roleVM = new()
		{
			ApplicationUser = _unitOfWork.ApplicationUser.Get(u=>u.Id==id, includeProperties: "Company"),

			RoleList = _roleManager.Roles.Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Name
			}),

			CompanyList = _unitOfWork.Company.GetAll().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString()
			}),
		};

		roleVM.ApplicationUser.Role = _userManager.GetRolesAsync(_unitOfWork.ApplicationUser.Get(u => u.Id == id))
			.GetAwaiter().GetResult().FirstOrDefault();


		return View(roleVM);
	}

	[HttpPost]
	public IActionResult RoleManagement(RoleManagementVM RoleVM)
	{
		string oldRole = _userManager.GetRolesAsync(_unitOfWork.ApplicationUser.Get(u => u.Id == RoleVM.ApplicationUser.Id))
			.GetAwaiter().GetResult().FirstOrDefault();


			ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == RoleVM.ApplicationUser.Id);

		if (!(RoleVM.ApplicationUser.Role == oldRole))
		{
			// a role was updated
			if(RoleVM.ApplicationUser.Role==SD.Role_Company)
			{
				applicationUser.CompanyId = RoleVM.ApplicationUser.CompanyId;
			}
			if(oldRole==SD.Role_Company)
			{
				applicationUser.CompanyId = null;
			}
			_unitOfWork.ApplicationUser.Update(applicationUser);
			_unitOfWork.Save();

			_userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
			_userManager.AddToRoleAsync(applicationUser, RoleVM.ApplicationUser.Role).GetAwaiter().GetResult();
		}

		else
		{
			if(oldRole==SD.Role_Company && applicationUser.CompanyId != RoleVM.ApplicationUser.CompanyId)
			{
				applicationUser.CompanyId = RoleVM.ApplicationUser.CompanyId;
				_unitOfWork.ApplicationUser.Update(applicationUser);
				_unitOfWork.Save();
			}
		}

		return RedirectToAction("Index");
	}

	[HttpPost]
	public IActionResult LockUnlock([FromBody] string id)
	{
		var objFromDb = _unitOfWork.ApplicationUser.Get(u => u.Id == id);

		if (objFromDb == null)
		{
			return Json(new { success = false, message = "Error while Locking/Unlocking" });

		}

		if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
		{
			//user is currently locked and we need to unlock them
			objFromDb.LockoutEnd = DateTime.Now;
		}

		else
		{
			objFromDb.LockoutEnd = DateTime.Now.AddYears(100);
		}

		_unitOfWork.ApplicationUser.Update(objFromDb);
		_unitOfWork.Save();

		return Json(new { success = true, message = "Operation Successful" });
	}



	#region API CALLS

	[HttpGet]
	public IActionResult GetAll()
	{
		List<ApplicationUser> objUserList = _unitOfWork.ApplicationUser.GetAll(includeProperties: "Company").ToList();

		foreach (var user in objUserList)
		{
			user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();

			if (user.Company == null)
			{
				user.Company = new() { Name = "" };
			}
		}

		return Json(new { data = objUserList });
	}

	#endregion

}
