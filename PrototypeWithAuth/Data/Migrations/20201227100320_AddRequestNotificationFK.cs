using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddRequestNotificationFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RequestNotifications_RequestID",
                table: "RequestNotifications",
                column: "RequestID");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestNotifications_Requests_RequestID",
                table: "RequestNotifications",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "RequestID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestNotifications_Requests_RequestID",
                table: "RequestNotifications");

            migrationBuilder.DropIndex(
                name: "IX_RequestNotifications_RequestID",
                table: "RequestNotifications");
        }
    }
}
