using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddNotificationDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NotificationDate",
                table: "TimekeeperNotifications",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.Sql("UPDATE TimekeeperNotifications SET NotificationDate = Timestamp");

            migrationBuilder.AddColumn<DateTime>(
                name: "NotificationDate",
                table: "RequestNotifications",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.Sql("UPDATE RequestNotifications SET NotificationDate = Timestamp");

            migrationBuilder.AddColumn<DateTime>(
                name: "NotificationDate",
                table: "EmployeeInfoNotifications",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
           
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationDate",
                table: "TimekeeperNotifications");

            migrationBuilder.DropColumn(
                name: "NotificationDate",
                table: "RequestNotifications");

            migrationBuilder.DropColumn(
                name: "NotificationDate",
                table: "EmployeeInfoNotifications");
        }
    }
}
