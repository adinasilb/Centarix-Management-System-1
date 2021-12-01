using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class UpdatedBirthdayIcon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NotificationStatuses",
                keyColumn: "NotificationStatusID",
                keyValue: 6,
                column: "Icon",
                value: "icon-cake_black_24dp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NotificationStatuses",
                keyColumn: "NotificationStatusID",
                keyValue: 6,
                column: "Icon",
                value: "icon-centarix-icons-10");
        }
    }
}
