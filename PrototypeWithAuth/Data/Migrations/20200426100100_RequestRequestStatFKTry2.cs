using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RequestRequestStatFKTry2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
    name: "RequestStatusID",
    table: "Requests",
    nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestStatusID",
                table: "Requests",
                column: "RequestStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_RequestStatuses_RequestStatusID",
                table: "Requests",
                column: "RequestStatusID",
                principalTable: "RequestStatuses",
                principalColumn: "RequestStatusID",
                onDelete: ReferentialAction.Restrict);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
    name: "FK_Requests_RequestStatuses_RequestStatusID",
    table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_RequestStatusID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "RequestStatusID",
                table: "Requests");

        }
    }
}
