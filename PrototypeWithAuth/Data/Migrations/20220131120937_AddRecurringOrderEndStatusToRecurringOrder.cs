using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddRecurringOrderEndStatusToRecurringOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasEnd",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "RecurringOrderEndStatusID",
                table: "Products",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<bool>(
                name: "HasEnd",
                table: "Products",
                type: "bit",
                nullable: true);
        }
    }
}
