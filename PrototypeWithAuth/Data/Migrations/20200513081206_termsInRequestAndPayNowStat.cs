using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class termsInRequestAndPayNowStat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Terms",
                table: "Requests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "RequestStatuses",
                columns: new[] { "RequestStatusID", "RequestStatusDescription" },
                values: new object[] { 6, "PayNow" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RequestStatuses",
                keyColumn: "RequestStatusID",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "Terms",
                table: "Requests");
        }
    }
}
