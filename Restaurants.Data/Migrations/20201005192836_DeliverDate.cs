using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Restaurants.Data.Migrations
{
    public partial class DeliverDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeliverTime",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliverTime",
                table: "Orders");
        }
    }
}
