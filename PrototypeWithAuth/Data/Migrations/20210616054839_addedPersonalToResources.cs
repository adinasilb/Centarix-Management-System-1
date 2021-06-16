using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedPersonalToResources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserCreatorID",
                table: "Resources",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPersonal",
                table: "Resources",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ApplicationUserCreatorID",
                table: "Resources",
                column: "ApplicationUserCreatorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_AspNetUsers_ApplicationUserCreatorID",
                table: "Resources",
                column: "ApplicationUserCreatorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_AspNetUsers_ApplicationUserCreatorID",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Resources_ApplicationUserCreatorID",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "ApplicationUserCreatorID",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "IsPersonal",
                table: "Resources");
        }
    }
}
