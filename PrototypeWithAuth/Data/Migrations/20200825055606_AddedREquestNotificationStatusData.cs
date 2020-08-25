using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedREquestNotificationStatusData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
