using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedAwaitingQuoteOrderStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "QuoteStatuses",
                columns: new[] { "QuoteStatusID", "QuoteStatusDescription" },
                values: new object[] { 3, "AwaitingQuoteOrder" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "QuoteStatuses",
                keyColumn: "QuoteStatusID",
                keyValue: 3);
        }
    }
}
