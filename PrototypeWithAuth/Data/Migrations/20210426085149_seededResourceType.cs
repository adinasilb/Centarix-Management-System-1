using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class seededResourceType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ResourceTypes",
                columns: new[] { "ResourceTypeId", "ResourceTypeDescription" },
                values: new object[] { 1, "Articles and Links" });

            migrationBuilder.InsertData(
                table: "ResourceTypes",
                columns: new[] { "ResourceTypeId", "ResourceTypeDescription" },
                values: new object[] { 2, "Resources" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ResourceTypes",
                keyColumn: "ResourceTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ResourceTypes",
                keyColumn: "ResourceTypeId",
                keyValue: 2);
        }
    }
}
