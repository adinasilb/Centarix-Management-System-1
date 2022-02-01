using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class BirthdayNotificationColorBlack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NotificationStatuses",
                keyColumn: "NotificationStatusID",
                keyValue: 6,
                column: "Color",
                value: "--black-87");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NotificationStatuses",
                keyColumn: "NotificationStatusID",
                keyValue: 6,
                column: "Color",
                value: "black");
        }
    }
}
