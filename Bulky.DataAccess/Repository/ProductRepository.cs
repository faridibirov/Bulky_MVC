using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
	private readonly ApplicationDbContext _db;

	public ProductRepository(ApplicationDbContext db) : base(db)
	{
		_db = db;
	}


	public void Update(Product obj)
	{
		var objFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
		if (objFromDb != null)
		{
			objFromDb.TitleEN = obj.TitleEN;
			objFromDb.TitleRU = obj.TitleRU;
			objFromDb.ISBN = obj.ISBN;
			objFromDb.Price = obj.Price;
			objFromDb.Price50 = obj.Price50;
			objFromDb.ListPrice = obj.ListPrice;
			objFromDb.Price100 = obj.Price100;
			objFromDb.DescriptionEN = obj.DescriptionEN;
			objFromDb.DescriptionRU = obj.DescriptionRU;
			objFromDb.CategoryId = obj.CategoryId;
			objFromDb.AuthorEN = obj.AuthorEN;
			objFromDb.AuthorRU = obj.AuthorRU;
			objFromDb.ProductImages = obj.ProductImages;
		}
	}
}
