using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class FixedMistake : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 1,
                column: "LocationRoomTypeID",
                value: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 1,
                column: "LocationRoomTypeID",
                value: 1);
        }
    }
}
