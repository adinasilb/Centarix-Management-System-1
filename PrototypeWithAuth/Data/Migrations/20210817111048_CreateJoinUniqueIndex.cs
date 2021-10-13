using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class CreateJoinUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Protocols_UniqueCode",
                table: "Protocols");

            migrationBuilder.AlterColumn<string>(
                name: "VersionNumber",
                table: "Protocols",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Protocols_UniqueCode_VersionNumber",
                table: "Protocols",
                columns: new[] { "UniqueCode", "VersionNumber" },
                unique: true,
                filter: "[UniqueCode] IS NOT NULL AND [VersionNumber] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Protocols_UniqueCode_VersionNumber",
                table: "Protocols");

            migrationBuilder.AlterColumn<string>(
                name: "VersionNumber",
                table: "Protocols",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Protocols_UniqueCode",
                table: "Protocols",
                column: "UniqueCode",
                unique: true,
                filter: "[UniqueCode] IS NOT NULL");
        }
    }
}
