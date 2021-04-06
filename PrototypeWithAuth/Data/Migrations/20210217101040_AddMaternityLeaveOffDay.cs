using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddMaternityLeaveOffDay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OffDayTypes",
                columns: new[] { "OffDayTypeID", "Description" },
                values: new object[] { 3, "Maternity Leave" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 3);
        }
    }
}
