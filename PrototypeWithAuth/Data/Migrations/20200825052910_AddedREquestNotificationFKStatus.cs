using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedREquestNotificationFKStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestNotificationStatusid",
                table: "Notifications",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RequestNotificationStatusid",
                table: "Notifications",
                column: "RequestNotificationStatusid");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_NotificationStatuses_RequestNotificationStatusid",
                table: "Notifications",
                column: "RequestNotificationStatusid",
                principalTable: "NotificationStatuses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_NotificationStatuses_RequestNotificationStatusid",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_RequestNotificationStatusid",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "RequestNotificationStatusid",
                table: "Notifications");
        }
    }
}
