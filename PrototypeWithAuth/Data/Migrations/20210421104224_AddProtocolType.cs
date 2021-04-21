using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddProtocolType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProtocolTypes",
                columns: new[] { "ProtocolTypeID", "ProtocolTypeDescription" },
                values: new object[,]
                {
                    { 1, "Research" },
                    { 2, "Kit" },
                    { 3, "SOP" },
                    { 4, "Buffer" },
                    { 5, "Robiotic" },
                    { 6, "Maintenance" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProtocolTypes",
                keyColumn: "ProtocolTypeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProtocolTypes",
                keyColumn: "ProtocolTypeID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProtocolTypes",
                keyColumn: "ProtocolTypeID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProtocolTypes",
                keyColumn: "ProtocolTypeID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProtocolTypes",
                keyColumn: "ProtocolTypeID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProtocolTypes",
                keyColumn: "ProtocolTypeID",
                keyValue: 6);
        }
    }
}
