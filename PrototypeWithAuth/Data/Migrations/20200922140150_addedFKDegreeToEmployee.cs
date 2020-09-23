using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedFKDegreeToEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Degree",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "DegreeID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DegreeID",
                table: "AspNetUsers",
                column: "DegreeID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Degrees_DegreeID",
                table: "AspNetUsers",
                column: "DegreeID",
                principalTable: "Degrees",
                principalColumn: "DegreeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Degrees_DegreeID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DegreeID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DegreeID",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Degree",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
