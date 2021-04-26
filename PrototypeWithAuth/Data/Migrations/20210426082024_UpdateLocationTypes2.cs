using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class UpdateLocationTypes2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 503);

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 501,
                columns: new[] { "LocationTypeChildID", "LocationTypeName", "LocationTypePluralName" },
                values: new object[] { 502, "Lab Part", "Lab Parts" });

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 502,
                columns: new[] { "LocationTypeChildID", "LocationTypeName", "LocationTypeParentID", "LocationTypePluralName" },
                values: new object[] { null, "Section", 501, "Sections" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 501,
                columns: new[] { "LocationTypeChildID", "LocationTypeName", "LocationTypePluralName" },
                values: new object[] { null, "Location", "Locations" });

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 502,
                columns: new[] { "LocationTypeChildID", "LocationTypeName", "LocationTypeParentID", "LocationTypePluralName" },
                values: new object[] { 502, "Lab Part", 500, "Lab Parts" });

            migrationBuilder.InsertData(
                table: "LocationTypes",
                columns: new[] { "LocationTypeID", "Depth", "Limit", "LocationTypeChildID", "LocationTypeName", "LocationTypeNameAbbre", "LocationTypeParentID", "LocationTypePluralName" },
                values: new object[] { 503, 3, 0, null, "Section", "S", 501, "Sections" });
        }
    }
}
