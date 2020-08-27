using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class seeded801 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LocationTypes",
                columns: new[] { "LocationTypeID", "Depth", "LocationTypeChildID", "LocationTypeName", "LocationTypeParentID" },
                values: new object[] { 200, 0, null, "-80°C", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 200);
        }
    }
}
