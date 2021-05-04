using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class LocationInstanceRemoveForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationInstances_LocationRoomTypes_LocationRoomTypeID",
                table: "LocationInstances");

            migrationBuilder.DropIndex(
                name: "IX_LocationInstances_LocationRoomTypeID",
                table: "LocationInstances");

            migrationBuilder.DropColumn(
                name: "LocationRoomTypeID",
                table: "LocationInstances");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationRoomTypeID",
                table: "LocationInstances",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 2,
                column: "LocationRoomTypeID",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 3,
                column: "LocationRoomTypeID",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 4,
                column: "LocationRoomTypeID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 5,
                column: "LocationRoomTypeID",
                value: 3);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 6,
                column: "LocationRoomTypeID",
                value: 4);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 7,
                column: "LocationRoomTypeID",
                value: 5);

            migrationBuilder.UpdateData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 8,
                column: "LocationRoomTypeID",
                value: 6);

            migrationBuilder.CreateIndex(
                name: "IX_LocationInstances_LocationRoomTypeID",
                table: "LocationInstances",
                column: "LocationRoomTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationInstances_LocationRoomTypes_LocationRoomTypeID",
                table: "LocationInstances",
                column: "LocationRoomTypeID",
                principalTable: "LocationRoomTypes",
                principalColumn: "LocationRoomTypeID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
