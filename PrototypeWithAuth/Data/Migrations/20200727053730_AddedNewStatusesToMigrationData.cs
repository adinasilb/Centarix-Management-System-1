using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedNewStatusesToMigrationData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "QuoteStatuses",
                columns: new[] { "QuoteStatusID", "QuoteStatusDescription" },
                values: new object[] { 1, "AwaitingRequestOfQuote" });

            migrationBuilder.InsertData(
                table: "QuoteStatuses",
                columns: new[] { "QuoteStatusID", "QuoteStatusDescription" },
                values: new object[] { 2, "AwaitingQuoteResponse" });

            migrationBuilder.UpdateData(
                table: "RequestStatuses",
                keyColumn: "RequestStatusID",
                keyValue: 6,
                column: "RequestStatusDescription",
                value: "Approved");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "QuoteStatuses",
                keyColumn: "QuoteStatusID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "QuoteStatuses",
                keyColumn: "QuoteStatusID",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "RequestStatuses",
                keyColumn: "RequestStatusID",
                keyValue: 6,
                column: "RequestStatusDescription",
                value: "AwaitingQuote");
        }
    }
}
