using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addTubeUnitToParentCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UnitTypeParentCategory",
                columns: new[] { "UnitTypeID", "ParentCategoryID" },
                values: new object[] { 24, 1 });

            migrationBuilder.InsertData(
                table: "UnitTypeParentCategory",
                columns: new[] { "UnitTypeID", "ParentCategoryID" },
                values: new object[] { 24, 2 });

            migrationBuilder.InsertData(
                table: "UnitTypeParentCategory",
                columns: new[] { "UnitTypeID", "ParentCategoryID" },
                values: new object[] { 24, 4 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 24, 1 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 24, 2 });

            migrationBuilder.DeleteData(
                table: "UnitTypeParentCategory",
                keyColumns: new[] { "UnitTypeID", "ParentCategoryID" },
                keyValues: new object[] { 24, 4 });
        }
    }
}
