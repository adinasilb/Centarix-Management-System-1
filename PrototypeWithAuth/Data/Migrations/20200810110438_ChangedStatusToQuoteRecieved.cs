using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ChangedStatusToQuoteRecieved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "QuoteStatuses",
                keyColumn: "QuoteStatusID",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "QuoteStatuses",
                keyColumn: "QuoteStatusID",
                keyValue: 4,
                column: "QuoteStatusDescription",
                value: "QuoteRecieved");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "QuoteStatuses",
                keyColumn: "QuoteStatusID",
                keyValue: 4,
                column: "QuoteStatusDescription",
                value: "InvoiceSent");

            migrationBuilder.InsertData(
                table: "QuoteStatuses",
                columns: new[] { "QuoteStatusID", "QuoteStatusDescription" },
                values: new object[] { 5, "AwaitingInvoice" });
        }
    }
}
