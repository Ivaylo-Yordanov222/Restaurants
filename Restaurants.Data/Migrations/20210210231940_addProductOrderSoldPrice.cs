using Microsoft.EntityFrameworkCore.Migrations;

namespace Restaurants.Data.Migrations
{
    public partial class addProductOrderSoldPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PromotionalPrice",
                table: "ProductsOrders",
                newName: "SoldPrice");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SoldPrice",
                table: "ProductsOrders",
                newName: "PromotionalPrice");
        }
    }
}
