using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class FixSomeDBWordIssues2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 19);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UnitTypes",
                columns: new[] { "UnitTypeID", "UnitParentTypeID", "UnitTypeDescription" },
                values: new object[] { 19, 1, "Case" });
        }
    }
}
