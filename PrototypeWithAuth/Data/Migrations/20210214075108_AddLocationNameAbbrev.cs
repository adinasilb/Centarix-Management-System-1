using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddLocationNameAbbrev : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocationInstanceAbbrev",
                table: "LocationInstances",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LabPartNameAbbrev",
                table: "LabParts",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LabParts",
                keyColumn: "LabPartID",
                keyValue: 1,
                column: "LabPartNameAbbrev",
                value: "C");

            migrationBuilder.UpdateData(
                table: "LabParts",
                keyColumn: "LabPartID",
                keyValue: 2,
                column: "LabPartNameAbbrev",
                value: "G");

            migrationBuilder.UpdateData(
                table: "LabParts",
                keyColumn: "LabPartID",
                keyValue: 3,
                column: "LabPartNameAbbrev",
                value: "T");

            migrationBuilder.UpdateData(
                table: "LabParts",
                keyColumn: "LabPartID",
                keyValue: 4,
                column: "LabPartNameAbbrev",
                value: "D");

            migrationBuilder.UpdateData(
                table: "LabParts",
                keyColumn: "LabPartID",
                keyValue: 5,
                column: "LabPartNameAbbrev",
                value: "S");

            migrationBuilder.UpdateData(
                table: "LocationRoomTypes",
                keyColumn: "LocationRoomTypeID",
                keyValue: 2,
                column: "LocationAbbreviation",
                value: "TC");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationInstanceAbbrev",
                table: "LocationInstances");

            migrationBuilder.DropColumn(
                name: "LabPartNameAbbrev",
                table: "LabParts");

            migrationBuilder.UpdateData(
                table: "LocationRoomTypes",
                keyColumn: "LocationRoomTypeID",
                keyValue: 2,
                column: "LocationAbbreviation",
                value: "");
        }
    }
}
