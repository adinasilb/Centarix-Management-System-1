using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddUnpaidLeaveOffDay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OffDayTypes",
                columns: new[] { "OffDayTypeID", "Description" },
                values: new object[] { 5, "Unpaid Leave" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OffDayTypes",
                keyColumn: "OffDayTypeID",
                keyValue: 5);
        }
    }
}
