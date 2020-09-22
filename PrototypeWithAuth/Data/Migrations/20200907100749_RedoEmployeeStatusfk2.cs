using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RedoEmployeeStatusfk2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Freelancers_AspNetUsers_EmployeeId1",
                table: "Freelancers");

            migrationBuilder.DropForeignKey(
                name: "FK_SalariedEmployees_AspNetUsers_EmployeeId1",
                table: "SalariedEmployees");

            migrationBuilder.DropIndex(
                name: "IX_SalariedEmployees_EmployeeId1",
                table: "SalariedEmployees");

            migrationBuilder.DropIndex(
                name: "IX_Freelancers_EmployeeId1",
                table: "Freelancers");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "SalariedEmployees");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "Freelancers");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "SalariedEmployees",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "Freelancers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_SalariedEmployees_EmployeeId",
                table: "SalariedEmployees",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Freelancers_EmployeeId",
                table: "Freelancers",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Freelancers_AspNetUsers_EmployeeId",
                table: "Freelancers",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalariedEmployees_AspNetUsers_EmployeeId",
                table: "SalariedEmployees",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Freelancers_AspNetUsers_EmployeeId",
                table: "Freelancers");

            migrationBuilder.DropForeignKey(
                name: "FK_SalariedEmployees_AspNetUsers_EmployeeId",
                table: "SalariedEmployees");

            migrationBuilder.DropIndex(
                name: "IX_SalariedEmployees_EmployeeId",
                table: "SalariedEmployees");

            migrationBuilder.DropIndex(
                name: "IX_Freelancers_EmployeeId",
                table: "Freelancers");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "SalariedEmployees",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId1",
                table: "SalariedEmployees",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "Freelancers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId1",
                table: "Freelancers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalariedEmployees_EmployeeId1",
                table: "SalariedEmployees",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Freelancers_EmployeeId1",
                table: "Freelancers",
                column: "EmployeeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Freelancers_AspNetUsers_EmployeeId1",
                table: "Freelancers",
                column: "EmployeeId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalariedEmployees_AspNetUsers_EmployeeId1",
                table: "SalariedEmployees",
                column: "EmployeeId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
