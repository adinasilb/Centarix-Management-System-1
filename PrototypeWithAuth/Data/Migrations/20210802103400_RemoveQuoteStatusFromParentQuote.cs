using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RemoveQuoteStatusFromParentQuote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParentQuotes_QuoteStatuses_QuoteStatusID",
                table: "ParentQuotes");

            migrationBuilder.DropIndex(
                name: "IX_ParentQuotes_QuoteStatusID",
                table: "ParentQuotes");

            migrationBuilder.DropColumn(
                name: "QuoteStatusID",
                table: "ParentQuotes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuoteStatusID",
                table: "ParentQuotes",
                type: "int",
                nullable: true);

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
        }
    }
}
