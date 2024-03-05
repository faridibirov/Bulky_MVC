using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bulky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class OrderHeaderLanguageAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "OrderHeaders",
                newName: "PaymentStatusRU");

            migrationBuilder.RenameColumn(
                name: "OrderStatus",
                table: "OrderHeaders",
                newName: "PaymentStatusEN");

            migrationBuilder.AddColumn<string>(
                name: "OrderStatusEN",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderStatusRU",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderStatusEN",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "OrderStatusRU",
                table: "OrderHeaders");

            migrationBuilder.RenameColumn(
                name: "PaymentStatusRU",
                table: "OrderHeaders",
                newName: "PaymentStatus");

            migrationBuilder.RenameColumn(
                name: "PaymentStatusEN",
                table: "OrderHeaders",
                newName: "OrderStatus");
        }
    }
}
