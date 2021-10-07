using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class UpdateRoomInstancesDS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LocationRoomTypes",
                columns: new[] { "LocationRoomTypeID", "LocationAbbreviation", "LocationRoomTypeDescription" },
                values: new object[] { 7, "LN", "Liquid Nitrogen Room" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocationRoomTypes",
                keyColumn: "LocationRoomTypeID",
                keyValue: 7);
        }
    }
}
