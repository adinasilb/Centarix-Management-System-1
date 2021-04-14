using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ChangedSpecialVacationDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 4,
                column: "Description",
                value: "Special Vacation Day");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 4,
                column: "Description",
                value: "Special Day");
        }
    }
}
