using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class DrawersCanHaveShelves : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LabParts",
                keyColumn: "LabPartID",
                keyValue: 4,
                column: "HasShelves",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LabParts",
                keyColumn: "LabPartID",
                keyValue: 4,
                column: "HasShelves",
                value: false);
        }
    }
}
