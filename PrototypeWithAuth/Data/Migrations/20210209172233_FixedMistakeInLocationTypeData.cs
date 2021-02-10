using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class FixedMistakeInLocationTypeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 503,
                column: "LocationTypeParentID",
                value: 502);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 503,
                column: "LocationTypeParentID",
                value: 501);
        }
    }
}
