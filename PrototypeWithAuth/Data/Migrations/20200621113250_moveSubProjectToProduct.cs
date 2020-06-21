using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class moveSubProjectToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_SubProjects_SubProjectID",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_SubProjectID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SubProjectID",
                table: "Requests");

            migrationBuilder.AddColumn<int>(
                name: "SubProjectID",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubProjectID",
                table: "Products",
                column: "SubProjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SubProjects_SubProjectID",
                table: "Products",
                column: "SubProjectID",
                principalTable: "SubProjects",
                principalColumn: "SubProjectID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_SubProjects_SubProjectID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SubProjectID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SubProjectID",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "SubProjectID",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_SubProjectID",
                table: "Requests",
                column: "SubProjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_SubProjects_SubProjectID",
                table: "Requests",
                column: "SubProjectID",
                principalTable: "SubProjects",
                principalColumn: "SubProjectID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
