using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class EditEmployeeField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BituachLeumiEmployer",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EducationFundEmployer",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Food",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HoursPerWeek",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PensionEmployer",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Transportation",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WorkScope",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<double>(
                name: "EmployerTax",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "HoursPerDay",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployerTax",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HoursPerDay",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<double>(
                name: "BituachLeumiEmployer",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "EducationFundEmployer",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Food",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "HoursPerWeek",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PensionEmployer",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Transportation",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkScope",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }
    }
}
