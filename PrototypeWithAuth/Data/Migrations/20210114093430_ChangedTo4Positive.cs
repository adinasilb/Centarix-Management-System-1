using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ChangedTo4Positive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 400,
                columns: new[] { "LocationTypeName", "LocationTypePluralName" },
                values: new object[] { "4°C", "4°C" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LocationTypes",
                keyColumn: "LocationTypeID",
                keyValue: 400,
                columns: new[] { "LocationTypeName", "LocationTypePluralName" },
                values: new object[] { "-4°C", "-4°C" });
        }
    }
}
