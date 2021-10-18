using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class whatIsNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropForeignKey(
                name: "FK_ParentQuotes_QuoteStatuses_QuoteStatusID",
                table: "ParentQuotes");

            migrationBuilder.DropIndex(
                name: "IX_ParentQuotes_QuoteStatusID",
                table: "ParentQuotes");

            migrationBuilder.DropColumn(
                name: "QuoteStatusID",
                table: "ParentQuotes");

            migrationBuilder.AddColumn<int>(
                name: "QuoteStatusID",
                table: "Requests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_QuoteStatusID",
                table: "Requests",
                column: "QuoteStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_QuoteStatuses_QuoteStatusID",
                table: "Requests",
                column: "QuoteStatusID",
                principalTable: "QuoteStatuses",
                principalColumn: "QuoteStatusID",
                onDelete: ReferentialAction.Restrict);*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropForeignKey(
                name: "FK_Requests_QuoteStatuses_QuoteStatusID",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_QuoteStatusID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "QuoteStatusID",
                table: "Requests");

            migrationBuilder.AddColumn<int>(
                name: "QuoteStatusID",
                table: "ParentQuotes",
                type: "int",
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
                onDelete: ReferentialAction.Restrict);*/
        }
    }
}
