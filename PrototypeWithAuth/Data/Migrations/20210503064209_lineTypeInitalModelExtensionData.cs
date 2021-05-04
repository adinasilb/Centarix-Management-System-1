using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class lineTypeInitalModelExtensionData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LineTypes",
                columns: new[] { "LineTypeID", "LineTypeChildID", "LineTypeDescription", "LineTypeParentID" },
                values: new object[] { 1, null, "Header", null });

            migrationBuilder.InsertData(
                table: "LineTypes",
                columns: new[] { "LineTypeID", "LineTypeChildID", "LineTypeDescription", "LineTypeParentID" },
                values: new object[] { 2, null, "Sub Header", null });

            migrationBuilder.InsertData(
                table: "LineTypes",
                columns: new[] { "LineTypeID", "LineTypeChildID", "LineTypeDescription", "LineTypeParentID" },
                values: new object[] { 3, null, "Step", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LineTypes",
                keyColumn: "LineTypeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LineTypes",
                keyColumn: "LineTypeID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LineTypes",
                keyColumn: "LineTypeID",
                keyValue: 3);
        }
    }
}
