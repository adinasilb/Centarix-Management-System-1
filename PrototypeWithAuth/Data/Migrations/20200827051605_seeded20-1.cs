using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class seeded201 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LocationTypes",
                columns: new[] { "LocationTypeID", "Depth", "LocationTypeChildID", "LocationTypeName", "LocationTypeParentID" },
                values: new object[] { 300, 0, null, "-20°C", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 300);
        }
    }
}
