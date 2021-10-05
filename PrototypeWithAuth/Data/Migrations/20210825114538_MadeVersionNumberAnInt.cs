using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MadeVersionNumberAnInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProtocolVersions_ProtocolID_VersionNumber",
                table: "ProtocolVersions");

            migrationBuilder.AlterColumn<int>(
                name: "VersionNumber",
                table: "ProtocolVersions",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolVersions_ProtocolID_VersionNumber",
                table: "ProtocolVersions",
                columns: new[] { "ProtocolID", "VersionNumber" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProtocolVersions_ProtocolID_VersionNumber",
                table: "ProtocolVersions");

            migrationBuilder.AlterColumn<string>(
                name: "VersionNumber",
                table: "ProtocolVersions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolVersions_ProtocolID_VersionNumber",
                table: "ProtocolVersions",
                columns: new[] { "ProtocolID", "VersionNumber" },
                unique: true,
                filter: "[VersionNumber] IS NOT NULL");
        }
    }
}
