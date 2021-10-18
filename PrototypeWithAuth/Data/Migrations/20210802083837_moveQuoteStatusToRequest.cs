using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class moveQuoteStatusToRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuoteStatusID",
                table: "Requests",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QuoteStatusID",
                table: "ParentQuotes",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "QuoteStatusID",
                table: "ParentQuotes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
