using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedMaterialCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MaterialCategories",
                columns: new[] { "MaterialCategoryID", "MaterialCategoryDescription" },
                values: new object[,]
                {
                    { 1, "Reagents" },
                    { 2, "Plastics" },
                    { 3, "Equipment" },
                    { 4, "Buffers" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MaterialCategories",
                keyColumn: "MaterialCategoryID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MaterialCategories",
                keyColumn: "MaterialCategoryID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MaterialCategories",
                keyColumn: "MaterialCategoryID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MaterialCategories",
                keyColumn: "MaterialCategoryID",
                keyValue: 4);
        }
    }
}
