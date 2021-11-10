using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddBiomarkerLab : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LocationRoomInstances",
                columns: new[] { "LocationRoomInstanceID", "LocationRoomInstanceAbbrev", "LocationRoomInstanceName", "LocationRoomTypeID" },
                values: new object[] { 13, "BL5", "Biomarker Lab 5", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 13);
        }
    }
}
