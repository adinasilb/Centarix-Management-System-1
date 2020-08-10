using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedQuoteStatusInvoiceSent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "QuoteStatuses",
                keyColumn: "QuoteStatusID",
                keyValue: 4,
                column: "QuoteStatusDescription",
                value: "InvoiceSent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "QuoteStatuses",
                keyColumn: "QuoteStatusID",
                keyValue: 4,
                column: "QuoteStatusDescription",
                value: "QuoteOrdered");
        }
    }
}
