using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class FillUnitsParentCategoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UnitTypeParentCategory",
                columns: new[] { "UnitTypeID", "ParentCategoryID" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 22, 2 },
                    { 23, 2 },
                    { 5, 3 },
                    { 1, 4 },
                    { 2, 4 },
                    { 19, 4 },
                    { 3, 4 },
                    { 5, 4 },
                    { 5, 7 },
                    { 10, 7 },
                    { 9, 7 },
                    { 20, 7 },
                    { 21, 7 },
                    { 22, 7 },
                    { 13, 7 },
                    { 21, 2 },
                    { 20, 2 },
                    { 13, 2 },
                    { 12, 2 },
                    { 2, 1 },
                    { 19, 1 },
                    { 3, 1 },
                    { 5, 1 },
                    { 17, 2 },
                    { 18, 2 },
                    { 1, 2 },
                    { 12, 7 },
                    { 2, 2 },
                    { 3, 2 },
                    { 5, 2 },
                    { 7, 2 },
                    { 8, 2 },
                    { 9, 2 },
                    { 10, 2 },
                    { 11, 2 },
                    { 19, 2 },
                    { 11, 7 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 3, 4 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 5, 4 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 5, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 7, 2 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 8, 2 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 9, 2 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 9, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 10, 2 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 10, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 11, 2 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 11, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 12, 2 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 12, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 13, 2 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 13, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 17, 2 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 18, 2 });

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
                keyValues: new object[] { 20, 2 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 20, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 21, 2 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 21, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 22, 2 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 22, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 23, 2 });
        }
    }
}
