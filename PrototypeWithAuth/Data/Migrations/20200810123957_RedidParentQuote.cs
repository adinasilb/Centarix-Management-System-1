using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RedidParentQuote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentQuoteID1",
                table: "Requests",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QuoteNumber",
                table: "ParentQuotes",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuoteStatusID",
                table: "ParentQuotes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ParentQuoteID1",
                table: "Requests",
                column: "ParentQuoteID1");

            migrationBuilder.CreateIndex(
                name: "IX_ParentQuotes_QuoteStatusID",
                table: "ParentQuotes",
                column: "QuoteStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_ParentQuotes_QuoteStatuses_QuoteStatusID",
                table: "ParentQuotes",
                column: "QuoteStatusID",
                principalTable: "QuoteStatuses",
                principalColumn: "QuoteStatusID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_ParentQuotes_ParentQuoteID1",
                table: "Requests",
                column: "ParentQuoteID1",
                principalTable: "ParentQuotes",
                principalColumn: "ParentQuoteID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParentQuotes_QuoteStatuses_QuoteStatusID",
                table: "ParentQuotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_ParentQuotes_ParentQuoteID1",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ParentQuoteID1",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_ParentQuotes_QuoteStatusID",
                table: "ParentQuotes");

            migrationBuilder.DropColumn(
                name: "ParentQuoteID1",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "QuoteStatusID",
                table: "ParentQuotes");

            migrationBuilder.AlterColumn<int>(
                name: "QuoteNumber",
                table: "ParentQuotes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
