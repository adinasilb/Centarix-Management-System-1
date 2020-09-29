using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedFKCitizenshipToEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Citizenship",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "CitizenshipID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CitizenshipID",
                table: "AspNetUsers",
                column: "CitizenshipID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Citizenships_CitizenshipID",
                table: "AspNetUsers",
                column: "CitizenshipID",
                principalTable: "Citizenships",
                principalColumn: "CitizenshipID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Citizenships_CitizenshipID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CitizenshipID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CitizenshipID",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Citizenship",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
