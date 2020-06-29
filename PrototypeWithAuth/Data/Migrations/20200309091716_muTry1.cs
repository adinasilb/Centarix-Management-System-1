using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class muTry1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 10,
                column: "UnitTypeDescription",
                value: "956g");

            migrationBuilder.UpdateData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 13,
                column: "UnitTypeDescription",
                value: "956l");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 10,
                column: "UnitTypeDescription",
                value: "�g");

            migrationBuilder.UpdateData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 13,
                column: "UnitTypeDescription",
                value: "�l");
        }
    }
}
