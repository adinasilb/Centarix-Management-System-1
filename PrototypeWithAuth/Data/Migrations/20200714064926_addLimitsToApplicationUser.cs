using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addLimitsToApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "LabMonthlyLimit",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LabOrderLimit",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LabUnitLimit",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OperaitonOrderLimit",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OperationMonthlyLimit",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OperationUnitLimit",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LabMonthlyLimit",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LabOrderLimit",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LabUnitLimit",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OperaitonOrderLimit",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OperationMonthlyLimit",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OperationUnitLimit",
                table: "AspNetUsers");
        }
    }
}
