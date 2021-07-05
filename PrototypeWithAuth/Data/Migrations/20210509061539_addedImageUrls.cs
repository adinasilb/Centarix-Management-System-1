using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedImageUrls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 1,
                column: "ImageUrl",
                value: "rejuvenation_image.svg");

            migrationBuilder.UpdateData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 2,
                column: "ImageUrl",
                value: "biomarkers_image.svg");

            migrationBuilder.UpdateData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 3,
                column: "ImageUrl",
                value: "delivery_systems_image.svg");

            migrationBuilder.UpdateData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 4,
                column: "ImageUrl",
                value: "clinical_trials_image.svg");

            migrationBuilder.UpdateData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 14,
                columns: new[] { "ImageUrl", "ResourceCategoryDescription" },
                values: new object[] { "software_image.svg", "Software" });

            migrationBuilder.UpdateData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 15,
                column: "ImageUrl",
                value: "learning_image.svg");

            migrationBuilder.UpdateData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 16,
                column: "ImageUrl",
                value: "companies_image.svg");

            migrationBuilder.UpdateData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 17,
                column: "ImageUrl",
                value: "news_image.svg");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 1,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 2,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 3,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 4,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 14,
                columns: new[] { "ImageUrl", "ResourceCategoryDescription" },
                values: new object[] { null, "Softwares" });

            migrationBuilder.UpdateData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 15,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 16,
                column: "ImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 17,
                column: "ImageUrl",
                value: null);
        }
    }
}
