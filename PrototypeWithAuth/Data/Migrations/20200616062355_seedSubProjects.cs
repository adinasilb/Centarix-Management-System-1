using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class seedSubProjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SubProjects",
                columns: new[] { "SubProjectID", "ProjectID", "SubProjectDescription" },
                values: new object[,]
                {
                    { 101, 1, "Epigenetic Rejuvenation" },
                    { 102, 1, "Plasma Rejuvenation" },
                    { 201, 2, "AAV" },
                    { 301, 3, "Epigenetic Clock" },
                    { 302, 3, "Telomere Measurement" },
                    { 401, 4, "Biomarker Trial" },
                    { 501, 5, "General" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 301);

            migrationBuilder.DeleteData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 302);

            migrationBuilder.DeleteData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 401);

            migrationBuilder.DeleteData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 501);
        }
    }
}
