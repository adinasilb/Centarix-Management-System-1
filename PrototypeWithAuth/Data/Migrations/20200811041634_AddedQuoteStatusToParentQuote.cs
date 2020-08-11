using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedQuoteStatusToParentQuote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuoteStatusID",
                table: "ParentQuotes",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
