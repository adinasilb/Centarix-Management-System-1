using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class FixLocationTypeSection2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 502,
                column: "LocationTypeNameAbbre",
                value: "S");
            migrationBuilder.Sql("Update li"+
                    "SET LocationInstanceAbbrev = lt.LocationTypeNameAbbre+li.LocationInstanceAbbrev"+
                    "From LocationInstances li inner join LocationTypes lt"+
                    "ON lt.LocationTypeID = li.LocationTypeID"+
                    "WHERE li.LocationTypeID =502");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 502,
                column: "LocationTypeNameAbbre",
                value: "");
            migrationBuilder.Sql("Update LocationInstances"+
                "SET LocationInstanceAbbrev = LocationNumber"+
                "WHERE LocationTypeID =502");
        }
    }
}
