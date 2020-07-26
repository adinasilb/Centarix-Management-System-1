using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedARequestStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "RequestStatuses",
                keyColumn: "RequestStatusID",
                keyValue: 6,
                column: "RequestStatusDescription",
                value: "AwaitingQuote");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "RequestStatuses",
                keyColumn: "RequestStatusID",
                keyValue: 6,
                column: "RequestStatusDescription",
                value: "PayNow");
        }
    }
}
