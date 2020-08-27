using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class seeded42 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LocationTypes",
                columns: new[] { "LocationTypeID", "Depth", "LocationTypeChildID", "LocationTypeName", "LocationTypeParentID" },
                values: new object[] { 401, 1, null, "Shelf", 400 });

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 400,
                column: "LocationTypeChildID",
                value: 401);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 401);

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 400,
                column: "LocationTypeChildID",
                value: null);

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 400,
                column: "LocationTypeChildID",
                value: null);
        }
    }
}
