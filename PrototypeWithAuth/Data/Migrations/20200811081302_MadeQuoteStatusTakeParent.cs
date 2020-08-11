using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MadeQuoteStatusTakeParent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_QuoteStatuses_QuoteStatusID",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_QuoteStatusID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "QuoteStatusID",
                table: "Requests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuoteStatusID",
                table: "Requests",
                type: "int",
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
                onDelete: ReferentialAction.Restrict);
        }
    }
}
