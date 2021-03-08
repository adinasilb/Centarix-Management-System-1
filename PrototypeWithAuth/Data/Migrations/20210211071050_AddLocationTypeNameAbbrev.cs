using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddLocationTypeNameAbbrev : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationNumber",
                table: "LocationRoomInstances");

            migrationBuilder.AddColumn<string>(
                name: "LocationTypeNameAbbre",
                table: "LocationTypes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationNumber",
                table: "LocationInstances",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 101,
                column: "LocationTypeNameAbbre",
                value: "R");

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 102,
                column: "LocationTypeNameAbbre",
                value: "B");

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 103,
                column: "LocationTypeNameAbbre",
                value: "B");

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 201,
                column: "LocationTypeNameAbbre",
                value: "F");

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 202,
                column: "LocationTypeNameAbbre",
                value: "R");

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 203,
                column: "LocationTypeNameAbbre",
                value: "S");

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 204,
                column: "LocationTypeNameAbbre",
                value: "B");

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 205,
                column: "LocationTypeNameAbbre",
                value: "B");

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 301,
                column: "LocationTypeNameAbbre",
                value: "S");

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 401,
                column: "LocationTypeNameAbbre",
                value: "S");

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 502,
                column: "LocationTypePluralName",
                value: "Lab Parts");

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 503,
                column: "LocationTypeNameAbbre",
                value: "S");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationTypeNameAbbre",
                table: "LocationTypes");

            migrationBuilder.DropColumn(
                name: "LocationNumber",
                table: "LocationInstances");

            migrationBuilder.AddColumn<int>(
                name: "LocationNumber",
                table: "LocationRoomInstances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 1,
                column: "LocationNumber",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 2,
                column: "LocationNumber",
                value: 2);

            migrationBuilder.UpdateData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 3,
                column: "LocationNumber",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 4,
                column: "LocationNumber",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 5,
                column: "LocationNumber",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 6,
                column: "LocationNumber",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LocationRoomInstances",
                keyColumn: "LocationRoomInstanceID",
                keyValue: 7,
                column: "LocationNumber",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 502,
                column: "LocationTypePluralName",
                value: "Lab Part");
        }
    }
}
