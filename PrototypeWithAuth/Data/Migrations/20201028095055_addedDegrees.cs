using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedDegrees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Degrees",
                columns: new[] { "DegreeID", "Description" },
                values: new object[] { 5, "No Degree" });

            migrationBuilder.InsertData(
                table: "Degrees",
                columns: new[] { "DegreeID", "Description" },
                values: new object[] { 6, "Certificate" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Degrees",
                keyColumn: "DegreeID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Degrees",
                keyColumn: "DegreeID",
                keyValue: 6);
        }
    }
}
