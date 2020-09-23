using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedFKMaritalStatusToEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelationshipStatus",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "MaritalStatusID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MaritalStatusID",
                table: "AspNetUsers",
                column: "MaritalStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_MaritalStatuses_MaritalStatusID",
                table: "AspNetUsers",
                column: "MaritalStatusID",
                principalTable: "MaritalStatuses",
                principalColumn: "MaritalStatusID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_MaritalStatuses_MaritalStatusID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_MaritalStatusID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MaritalStatusID",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "RelationshipStatus",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
