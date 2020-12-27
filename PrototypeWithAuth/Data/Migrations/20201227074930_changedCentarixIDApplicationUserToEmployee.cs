using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class changedCentarixIDApplicationUserToEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CentarixIDs_AspNetUsers_ApplicationUserID",
                table: "CentarixIDs");

            migrationBuilder.DropIndex(
                name: "IX_CentarixIDs_ApplicationUserID",
                table: "CentarixIDs");

            migrationBuilder.DropColumn(
                name: "ApplicationUserID",
                table: "CentarixIDs");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeID",
                table: "CentarixIDs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeStatusID",
                table: "CentarixIDs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CentarixIDs_EmployeeID",
                table: "CentarixIDs",
                column: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_CentarixIDs_AspNetUsers_EmployeeID",
                table: "CentarixIDs",
                column: "EmployeeID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CentarixIDs_AspNetUsers_EmployeeID",
                table: "CentarixIDs");

            migrationBuilder.DropIndex(
                name: "IX_CentarixIDs_EmployeeID",
                table: "CentarixIDs");

            migrationBuilder.DropColumn(
                name: "EmployeeID",
                table: "CentarixIDs");

            migrationBuilder.DropColumn(
                name: "EmployeeStatusID",
                table: "CentarixIDs");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserID",
                table: "CentarixIDs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CentarixIDs_ApplicationUserID",
                table: "CentarixIDs",
                column: "ApplicationUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_CentarixIDs_AspNetUsers_ApplicationUserID",
                table: "CentarixIDs",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
