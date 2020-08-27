using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class seeded804 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LocationTypes",
                columns: new[] { "LocationTypeID", "Depth", "LocationTypeChildID", "LocationTypeName", "LocationTypeParentID" },
                values: new object[] { 203, 3, null, "Box", 202 });

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 202,
                column: "LocationTypeChildID",
                value: 203);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 203);

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 202,
                column: "LocationTypeChildID",
                value: null);

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 202,
                column: "LocationTypeChildID",
                value: null);
        }
    }
}
