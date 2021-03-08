using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedTempJobSubcategorySeededTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "JobSubcategoryTypes",
                columns: new[] { "JobSubcategoryTypeID", "Description", "JobCategoryTypeID" },
                values: new object[] { 301, "Biomarker", 3 });

            migrationBuilder.InsertData(
                table: "JobSubcategoryTypes",
                columns: new[] { "JobSubcategoryTypeID", "Description", "JobCategoryTypeID" },
                values: new object[] { 401, "Delivery Systems", 4 });

            migrationBuilder.InsertData(
                table: "JobSubcategoryTypes",
                columns: new[] { "JobSubcategoryTypeID", "Description", "JobCategoryTypeID" },
                values: new object[] { 501, "Clinical Trials", 5 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 301);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 401);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 501);
        }
    }
}
