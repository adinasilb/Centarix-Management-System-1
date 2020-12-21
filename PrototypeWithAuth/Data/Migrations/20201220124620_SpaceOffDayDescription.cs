using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class SpaceOffDayDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 1,
                column: "Description",
                value: "Sick Day");

            migrationBuilder.UpdateData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 2,
                column: "Description",
                value: " Vacation Day");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 1,
                column: "Description",
                value: "SickDay");

            migrationBuilder.UpdateData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 2,
                column: "Description",
                value: " VacationDay");
        }
    }
}
