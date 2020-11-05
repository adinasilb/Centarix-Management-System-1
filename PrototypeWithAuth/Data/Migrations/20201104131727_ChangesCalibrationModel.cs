using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ChangesCalibrationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<int>(
                name: "Months",
                table: "Calibrations",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsRepeat",
                table: "Calibrations",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Done",
                table: "Calibrations",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Days",
                table: "Calibrations",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Months",
                table: "Calibrations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<bool>(
                name: "IsRepeat",
                table: "Calibrations",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "Done",
                table: "Calibrations",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<int>(
                name: "Days",
                table: "Calibrations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "InternalCalibration_Days",
                table: "Calibrations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InternalCalibration_Done",
                table: "Calibrations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InternalCalibration_IsRepeat",
                table: "Calibrations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InternalCalibration_Months",
                table: "Calibrations",
                type: "int",
                nullable: true);
        }
    }
}
