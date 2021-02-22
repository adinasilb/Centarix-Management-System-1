using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class TookAwayPartialClarifyStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RequestStatuses",
                keyColumn: "RequestStatusID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RequestStatuses",
                keyColumn: "RequestStatusID",
                keyValue: 5);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RequestStatuses",
                columns: new[] { "RequestStatusID", "RequestStatusDescription" },
                values: new object[] { 4, "Partial" });

            migrationBuilder.InsertData(
                table: "RequestStatuses",
                columns: new[] { "RequestStatusID", "RequestStatusDescription" },
                values: new object[] { 5, "Clarify" });
        }
    }
}
