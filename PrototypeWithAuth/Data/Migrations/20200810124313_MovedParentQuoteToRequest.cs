using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MovedParentQuoteToRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_ParentQuotes_ParentQuoteID1",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ParentQuoteID1",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ParentQuoteID1",
                table: "Requests");

            migrationBuilder.AlterColumn<int>(
                name: "ParentQuoteID",
                table: "Requests",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ParentQuoteID",
                table: "Requests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ParentQuoteID1",
                table: "Requests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ParentQuoteID1",
                table: "Requests",
                column: "ParentQuoteID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_ParentQuotes_ParentQuoteID1",
                table: "Requests",
                column: "ParentQuoteID1",
                principalTable: "ParentQuotes",
                principalColumn: "ParentQuoteID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
