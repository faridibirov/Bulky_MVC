using Bulky.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{

	}

	public DbSet<Category> Categories { get; set; }
	public DbSet<Product> Products { get; set; }
	public DbSet<Company> Companies { get; set; }
	public DbSet<ApplicationUser> ApplicationUsers { get; set; }
	public DbSet<ShoppingCart>  ShoppingCarts { get; set; }
	public DbSet<ProductImage> ProductImages { get; set; }
	public DbSet<OrderHeader> OrderHeaders { get; set; }
	public DbSet<OrderDetail> OrderDetails { get; set; }
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Category>().HasData(
			new Category { Id = 1, NameEN = "Action", NameRU = "Боевик", DisplayOrder = 1 },
			new Category { Id = 2, NameEN = "SciFi", NameRU = "Научная Фантастика", DisplayOrder = 2 },
			new Category { Id = 3, NameEN = "History", NameRU = "История", DisplayOrder = 3 });

		modelBuilder.Entity<Product>().HasData(
		  new Product
		  {
			  Id = 1,
			  TitleEN = "Fortune of Time",
			  AuthorEN = "Billy Spark",
			  DescriptionEN = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
			  TitleRU = "Удача Времени",
			  AuthorRU = "Билли Спарк",
			  DescriptionRU = "Значимость этих проблем настолько очевидна, что реализация намеченных плановых заданий в значительной степени обуславливает создание соответствующий условий активизации. ",
			  ISBN = "SWD9999001",
			  ListPrice = 99,
			  Price = 90,
			  Price50 = 85,
			  Price100 = 80,
			  CategoryId = 1
		  },
				new Product
				{
					Id = 2,
					TitleEN = "Dark Skies",
					AuthorEN = "Nancy Hoover",
					DescriptionEN = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					TitleRU = "Темные Небеса",
					AuthorRU = "Нэнси Гувер",
					DescriptionRU = "Значимость этих проблем настолько очевидна, что реализация намеченных плановых заданий в значительной степени обуславливает создание соответствующий условий активизации. ",
					ISBN = "CAW777777701",
					ListPrice = 40,
					Price = 30,
					Price50 = 25,
					Price100 = 20,
					CategoryId = 1
				},
				new Product
				{
					Id = 3,
					TitleEN = "Vanish in the Sunset",
					AuthorEN = "Julian Button",
					DescriptionEN = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					TitleRU = "Исчезнуть на Накате",
					AuthorRU = "Джулиан Баттон",
					DescriptionRU = "Значимость этих проблем настолько очевидна, что реализация намеченных плановых заданий в значительной степени обуславливает создание соответствующий условий активизации. ",
					ISBN = "RITO5555501",
					ListPrice = 55,
					Price = 50,
					Price50 = 40,
					Price100 = 35,
					CategoryId = 1
				},
				new Product
				{
					Id = 4,
					TitleEN = "Cotton Candy",
					AuthorEN = "Abby Muscles",
					DescriptionEN = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					TitleRU = "Сахарная Вата",
					AuthorRU = "Эбби Маслс",
					DescriptionRU = "Значимость этих проблем настолько очевидна, что реализация намеченных плановых заданий в значительной степени обуславливает создание соответствующий условий активизации. ",
					ISBN = "WS3333333301",
					ListPrice = 70,
					Price = 65,
					Price50 = 60,
					Price100 = 55,
					CategoryId = 2
				},
				new Product
				{
					Id = 5,
					TitleEN = "Rock in the Ocean",
					AuthorEN = "Ron Parker",
					DescriptionEN = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					TitleRU = "Скала в Океане",
					AuthorRU = "Рон Паркер",
					DescriptionRU = "Значимость этих проблем настолько очевидна, что реализация намеченных плановых заданий в значительной степени обуславливает создание соответствующий условий активизации. ",
					ISBN = "SOTJ1111111101",
					ListPrice = 30,
					Price = 27,
					Price50 = 25,
					Price100 = 20,
					CategoryId = 2
				},
				new Product
				{
					Id = 6,
					TitleEN = "Leaves and Wonders",
					AuthorEN = "Laura Phantom",
					DescriptionEN = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					TitleRU = "Листья и Чудеса",
					AuthorRU = "Лора Фантом",
					DescriptionRU = "Значимость этих проблем настолько очевидна, что реализация намеченных плановых заданий в значительной степени обуславливает создание соответствующий условий активизации. ",
					ISBN = "FOT000000001",
					ListPrice = 25,
					Price = 23,
					Price50 = 22,
					Price100 = 20,
					CategoryId = 3
				});

		modelBuilder.Entity<Company>().HasData(
		  new Company
		  {
			  Id = 1,
			  Name = "Mr. Robot",
			  StreetAddress = "Mirqasimov st. 3",
			  City = "Baku",
			  State = "Azerbaijan",
			  PostalCode = "AZ1007",
			  PhoneNumber = "0554536909"
		  });
	}

}