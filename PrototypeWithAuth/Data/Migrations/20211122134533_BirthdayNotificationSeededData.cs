using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class BirthdayNotificationSeededData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "NotificationStatuses",
                columns: new[] { "NotificationStatusID", "Color", "Description", "Discriminator", "Icon" },
                values: new object[] { 6, "black", "Happy Birthday", "EmployeeInfoNotificationStatus", "icon-notification_birthday-24px" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NotificationStatuses",
                keyColumn: "NotificationStatusID",
                keyValue: 6);
        }
    }
}
