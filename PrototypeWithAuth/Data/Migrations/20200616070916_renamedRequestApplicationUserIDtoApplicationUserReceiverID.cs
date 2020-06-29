using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class renamedRequestApplicationUserIDtoApplicationUserReceiverID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_ApplicationUserID",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ApplicationUserID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ApplicationUserID",
                table: "Requests");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserReceiverID",
                table: "Requests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ApplicationUserReceiverID",
                table: "Requests",
                column: "ApplicationUserReceiverID");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_ApplicationUserReceiverID",
                table: "Requests",
                column: "ApplicationUserReceiverID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_ApplicationUserReceiverID",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ApplicationUserReceiverID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ApplicationUserReceiverID",
                table: "Requests");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserID",
                table: "Requests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ApplicationUserID",
                table: "Requests",
                column: "ApplicationUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_ApplicationUserID",
                table: "Requests",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
