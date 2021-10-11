using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class FixSomeDBWordIssues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 19, 1 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 19, 2 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 19, 4 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 19, 5 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 19, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 19, 7 });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 109,
                column: "ProductSubcategoryDescription",
                value: "Q-PCR Plastics");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 202,
                column: "ProductSubcategoryDescription",
                value: "Antibody");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 207,
                column: "ProductSubcategoryDescription",
                value: "Q-PCR Reagents");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 209,
                column: "ProductSubcategoryDescription",
                value: "Primers and Oligos");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 218,
                column: "ProductSubcategoryDescription",
                value: "DNA Enzymes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 109,
                column: "ProductSubcategoryDescription",
                value: "RT-PCR Plastics");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 202,
                column: "ProductSubcategoryDescription",
                value: "Antibodies");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 207,
                column: "ProductSubcategoryDescription",
                value: "RT-PCR Reagents");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 209,
                column: "ProductSubcategoryDescription",
                value: "Primers");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 218,
                column: "ProductSubcategoryDescription",
                value: "DNA Enzyme");

            migrationBuilder.InsertData(
                table: "UnitTypeParentCategory",
                columns: new[] { "UnitTypeID", "ParentCategoryID" },
                values: new object[,]
                {
                    { 19, 1 },
                    { 19, 2 },
                    { 19, 4 },
                    { 19, 5 },
                    { 19, 7 },
                    { 19, 6 }
                });
        }
    }
}
