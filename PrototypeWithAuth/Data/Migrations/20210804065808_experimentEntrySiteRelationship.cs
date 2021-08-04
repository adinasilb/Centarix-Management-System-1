using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class experimentEntrySiteRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SiteID",
                table: "ExperimentEntries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentEntries_SiteID",
                table: "ExperimentEntries",
                column: "SiteID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExperimentEntries_Sites_SiteID",
                table: "ExperimentEntries",
                column: "SiteID",
                principalTable: "Sites",
                principalColumn: "SiteID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExperimentEntries_Sites_SiteID",
                table: "ExperimentEntries");

            migrationBuilder.DropIndex(
                name: "IX_ExperimentEntries_SiteID",
                table: "ExperimentEntries");

            migrationBuilder.DropColumn(
                name: "SiteID",
                table: "ExperimentEntries");
        }
    }
}
