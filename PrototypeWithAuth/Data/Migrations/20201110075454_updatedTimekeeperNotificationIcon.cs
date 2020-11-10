using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class updatedTimekeeperNotificationIcon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NotificationStatuses",
                keyColumn: "NotificationStatusID",
                keyValue: 5,
                column: "Icon",
                value: "icon-notification_timekeeper-24px");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NotificationStatuses",
                keyColumn: "NotificationStatusID",
                keyValue: 5,
                column: "Icon",
                value: "icon-access_time-24px");
        }
    }
}
