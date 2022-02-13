using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RenameRecurrenceEndStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_RecurringOrderEndStatuses_RecurringOrderEndStatusID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_RecurringOrderEndStatusID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RecurringOrderEndStatusID",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "RecurrenceEndStatusID",
                table: "Products",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "RecurringOrderEndStatuses",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Description", "DescriptionEnum" },
                values: new object[] { "No End", "NoEnd" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_RecurrenceEndStatusID",
                table: "Products",
                column: "RecurrenceEndStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_RecurringOrderEndStatuses_RecurrenceEndStatusID",
                table: "Products",
                column: "RecurrenceEndStatusID",
                principalTable: "RecurringOrderEndStatuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_RecurringOrderEndStatuses_RecurrenceEndStatusID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_RecurrenceEndStatusID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RecurrenceEndStatusID",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "RecurringOrderEndStatusID",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "RecurringOrderEndStatuses",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Description", "DescriptionEnum" },
                values: new object[] { "Never", "Never" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_RecurringOrderEndStatusID",
                table: "Products",
                column: "RecurringOrderEndStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_RecurringOrderEndStatuses_RecurringOrderEndStatusID",
                table: "Products",
                column: "RecurringOrderEndStatusID",
                principalTable: "RecurringOrderEndStatuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
