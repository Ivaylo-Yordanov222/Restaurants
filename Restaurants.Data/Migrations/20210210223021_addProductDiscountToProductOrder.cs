using Microsoft.EntityFrameworkCore.Migrations;

namespace Restaurants.Data.Migrations
{
    public partial class addProductDiscountToProductOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ProductDiscount",
                table: "ProductsOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductDiscount",
                table: "ProductsOrders");
        }
    }
}
