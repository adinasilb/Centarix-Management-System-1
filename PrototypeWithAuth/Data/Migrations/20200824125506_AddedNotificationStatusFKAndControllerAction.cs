using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedNotificationStatusFKAndControllerAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Controller",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NotificationStatusID",
                table: "Notifications",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationStatusID",
                table: "Notifications",
                column: "NotificationStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_NotificationStatuses_NotificationStatusID",
                table: "Notifications",
                column: "NotificationStatusID",
                principalTable: "NotificationStatuses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_NotificationStatuses_NotificationStatusID",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_NotificationStatusID",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Controller",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "NotificationStatusID",
                table: "Notifications");
        }
    }
}
