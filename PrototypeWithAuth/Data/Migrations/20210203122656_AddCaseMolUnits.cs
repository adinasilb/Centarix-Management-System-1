using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddCaseMolUnits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UnitTypes",
                columns: new[] { "UnitTypeID", "UnitParentTypeID", "UnitTypeDescription" },
                values: new object[,]
                {
                    { 19, 1, "Case" },
                    { 20, 2, "pmol" },
                    { 21, 2, "nmol" },
                    { 22, 2, "umol" },
                    { 23, 2, "mol" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 23);
        }
    }
}
