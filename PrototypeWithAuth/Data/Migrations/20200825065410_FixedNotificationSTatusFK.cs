using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class FixedNotificationSTatusFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_NotificationStatuses_RequestNotificationStatusid",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_RequestNotificationStatusid",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "RequestID",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "RequestName",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "RequestNotificationStatusid",
                table: "Notifications");

            migrationBuilder.CreateTable(
                name: "RequestNotifications",
                columns: table => new
                {
                    NotificationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false),
                    ApplicationUserID = table.Column<string>(nullable: true),
                    Controller = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    RequestID = table.Column<int>(nullable: false),
                    RequestName = table.Column<string>(nullable: true),
                    NotificationStatusID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestNotifications", x => x.NotificationID);
                    table.ForeignKey(
                        name: "FK_RequestNotifications_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestNotifications_NotificationStatuses_NotificationStatusID",
                        column: x => x.NotificationStatusID,
                        principalTable: "NotificationStatuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestNotifications_ApplicationUserID",
                table: "RequestNotifications",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestNotifications_NotificationStatusID",
                table: "RequestNotifications",
                column: "NotificationStatusID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestNotifications");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RequestID",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestName",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestNotificationStatusid",
                table: "Notifications",
                type: "int",
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
    }
}
