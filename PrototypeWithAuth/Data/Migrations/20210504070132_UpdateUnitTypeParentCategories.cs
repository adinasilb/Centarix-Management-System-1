using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class UpdateUnitTypeParentCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 9, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 10, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 11, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 12, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 13, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 20, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 21, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 22, 7 });

            migrationBuilder.InsertData(
                table: "UnitTypeParentCategory",
                columns: new[] { "UnitTypeID", "ParentCategoryID" },
                values: new object[,]
                {
                    { 5, 14 },
                    { 5, 6 },
                    { 6, 6 },
                    { 7, 6 },
                    { 8, 6 },
                    { 9, 6 },
                    { 10, 6 },
                    { 11, 6 },
                    { 12, 6 },
                    { 4, 6 },
                    { 13, 6 },
                    { 15, 6 },
                    { 16, 6 },
                    { 17, 6 },
                    { 18, 6 },
                    { 19, 6 },
                    { 20, 6 },
                    { 21, 6 },
                    { 22, 6 },
                    { 14, 6 },
                    { 3, 6 },
                    { 2, 6 },
                    { 1, 6 },
                    { 10, 14 },
                    { 9, 14 },
                    { 20, 14 },
                    { 21, 14 },
                    { 22, 14 },
                    { 13, 14 },
                    { 12, 14 },
                    { 11, 14 },
                    { 1, 5 },
                    { 2, 5 },
                    { 19, 5 },
                    { 3, 5 },
                    { 5, 5 },
                    { 24, 5 },
                    { 1, 7 },
                    { 2, 7 },
                    { 19, 7 },
                    { 3, 7 },
                    { 24, 7 },
                    { 23, 6 },
                    { 24, 6 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 1, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 1, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 2, 5 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 2, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 2, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 3, 5 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 3, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 3, 7 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 4, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 5, 5 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 5, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 5, 14 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 6, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 7, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 8, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 9, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 9, 14 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 10, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 10, 14 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 11, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 11, 14 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 12, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 12, 14 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 13, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 13, 14 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 14, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 15, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 16, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 17, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 18, 6 });

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

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 20, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 20, 14 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 21, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 21, 14 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 22, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 22, 14 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 23, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 24, 5 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 24, 6 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 24, 7 });

            migrationBuilder.InsertData(
                table: "UnitTypeParentCategory",
                columns: new[] { "UnitTypeID", "ParentCategoryID" },
                values: new object[,]
                {
                    { 10, 7 },
                    { 9, 7 },
                    { 20, 7 },
                    { 21, 7 },
                    { 22, 7 },
                    { 13, 7 },
                    { 12, 7 },
                    { 11, 7 }
                });
        }
    }
}
