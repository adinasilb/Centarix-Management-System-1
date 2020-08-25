using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MadeNotificationAbstract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestNotifications_NotificationStatuses_NotificationStatusID",
                table: "RequestNotifications");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationStatuses",
                table: "NotificationStatuses");

            migrationBuilder.DeleteData(
                table: "NotificationStatuses",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "NotificationStatuses",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "NotificationStatuses",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "NotificationStatuses",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "id",
                table: "NotificationStatuses");

            migrationBuilder.AddColumn<int>(
                name: "NotificationStatusID",
                table: "NotificationStatuses",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationStatuses",
                table: "NotificationStatuses",
                column: "NotificationStatusID");

            migrationBuilder.InsertData(
                table: "NotificationStatuses",
                columns: new[] { "NotificationStatusID", "Color", "Description", "Discriminator", "Icon" },
                values: new object[,]
                {
                    { 1, "--notifications-orderlate-color", "OrderLate", "RequestNotificationStatus", "icon-centarix-icons-05" },
                    { 2, "--notifications-ordered-color", "ItemOrdered", "RequestNotificationStatus", "icon-centarix-icons-05" },
                    { 3, "--notifications-approved-color", "ItemApproved", "RequestNotificationStatus", "icon-centarix-icons-05" },
                    { 4, "--notifications-received-color", "ItemReceived", "RequestNotificationStatus", "icon-centarix-icons-05" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RequestNotifications_NotificationStatuses_NotificationStatusID",
                table: "RequestNotifications",
                column: "NotificationStatusID",
                principalTable: "NotificationStatuses",
                principalColumn: "NotificationStatusID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestNotifications_NotificationStatuses_NotificationStatusID",
                table: "RequestNotifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationStatuses",
                table: "NotificationStatuses");

            migrationBuilder.DeleteData(
                table: "NotificationStatuses",
                keyColumn: "NotificationStatusID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "NotificationStatuses",
                keyColumn: "NotificationStatusID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "NotificationStatuses",
                keyColumn: "NotificationStatusID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "NotificationStatuses",
                keyColumn: "NotificationStatusID",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "NotificationStatusID",
                table: "NotificationStatuses");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "NotificationStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationStatuses",
                table: "NotificationStatuses",
                column: "id");

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationUserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Controller = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    NotificationStatusID = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationID);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_NotificationStatuses_NotificationStatusID",
                        column: x => x.NotificationStatusID,
                        principalTable: "NotificationStatuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "NotificationStatuses",
                columns: new[] { "id", "Color", "Description", "Discriminator", "Icon" },
                values: new object[,]
                {
                    { 1, "--notifications-orderlate-color", "OrderLate", "RequestNotificationStatus", "icon-centarix-icons-05" },
                    { 2, "--notifications-ordered-color", "ItemOrdered", "RequestNotificationStatus", "icon-centarix-icons-05" },
                    { 3, "--notifications-approved-color", "ItemApproved", "RequestNotificationStatus", "icon-centarix-icons-05" },
                    { 4, "--notifications-received-color", "ItemReceived", "RequestNotificationStatus", "icon-centarix-icons-05" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ApplicationUserID",
                table: "Notifications",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationStatusID",
                table: "Notifications",
                column: "NotificationStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestNotifications_NotificationStatuses_NotificationStatusID",
                table: "RequestNotifications",
                column: "NotificationStatusID",
                principalTable: "NotificationStatuses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
