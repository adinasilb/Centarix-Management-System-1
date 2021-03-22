using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addTubesToUnitType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UnitTypes",
                columns: new[] { "UnitTypeID", "UnitParentTypeID", "UnitTypeDescription" },
                values: new object[] { 24, 1, "Tube" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 24);
        }
    }
}
