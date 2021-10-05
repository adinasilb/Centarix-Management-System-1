using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RemovedProtocolTypeFromVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProtocolVersions_ProtocolTypes_ProtocolTypeID",
                table: "ProtocolVersions");

            migrationBuilder.DropIndex(
                name: "IX_ProtocolVersions_ProtocolTypeID",
                table: "ProtocolVersions");

            migrationBuilder.DropColumn(
                name: "ProtocolTypeID",
                table: "ProtocolVersions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProtocolTypeID",
                table: "ProtocolVersions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolVersions_ProtocolTypeID",
                table: "ProtocolVersions",
                column: "ProtocolTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProtocolVersions_ProtocolTypes_ProtocolTypeID",
                table: "ProtocolVersions",
                column: "ProtocolTypeID",
                principalTable: "ProtocolTypes",
                principalColumn: "ProtocolTypeID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
