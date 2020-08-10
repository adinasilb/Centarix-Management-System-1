using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedQuoteStatusAwatingInvoiceAndOrdered : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ParentRequestID",
                table: "Requests",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNumber",
                table: "ParentRequests",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "QuoteStatuses",
                columns: new[] { "QuoteStatusID", "QuoteStatusDescription" },
                values: new object[] { 4, "QuoteOrdered" });

            migrationBuilder.InsertData(
                table: "QuoteStatuses",
                columns: new[] { "QuoteStatusID", "QuoteStatusDescription" },
                values: new object[] { 5, "AwaitingInvoice" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "QuoteStatuses",
                keyColumn: "QuoteStatusID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "QuoteStatuses",
                keyColumn: "QuoteStatusID",
                keyValue: 5);

            migrationBuilder.AlterColumn<int>(
                name: "ParentRequestID",
                table: "Requests",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNumber",
                table: "ParentRequests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
