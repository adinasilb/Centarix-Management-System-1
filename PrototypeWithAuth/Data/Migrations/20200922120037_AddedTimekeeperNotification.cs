using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedTimekeeperNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimekeeperNotifications",
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
                    NotificationStatusID = table.Column<int>(nullable: false),
                    EmployeeHoursID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimekeeperNotifications", x => x.NotificationID);
                    table.ForeignKey(
                        name: "FK_TimekeeperNotifications_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimekeeperNotifications_NotificationStatuses_NotificationStatusID",
                        column: x => x.NotificationStatusID,
                        principalTable: "NotificationStatuses",
                        principalColumn: "NotificationStatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "NotificationStatuses",
                keyColumn: "NotificationStatusID",
                keyValue: 2,
                column: "Icon",
                value: "icon-centarix-icons-03");

            migrationBuilder.InsertData(
                table: "NotificationStatuses",
                columns: new[] { "NotificationStatusID", "Color", "Description", "Discriminator", "Icon" },
                values: new object[] { 5, "--timekeeper-color", "UpdateHours", "TimekeeperNotificationStatus", "icon-access_time-24px" });

            migrationBuilder.CreateIndex(
                name: "IX_TimekeeperNotifications_ApplicationUserID",
                table: "TimekeeperNotifications",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_TimekeeperNotifications_NotificationStatusID",
                table: "TimekeeperNotifications",
                column: "NotificationStatusID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimekeeperNotifications");

            migrationBuilder.DeleteData(
                table: "NotificationStatuses",
                keyColumn: "NotificationStatusID",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "NotificationStatuses",
                keyColumn: "NotificationStatusID",
                keyValue: 2,
                column: "Icon",
                value: "icon-centarix-icons-05");
        }
    }
}
