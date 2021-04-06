using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedNewJobSubacategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 301,
                column: "Description",
                value: "Senior Scientist");

            migrationBuilder.UpdateData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 401,
                column: "Description",
                value: "Senior Scientist");

            migrationBuilder.InsertData(
                table: "JobSubcategoryTypes",
                columns: new[] { "JobSubcategoryTypeID", "Description", "JobCategoryTypeID" },
                values: new object[,]
                {
                    { 410, "Bioinformatician", 4 },
                    { 409, "Lab Manager", 4 },
                    { 408, "Sales", 4 },
                    { 407, "Business Development", 4 },
                    { 406, "Operation Executive", 4 },
                    { 405, "Production Worker", 4 },
                    { 404, "Team Manager", 4 },
                    { 403, "Lab Technician", 4 },
                    { 402, "Research Associate", 4 },
                    { 309, "Lab Manager", 3 },
                    { 308, "Sales", 3 },
                    { 307, "Business Development", 3 },
                    { 306, "Operation Executive", 3 },
                    { 305, "Production Worker", 3 },
                    { 304, "Team Manager", 3 },
                    { 303, "Lab Technician", 3 },
                    { 310, "Bioinformatician", 3 },
                    { 302, "Research Associate", 3 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 302);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 303);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 304);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 305);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 306);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 307);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 308);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 309);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 310);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 402);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 403);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 404);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 405);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 406);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 407);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 408);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 409);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 410);

            migrationBuilder.UpdateData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 301,
                column: "Description",
                value: "Biomarker");

            migrationBuilder.UpdateData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 401,
                column: "Description",
                value: "Delivery Systems");
        }
    }
}
