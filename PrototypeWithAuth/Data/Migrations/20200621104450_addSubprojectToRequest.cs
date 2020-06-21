using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addSubprojectToRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubProjectID",
                table: "Requests",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
