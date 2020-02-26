using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class seedRequestStatAndAddArrivalDateRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Warranty",
                table: "Requests",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<byte>(
                name: "ExpectedSupplyDays",
                table: "Requests",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalDate",
                table: "Requests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "RequestStatuses",
                columns: new[] { "RequestStatusID", "RequestStatusDescription" },
                values: new object[,]
                {
                    { 1, "New" },
                    { 2, "Ordered" },
                    { 3, "RecievedAndIsInventory" },
                    { 4, "Partial" },
                    { 5, "Clarify" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RequestStatuses",
                keyColumn: "RequestStatusID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RequestStatuses",
                keyColumn: "RequestStatusID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RequestStatuses",
                keyColumn: "RequestStatusID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RequestStatuses",
                keyColumn: "RequestStatusID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RequestStatuses",
                keyColumn: "RequestStatusID",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "ArrivalDate",
                table: "Requests");

            migrationBuilder.AlterColumn<int>(
                name: "Warranty",
                table: "Requests",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.AlterColumn<int>(
                name: "ExpectedSupplyDays",
                table: "Requests",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte));
        }
    }
}
