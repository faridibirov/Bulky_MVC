using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bulky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ProductLanguageAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Products",
                newName: "TitleRU");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Products",
                newName: "TitleEN");

            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Products",
                newName: "DescriptionRU");

            migrationBuilder.AddColumn<string>(
                name: "AuthorEN",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AuthorRU",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEN",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AuthorEN", "AuthorRU", "DescriptionEN", "DescriptionRU", "TitleEN", "TitleRU" },
                values: new object[] { "Billy Spark", "Билли Спарк", "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "Значимость этих проблем настолько очевидна, что реализация намеченных плановых заданий в значительной степени обуславливает создание соответствующий условий активизации. ", "Fortune of Time", "Удача Времени" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AuthorEN", "AuthorRU", "DescriptionEN", "DescriptionRU", "TitleEN", "TitleRU" },
                values: new object[] { "Nancy Hoover", "Нэнси Гувер", "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "Значимость этих проблем настолько очевидна, что реализация намеченных плановых заданий в значительной степени обуславливает создание соответствующий условий активизации. ", "Dark Skies", "Темные Небеса" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AuthorEN", "AuthorRU", "DescriptionEN", "DescriptionRU", "TitleEN", "TitleRU" },
                values: new object[] { "Julian Button", "Джулиан Баттон", "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "Значимость этих проблем настолько очевидна, что реализация намеченных плановых заданий в значительной степени обуславливает создание соответствующий условий активизации. ", "Vanish in the Sunset", "Исчезнуть на Накате" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AuthorEN", "AuthorRU", "DescriptionEN", "DescriptionRU", "TitleEN", "TitleRU" },
                values: new object[] { "Abby Muscles", "Эбби Маслс", "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "Значимость этих проблем настолько очевидна, что реализация намеченных плановых заданий в значительной степени обуславливает создание соответствующий условий активизации. ", "Cotton Candy", "Сахарная Вата" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AuthorEN", "AuthorRU", "DescriptionEN", "DescriptionRU", "TitleEN", "TitleRU" },
                values: new object[] { "Ron Parker", "Рон Паркер", "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "Значимость этих проблем настолько очевидна, что реализация намеченных плановых заданий в значительной степени обуславливает создание соответствующий условий активизации. ", "Rock in the Ocean", "Скала в Океане" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AuthorEN", "AuthorRU", "DescriptionEN", "DescriptionRU", "TitleEN", "TitleRU" },
                values: new object[] { "Laura Phantom", "Лора Фантом", "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "Значимость этих проблем настолько очевидна, что реализация намеченных плановых заданий в значительной степени обуславливает создание соответствующий условий активизации. ", "Leaves and Wonders", "Листья и Чудеса" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorEN",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AuthorRU",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DescriptionEN",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "TitleRU",
                table: "Products",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "TitleEN",
                table: "Products",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "DescriptionRU",
                table: "Products",
                newName: "Author");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Author", "Description", "Title" },
                values: new object[] { "Billy Spark", "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "Fortune of Time" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Author", "Description", "Title" },
                values: new object[] { "Nancy Hoover", "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "Dark Skies" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Author", "Description", "Title" },
                values: new object[] { "Julian Button", "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "Vanish in the Sunset" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Author", "Description", "Title" },
                values: new object[] { "Abby Muscles", "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "Cotton Candy" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Author", "Description", "Title" },
                values: new object[] { "Ron Parker", "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "Rock in the Ocean" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Author", "Description", "Title" },
                values: new object[] { "Laura Phantom", "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "Leaves and Wonders" });
        }
    }
}
