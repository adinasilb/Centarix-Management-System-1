using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ReAddedRequestNotificationFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestID",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestName",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestStatusID",
                table: "Notifications",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_RequestStatuses_RequestStatusID",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_RequestStatusID",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "RequestID",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "RequestName",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "RequestStatusID",
                table: "Notifications");
        }
    }
}
