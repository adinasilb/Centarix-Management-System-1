using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addGeneralOptionAllSubprojects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 101,
                column: "SubProjectDescription",
                value: "General");

            migrationBuilder.UpdateData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 102,
                column: "SubProjectDescription",
                value: "Epigenetic Rejuvenation");

            migrationBuilder.UpdateData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 201,
                column: "SubProjectDescription",
                value: "General");

            migrationBuilder.UpdateData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 301,
                column: "SubProjectDescription",
                value: "General");

            migrationBuilder.UpdateData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 302,
                column: "SubProjectDescription",
                value: "Epigenetic Clock");

            migrationBuilder.UpdateData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 401,
                column: "SubProjectDescription",
                value: "General");

            migrationBuilder.InsertData(
                table: "SubProjects",
                columns: new[] { "SubProjectID", "ProjectID", "SubProjectDescription" },
                values: new object[,]
                {
                    { 103, 1, "Plasma Rejuvenation" },
                    { 202, 2, "AAV" },
                    { 303, 3, "Telomere Measurement" },
                    { 402, 4, "Biomarker Trial" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 303);

            migrationBuilder.DeleteData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 402);

            migrationBuilder.UpdateData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 101,
                column: "SubProjectDescription",
                value: "Epigenetic Rejuvenation");

            migrationBuilder.UpdateData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 102,
                column: "SubProjectDescription",
                value: "Plasma Rejuvenation");

            migrationBuilder.UpdateData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 201,
                column: "SubProjectDescription",
                value: "AAV");

            migrationBuilder.UpdateData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 301,
                column: "SubProjectDescription",
                value: "Epigenetic Clock");

            migrationBuilder.UpdateData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 302,
                column: "SubProjectDescription",
                value: "Telomere Measurement");

            migrationBuilder.UpdateData(
                table: "SubProjects",
                keyColumn: "SubProjectID",
                keyValue: 401,
                column: "SubProjectDescription",
                value: "Biomarker Trial");
        }
    }
}
