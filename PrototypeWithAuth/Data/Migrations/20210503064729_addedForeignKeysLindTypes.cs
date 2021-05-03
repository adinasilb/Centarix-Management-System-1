using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedForeignKeysLindTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LineTypes",
                keyColumn: "LineTypeID",
                keyValue: 1,
                column: "LineTypeChildID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "LineTypes",
                keyColumn: "LineTypeID",
                keyValue: 2,
                columns: new[] { "LineTypeChildID", "LineTypeParentID" },
                values: new object[] { 3, 1 });

            migrationBuilder.UpdateData(
                table: "LineTypes",
                keyColumn: "LineTypeID",
                keyValue: 3,
                column: "LineTypeParentID",
                value: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LineTypes",
                keyColumn: "LineTypeID",
                keyValue: 1,
                column: "LineTypeChildID",
                value: null);

            migrationBuilder.UpdateData(
                table: "LineTypes",
                keyColumn: "LineTypeID",
                keyValue: 2,
                columns: new[] { "LineTypeChildID", "LineTypeParentID" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "LineTypes",
                keyColumn: "LineTypeID",
                keyValue: 3,
                column: "LineTypeParentID",
                value: null);
        }
    }
}
