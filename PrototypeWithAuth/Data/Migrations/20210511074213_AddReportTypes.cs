using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddReportTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReportTypeID",
                table: "Reports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "ReportTypes",
                columns: new[] { "ReportTypeID", "ReportTypeDescription" },
                values: new object[] { 1, "Daily" });

            migrationBuilder.InsertData(
                table: "ReportTypes",
                columns: new[] { "ReportTypeID", "ReportTypeDescription" },
                values: new object[] { 2, "Weekly" });

            migrationBuilder.InsertData(
                table: "ReportTypes",
                columns: new[] { "ReportTypeID", "ReportTypeDescription" },
                values: new object[] { 3, "Monthly" });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportTypeID",
                table: "Reports",
                column: "ReportTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_ReportTypes_ReportTypeID",
                table: "Reports",
                column: "ReportTypeID",
                principalTable: "ReportTypes",
                principalColumn: "ReportTypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_ReportTypes_ReportTypeID",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ReportTypeID",
                table: "Reports");

            migrationBuilder.DeleteData(
                table: "ReportTypes",
                keyColumn: "ReportTypeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ReportTypes",
                keyColumn: "ReportTypeID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ReportTypes",
                keyColumn: "ReportTypeID",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "ReportTypeID",
                table: "Reports");
        }
    }
}
