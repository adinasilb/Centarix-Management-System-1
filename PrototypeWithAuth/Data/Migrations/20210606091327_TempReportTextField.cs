using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class TempReportTextField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReportDescription",
                table: "Reports",
                newName: "ReportText");

            migrationBuilder.AddColumn<string>(
                name: "TemporaryReportText",
                table: "Reports",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemporaryReportText",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "ReportText",
                table: "Reports",
                newName: "ReportDescription");
        }
    }
}
