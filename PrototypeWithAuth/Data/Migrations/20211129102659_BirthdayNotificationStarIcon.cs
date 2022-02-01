using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class BirthdayNotificationStarIcon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NotificationStatuses",
                keyColumn: "NotificationStatusID",
                keyValue: 6,
                column: "Icon",
                value: "icon-centarix-icons-10");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NotificationStatuses",
                keyColumn: "NotificationStatusID",
                keyValue: 6,
                column: "Icon",
                value: "icon-notification_birthday-24px");
        }
    }
}
