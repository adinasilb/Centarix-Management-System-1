using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedRequestStatusSeven : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RequestStatuses",
                columns: new[] { "RequestStatusID", "RequestStatusDescription" },
                values: new object[] { 7, "Saved To Inventory" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RequestStatuses",
                keyColumn: "RequestStatusID",
                keyValue: 7);
        }
    }
}
