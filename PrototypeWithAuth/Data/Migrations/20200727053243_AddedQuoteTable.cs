using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedQuoteTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentQuoteID",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuoteStatusID",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Requests",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ParentQuoteID",
                table: "Requests",
                column: "ParentQuoteID");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_QuoteStatusID",
                table: "Requests",
                column: "QuoteStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_ParentQuotes_ParentQuoteID",
                table: "Requests",
                column: "ParentQuoteID",
                principalTable: "ParentQuotes",
                principalColumn: "ParentQuoteID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_QuoteStatuses_QuoteStatusID",
                table: "Requests",
                column: "QuoteStatusID",
                principalTable: "QuoteStatuses",
                principalColumn: "QuoteStatusID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_ParentQuotes_ParentQuoteID",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_QuoteStatuses_QuoteStatusID",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ParentQuoteID",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_QuoteStatusID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ParentQuoteID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "QuoteStatusID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Requests");
        }
    }
}
