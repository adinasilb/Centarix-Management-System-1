using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class Redid25And80Locations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 201,
                columns: new[] { "LocationTypeName", "LocationTypePluralName" },
                values: new object[] { "Floor", "Floors" });

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 203,
                columns: new[] { "LocationTypeName", "LocationTypePluralName" },
                values: new object[] { "Shelf", "Shelves" });

            migrationBuilder.InsertData(
                table: "LocationTypes",
                columns: new[] { "LocationTypeID", "Depth", "Limit", "LocationTypeChildID", "LocationTypeName", "LocationTypeParentID", "LocationTypePluralName" },
                values: new object[,]
                {
                    { 205, 5, 1, null, "Box Unit", 204, "Box Units" },
                    { 503, 3, 0, null, "Section", 501, "Sections" }
                });

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 204,
                columns: new[] { "Limit", "LocationTypeChildID", "LocationTypeName", "LocationTypePluralName" },
                values: new object[] { 0, 205, "Box", "Boxes" });

            migrationBuilder.InsertData(
                table: "LocationTypes",
                columns: new[] { "LocationTypeID", "Depth", "Limit", "LocationTypeChildID", "LocationTypeName", "LocationTypeParentID", "LocationTypePluralName" },
                values: new object[] { 502, 2, 0, 503, "Lab Part", 501, "Lab Part" });

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 501,
                columns: new[] { "LocationTypeChildID", "LocationTypeName", "LocationTypePluralName" },
                values: new object[] { 502, "Location", "Locations" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 502);

            migrationBuilder.DeleteData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 503);

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 204,
                column: "LocationTypeChildID",
                value: null);

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 501,
                column: "LocationTypeChildID",
                value: null);

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 201,
                columns: new[] { "LocationTypeName", "LocationTypePluralName" },
                values: new object[] { "Shelf", "Shelves" });

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 203,
                columns: new[] { "LocationTypeName", "LocationTypePluralName" },
                values: new object[] { "Box", "Boxes" });

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 204,
                columns: new[] { "Limit", "LocationTypeChildID", "LocationTypeName", "LocationTypePluralName" },
                values: new object[] { 1, null, "Box Unit", "Box Units" });

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 501,
                columns: new[] { "LocationTypeChildID", "LocationTypeName", "LocationTypePluralName" },
                values: new object[] { null, "Shelf", "Shelves" });
        }
    }
}
