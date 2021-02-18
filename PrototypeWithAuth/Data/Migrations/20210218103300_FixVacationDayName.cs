using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class FixVacationDayName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 2,
                column: "Description",
                value: "Vacation Day");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 2,
                column: "Description",
                value: " Vacation Day");
        }
    }
}
