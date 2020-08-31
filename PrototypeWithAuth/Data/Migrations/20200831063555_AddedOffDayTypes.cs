using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedOffDayTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OffDayTypes",
                columns: new[] { "OffDayTypeID", "Description" },
                values: new object[] { 1, "SickDay" });

            migrationBuilder.InsertData(
                table: "OffDayTypes",
                columns: new[] { "OffDayTypeID", "Description" },
                values: new object[] { 2, " VacationDay" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 2);
        }
    }
}
