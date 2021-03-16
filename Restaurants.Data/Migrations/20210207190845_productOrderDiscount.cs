using Microsoft.EntityFrameworkCore.Migrations;

namespace Restaurants.Data.Migrations
{
    public partial class productOrderDiscount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Discount",
                table: "ProductsOrders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PromotionalPrice",
                table: "ProductsOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "ProductsOrders");

            migrationBuilder.DropColumn(
                name: "PromotionalPrice",
                table: "ProductsOrders");
        }
    }
}
