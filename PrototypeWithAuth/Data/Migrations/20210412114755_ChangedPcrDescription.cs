using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ChangedPcrDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 102,
                column: "ProductSubcategoryDescription",
                value: "PCR");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 102,
                column: "ProductSubcategoryDescription",
                value: "PCR Plates");
        }
    }
}
