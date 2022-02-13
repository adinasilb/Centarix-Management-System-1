using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RecurringOrderEndStatusSeededData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RecurringOrderEndStatuses",
                columns: new[] { "ID", "Description", "DescriptionEnum" },
                values: new object[] { 1, "Never", "Never" });

            migrationBuilder.InsertData(
                table: "RecurringOrderEndStatuses",
                columns: new[] { "ID", "Description", "DescriptionEnum" },
                values: new object[] { 2, "End Date", "EndDate" });

            migrationBuilder.InsertData(
                table: "RecurringOrderEndStatuses",
                columns: new[] { "ID", "Description", "DescriptionEnum" },
                values: new object[] { 3, "Limited Occurrences", "LimitedOccurrences" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RecurringOrderEndStatuses",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RecurringOrderEndStatuses",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RecurringOrderEndStatuses",
                keyColumn: "ID",
                keyValue: 3);
        }
    }
}
