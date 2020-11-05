using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedCalibrationTypeIcons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "CalibrationTypes",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CalibrationTypes",
                keyColumn: "CalibrationTypeID",
                keyValue: 1,
                column: "Icon",
                value: "icon-build-24px");

            migrationBuilder.UpdateData(
                table: "CalibrationTypes",
                keyColumn: "CalibrationTypeID",
                keyValue: 2,
                column: "Icon",
                value: "icon-miscellaneous_services-24px-1");

            migrationBuilder.UpdateData(
                table: "CalibrationTypes",
                keyColumn: "CalibrationTypeID",
                keyValue: 3,
                column: "Icon",
                value: "icon-inhouse-maintainance-24px");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "CalibrationTypes");
        }
    }
}
