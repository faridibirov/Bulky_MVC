﻿using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _db;

    public CategoryController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        List<Category> objCategoryList = _db.Categories.ToList();

        return View(objCategoryList);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
	public IActionResult Create(Category obj)
	{
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name");
        }

        if (ModelState.IsValid)
        {
            _db.Categories.Add(obj);
            _db.SaveChanges();
			return RedirectToAction("Index");
		}
        return View();
	}

	public IActionResult Edit(int? id)
	{
		if(id==null || id==0)
		{
			return NotFound();
		}
		var categoryFromDb = _db.Categories.Find(id);
		if(categoryFromDb==null)
		{
			return NotFound();
		}
		return View(categoryFromDb);
	}

	[HttpPost]
	public IActionResult Edit(Category obj)
	{

		if (ModelState.IsValid)
		{
			_db.Categories.Update(obj);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}
		return View();
	}
}
