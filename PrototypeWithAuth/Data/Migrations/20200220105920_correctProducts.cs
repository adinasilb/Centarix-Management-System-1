using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class correctProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Handeling",
                table: "Products",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LocationID",
                table: "Products",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QuantityPerUnit",
                table: "Products",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ReorderLevel",
                table: "Products",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitsInOrder",
                table: "Products",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitsInStock",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Handeling",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LocationID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "QuantityPerUnit",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ReorderLevel",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnitsInOrder",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnitsInStock",
                table: "Products");
        }
    }
}
