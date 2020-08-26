using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class renamed196 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 100,
                column: "LocationTypeName",
                value: "-196°C");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 100,
                column: "LocationTypeName",
                value: "-196");
        }
    }
}
