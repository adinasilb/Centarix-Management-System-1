using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ChangedSpecialDaysName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpecialVacationDays",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<double>(
                name: "SpecialDays",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 4,
                column: "Description",
                value: "Special Day");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpecialDays",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<double>(
                name: "SpecialVacationDays",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 4,
                column: "Description",
                value: "Special Vacation Day");
        }
    }
}
