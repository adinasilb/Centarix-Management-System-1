using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class seededCustomDataTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CustomDataTypes",
                columns: new[] { "CustomDataTypeID", "Name" },
                values: new object[,]
                {
                    { 1, "String" },
                    { 2, "Double" },
                    { 3, "Bool" },
                    { 4, "DateTime" },
                    { 5, "File" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CustomDataTypes",
                keyColumn: "CustomDataTypeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CustomDataTypes",
                keyColumn: "CustomDataTypeID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CustomDataTypes",
                keyColumn: "CustomDataTypeID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CustomDataTypes",
                keyColumn: "CustomDataTypeID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CustomDataTypes",
                keyColumn: "CustomDataTypeID",
                keyValue: 5);
        }
    }
}
