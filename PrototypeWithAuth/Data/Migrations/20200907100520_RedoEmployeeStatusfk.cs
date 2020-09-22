using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RedoEmployeeStatusfk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Freelancers_EmployeeStatuses_EmployeeStatusID",
                table: "Freelancers");

            migrationBuilder.DropForeignKey(
                name: "FK_SalariedEmployees_EmployeeStatuses_EmployeeStatusID",
                table: "SalariedEmployees");

            migrationBuilder.DropIndex(
                name: "IX_SalariedEmployees_EmployeeStatusID",
                table: "SalariedEmployees");

            migrationBuilder.DropIndex(
                name: "IX_Freelancers_EmployeeStatusID",
                table: "Freelancers");

            migrationBuilder.DropColumn(
                name: "EmployeeStatusID",
                table: "SalariedEmployees");

            migrationBuilder.DropColumn(
                name: "EmployeeStatusID",
                table: "Freelancers");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "SalariedEmployees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId1",
                table: "SalariedEmployees",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Freelancers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId1",
                table: "Freelancers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeStatusID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalariedEmployees_EmployeeId1",
                table: "SalariedEmployees",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Freelancers_EmployeeId1",
                table: "Freelancers",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmployeeStatusID",
                table: "AspNetUsers",
                column: "EmployeeStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_EmployeeStatuses_EmployeeStatusID",
                table: "AspNetUsers",
                column: "EmployeeStatusID",
                principalTable: "EmployeeStatuses",
                principalColumn: "EmployeeStatusID",
                onDelete: ReferentialAction.Restrict);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_EmployeeStatuses_EmployeeStatusID",
                table: "AspNetUsers");

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

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmployeeStatusID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "SalariedEmployees");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "SalariedEmployees");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Freelancers");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "Freelancers");

            migrationBuilder.DropColumn(
                name: "EmployeeStatusID",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeStatusID",
                table: "SalariedEmployees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeStatusID",
                table: "Freelancers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SalariedEmployees_EmployeeStatusID",
                table: "SalariedEmployees",
                column: "EmployeeStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Freelancers_EmployeeStatusID",
                table: "Freelancers",
                column: "EmployeeStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Freelancers_EmployeeStatuses_EmployeeStatusID",
                table: "Freelancers",
                column: "EmployeeStatusID",
                principalTable: "EmployeeStatuses",
                principalColumn: "EmployeeStatusID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalariedEmployees_EmployeeStatuses_EmployeeStatusID",
                table: "SalariedEmployees",
                column: "EmployeeStatusID",
                principalTable: "EmployeeStatuses",
                principalColumn: "EmployeeStatusID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
