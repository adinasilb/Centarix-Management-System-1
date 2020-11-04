using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedInternalExternalVariables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Days",
                table: "Calibrations",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Done",
                table: "Calibrations",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRepeat",
                table: "Calibrations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Months",
                table: "Calibrations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InternalCalibration_Days",
                table: "Calibrations",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InternalCalibration_Done",
                table: "Calibrations",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InternalCalibration_IsRepeat",
                table: "Calibrations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InternalCalibration_Months",
                table: "Calibrations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Days",
                table: "Calibrations");

            migrationBuilder.DropColumn(
                name: "Done",
                table: "Calibrations");

            migrationBuilder.DropColumn(
                name: "IsRepeat",
                table: "Calibrations");

            migrationBuilder.DropColumn(
                name: "Months",
                table: "Calibrations");

            migrationBuilder.DropColumn(
                name: "InternalCalibration_Days",
                table: "Calibrations");

            migrationBuilder.DropColumn(
                name: "InternalCalibration_Done",
                table: "Calibrations");

            migrationBuilder.DropColumn(
                name: "InternalCalibration_IsRepeat",
                table: "Calibrations");

            migrationBuilder.DropColumn(
                name: "InternalCalibration_Months",
                table: "Calibrations");
        }
    }
}
