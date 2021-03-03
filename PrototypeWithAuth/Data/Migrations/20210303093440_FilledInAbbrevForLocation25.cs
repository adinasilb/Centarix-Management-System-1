using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class FilledInAbbrevForLocation25 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 2,
                column: "LocationInstanceAbbrev",
                value: "L1");

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 3,
                column: "LocationInstanceAbbrev",
                value: "L2");

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 4,
                column: "LocationInstanceAbbrev",
                value: "TC1");

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 5,
                column: "LocationInstanceAbbrev",
                value: "E1");

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 6,
                column: "LocationInstanceAbbrev",
                value: "R1");

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 7,
                column: "LocationInstanceAbbrev",
                value: "W1");

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 8,
                column: "LocationInstanceAbbrev",
                value: "S1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 2,
                column: "LocationInstanceAbbrev",
                value: null);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 3,
                column: "LocationInstanceAbbrev",
                value: null);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 4,
                column: "LocationInstanceAbbrev",
                value: null);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 5,
                column: "LocationInstanceAbbrev",
                value: null);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 6,
                column: "LocationInstanceAbbrev",
                value: null);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 7,
                column: "LocationInstanceAbbrev",
                value: null);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 8,
                column: "LocationInstanceAbbrev",
                value: null);
        }
    }
}
