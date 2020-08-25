using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedRequestNotificationStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_RequestStatuses_RequestStatusID",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_RequestStatusID",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "NotificationStatuses",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "NotificationStatuses");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RequestStatusID",
                table: "Notifications",
                column: "RequestStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_RequestStatuses_RequestStatusID",
                table: "Notifications",
                column: "RequestStatusID",
                principalTable: "RequestStatuses",
                principalColumn: "RequestStatusID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
