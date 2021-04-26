using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class StartedRedoLocationsAgain4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DeleteData(
            //    table: "LocationTypes",
            //    keyColumn: "LocationTypeID",
            //    keyValue: 503);

            //migrationBuilder.UpdateData(
            //    table: "LocationTypes",
            //    keyColumn: "LocationTypeID",
            //    keyValue: 501,
            //    columns: new[] { "Depth", "LocationTypeName", "LocationTypePluralName" },
            //    values: new object[] { 2, "Lab Part", "Lab Parts" });

            //migrationBuilder.UpdateData(
            //    table: "LocationTypes",
            //    keyColumn: "LocationTypeID",
            //    keyValue: 502,
            //    columns: new[] { "Depth", "LocationTypeName", "LocationTypeNameAbbre", "LocationTypePluralName" },
            //    values: new object[] { 3, "Section", "S", "Sections" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.UpdateData(
            //    table: "LocationTypes",
            //    keyColumn: "LocationTypeID",
            //    keyValue: 501,
            //    columns: new[] { "Depth", "LocationTypeName", "LocationTypePluralName" },
            //    values: new object[] { 1, "Location", "Locations" });

            //migrationBuilder.UpdateData(
            //    table: "LocationTypes",
            //    keyColumn: "LocationTypeID",
            //    keyValue: 502,
            //    columns: new[] { "Depth", "LocationTypeName", "LocationTypeNameAbbre", "LocationTypePluralName" },
            //    values: new object[] { 2, "Lab Part", null, "Lab Parts" });

            //migrationBuilder.InsertData(
            //    table: "LocationTypes",
            //    columns: new[] { "LocationTypeID", "Depth", "Limit", "LocationTypeChildID", "LocationTypeName", "LocationTypeNameAbbre", "LocationTypeParentID", "LocationTypePluralName" },
            //    values: new object[] { 503, 3, 0, null, "Section", "S", 502, "Sections" });
        }
    }
}
