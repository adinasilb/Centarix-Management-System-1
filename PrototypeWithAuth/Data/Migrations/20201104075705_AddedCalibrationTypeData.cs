using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedCalibrationTypeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Calibrations",
                nullable: true);

            migrationBuilder.InsertData(
                table: "CalibrationTypes",
                columns: new[] { "CalibrationTypeID", "Description" },
                values: new object[] { 1, "Repair" });

            migrationBuilder.InsertData(
                table: "CalibrationTypes",
                columns: new[] { "CalibrationTypeID", "Description" },
                values: new object[] { 2, "External Calibration" });

            migrationBuilder.InsertData(
                table: "CalibrationTypes",
                columns: new[] { "CalibrationTypeID", "Description" },
                values: new object[] { 3, "In House Maintainance" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CalibrationTypes",
                keyColumn: "CalibrationTypeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CalibrationTypes",
                keyColumn: "CalibrationTypeID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CalibrationTypes",
                keyColumn: "CalibrationTypeID",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Calibrations");
        }
    }
}
