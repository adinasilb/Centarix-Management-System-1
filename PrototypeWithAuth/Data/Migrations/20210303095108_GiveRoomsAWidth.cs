using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class GiveRoomsAWidth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 2,
                column: "Width",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 3,
                column: "Width",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 4,
                column: "Width",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 5,
                column: "Width",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 6,
                column: "Width",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 7,
                column: "Width",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 8,
                column: "Width",
                value: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 2,
                column: "Width",
                value: 0);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 3,
                column: "Width",
                value: 0);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 4,
                column: "Width",
                value: 0);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 5,
                column: "Width",
                value: 0);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 6,
                column: "Width",
                value: 0);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 7,
                column: "Width",
                value: 0);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 8,
                column: "Width",
                value: 0);
        }
    }
}
