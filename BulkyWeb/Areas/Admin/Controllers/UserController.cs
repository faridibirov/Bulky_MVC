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
	private readonly ApplicationDbContext _db;

	private readonly UserManager<IdentityUser> _userManager;

	public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
	{
		_db = db;
		_userManager = userManager;
	}

	public IActionResult Index()
	{
		return View();
	}



	#region API CALLS

	[HttpGet]
	public IActionResult GetAll()
	{
		List<ApplicationUser> objUserList = _db.ApplicationUsers.Include(u => u.Company).ToList();

		var userRoles = _db.UserRoles.ToList();
		var roles = _db.Roles.ToList();

		foreach (var user in objUserList)
		{
			var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
			user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;

			if (user.Company == null)
			{
				user.Company = new() { Name = "" };
			}
		}

		return Json(new { data = objUserList });
	}
	public IActionResult RoleManagement(string id)
	{
		string RoleId = _db.UserRoles.FirstOrDefault(u => u.UserId == id).RoleId;

		RoleManagementVM roleVM = new()
		{
			ApplicationUser = _db.ApplicationUsers.Include(u => u.Company).FirstOrDefault(u => u.Id == id),
			RoleList = _db.Roles.Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Name
			}),
			CompanyList = _db.Companies.Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString()
			}),
		};

		roleVM.ApplicationUser.Role = _db.Roles.FirstOrDefault(u => u.Id == RoleId).Name;


		return View(roleVM);
	}

	[HttpPost]
	public IActionResult RoleManagement(RoleManagementVM RoleVM)
	{
		string RoleId = _db.UserRoles.FirstOrDefault(u => u.UserId == RoleVM.ApplicationUser.Id).RoleId;
		string oldRole = _db.Roles.FirstOrDefault(u=>u.Id==RoleId).Name;

		if (!(RoleVM.ApplicationUser.Role == oldRole))
		{
			// a role was updated
			ApplicationUser applicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.Id == RoleVM.ApplicationUser.Id);
			if(RoleVM.ApplicationUser.Role==SD.Role_Company)
			{
				applicationUser.CompanyId = RoleVM.ApplicationUser.CompanyId;
			}
			if(oldRole==SD.Role_Company)
			{
				applicationUser.CompanyId = null;
			}
			_db.SaveChanges();

			_userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
			_userManager.AddToRoleAsync(applicationUser, RoleVM.ApplicationUser.Role).GetAwaiter().GetResult();
		}

		return RedirectToAction("Index");
	}

	[HttpPost]
	public IActionResult LockUnlock([FromBody] string id)
	{
		var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);

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

		_db.SaveChanges();

		return Json(new { success = true, message = "Operation Successful" });
	}


	#endregion

}
