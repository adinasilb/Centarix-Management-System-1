using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddReportCategoryField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReportCategoryID",
                table: "Reports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportCategoryID",
                table: "Reports",
                column: "ReportCategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_ResourceCategories_ReportCategoryID",
                table: "Reports",
                column: "ReportCategoryID",
                principalTable: "ResourceCategories",
                principalColumn: "ResourceCategoryID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_ResourceCategories_ReportCategoryID",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ReportCategoryID",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReportCategoryID",
                table: "Reports");
        }
    }
}
