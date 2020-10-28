using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedJobCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "JobCategoryTypes",
                columns: new[] { "JobCategoryTypeID", "Description" },
                values: new object[] { 10, "Administration" });

            migrationBuilder.InsertData(
                table: "JobCategoryTypes",
                columns: new[] { "JobCategoryTypeID", "Description" },
                values: new object[] { 11, "General" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 11);
        }
    }
}
