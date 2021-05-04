using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class seededResourceCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ResourceCategories",
                columns: new[] { "ResourceCategoryID", "IsMain", "IsResourceType", "ResourceCategoryDescription" },
                values: new object[,]
                {
                    { 1, true, false, "Rejuvenation" },
                    { 15, false, true, "Learning" },
                    { 14, false, true, "Softwares" },
                    { 13, false, false, "New Methods" },
                    { 12, false, false, "Methylation Rejuvenation" },
                    { 11, false, false, "Reprogramming" },
                    { 10, false, false, "Serum Rejuvenation" },
                    { 16, false, true, "Companies" },
                    { 9, false, false, "Transcriptome" },
                    { 7, false, false, "Telomere Measurement" },
                    { 6, false, false, "Telomere Rejuvenation" },
                    { 5, false, false, "AAV" },
                    { 4, true, false, "Clinical Trials" },
                    { 3, true, false, "Delivery Systems" },
                    { 2, true, false, "Biomarkers" },
                    { 8, false, false, "Methylation Biomarker" },
                    { 17, false, true, "News" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ResourceCategories",
                keyColumn: "ResourceCategoryID",
                keyValue: 17);
        }
    }
}
