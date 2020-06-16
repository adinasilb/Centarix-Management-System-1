using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class seedProprietyProductSubcategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductSubcategories",
                columns: new[] { "ProductSubcategoryID", "ParentCategoryID", "ProductSubcategoryDescription" },
                values: new object[,]
                {
                    { 301, 3, "Virus" },
                    { 302, 3, "Plasmid" },
                    { 303, 3, "Primers" },
                    { 304, 3, "Probes" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 301);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 302);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 303);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 304);
        }
    }
}
