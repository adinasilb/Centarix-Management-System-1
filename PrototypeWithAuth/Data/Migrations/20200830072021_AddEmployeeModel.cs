using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddEmployeeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "BituachLeumiEmployer",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "EducationFundEmployer",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Food",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "GrossSalary",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "IncomeTax",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PensionEmployer",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedWorking",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Transportation",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkScope",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

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
                name: "GrossSalary",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IncomeTax",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PensionEmployer",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StartedWorking",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Transportation",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WorkScope",
                table: "AspNetUsers");
        }
    }
}
