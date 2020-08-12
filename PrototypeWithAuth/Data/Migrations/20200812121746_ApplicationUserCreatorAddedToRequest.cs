using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ApplicationUserCreatorAddedToRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserCreatorID",
                table: "Requests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ApplicationUserCreatorID",
                table: "Requests",
                column: "ApplicationUserCreatorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_ApplicationUserCreatorID",
                table: "Requests",
                column: "ApplicationUserCreatorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_ApplicationUserCreatorID",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ApplicationUserCreatorID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ApplicationUserCreatorID",
                table: "Requests");
        }
    }
}
