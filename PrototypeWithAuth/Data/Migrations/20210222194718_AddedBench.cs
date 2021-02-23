using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedBench : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LabParts",
                columns: new[] { "LabPartID", "HasShelves", "LabPartName", "LabPartNameAbbrev" },
                values: new object[] { 6, false, "Bench", "B" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LabParts",
                keyColumn: "LabPartID",
                keyValue: 6);
        }
    }
}
