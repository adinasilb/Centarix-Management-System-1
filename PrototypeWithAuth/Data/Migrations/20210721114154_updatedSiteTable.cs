using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class updatedSiteTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Sites",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Sites",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Line1Address",
                table: "Sites",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Sites",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryContactID",
                table: "Sites",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sites_PrimaryContactID",
                table: "Sites",
                column: "PrimaryContactID");

            migrationBuilder.AddForeignKey(
                name: "FK_Sites_AspNetUsers_PrimaryContactID",
                table: "Sites",
                column: "PrimaryContactID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sites_AspNetUsers_PrimaryContactID",
                table: "Sites");

            migrationBuilder.DropIndex(
                name: "IX_Sites_PrimaryContactID",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "Line1Address",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "PrimaryContactID",
                table: "Sites");
        }
    }
}
