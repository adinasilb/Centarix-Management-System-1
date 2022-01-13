using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddOffDayDescriptionEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionEnum",
                table: "OffDayTypes",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 1,
                column: "DescriptionEnum",
                value: "SickDay");

            migrationBuilder.UpdateData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 2,
                column: "DescriptionEnum",
                value: "VacationDay");

            migrationBuilder.UpdateData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 3,
                column: "DescriptionEnum",
                value: "MaternityLeave");

            migrationBuilder.UpdateData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 4,
                column: "DescriptionEnum",
                value: "SpecialDay");

            migrationBuilder.UpdateData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 5,
                column: "DescriptionEnum",
                value: "UnpaidLeave");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionEnum",
                table: "OffDayTypes");
        }
    }
}
