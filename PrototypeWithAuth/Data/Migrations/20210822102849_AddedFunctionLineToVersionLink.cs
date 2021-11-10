using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedFunctionLineToVersionLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProtocolVersions_ProtocolID",
                table: "ProtocolVersions");

            migrationBuilder.DropIndex(
                name: "IX_Protocols_UniqueCode_VersionNumber",
                table: "Protocols");

            migrationBuilder.AlterColumn<string>(
                name: "VersionNumber",
                table: "ProtocolVersions",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VersionNumber",
                table: "Protocols",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProtocolVersionID",
                table: "FunctionResults",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProtocolVersionID",
                table: "FunctionReports",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProtocolVersionID",
                table: "FunctionLines",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolVersions_ProtocolID_VersionNumber",
                table: "ProtocolVersions",
                columns: new[] { "ProtocolID", "VersionNumber" },
                unique: true,
                filter: "[VersionNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Protocols_UniqueCode",
                table: "Protocols",
                column: "UniqueCode",
                unique: true,
                filter: "[UniqueCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionResults_ProtocolVersionID",
                table: "FunctionResults",
                column: "ProtocolVersionID");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionReports_ProtocolVersionID",
                table: "FunctionReports",
                column: "ProtocolVersionID");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionLines_ProtocolVersionID",
                table: "FunctionLines",
                column: "ProtocolVersionID");

            migrationBuilder.AddForeignKey(
                name: "FK_FunctionLines_ProtocolVersions_ProtocolVersionID",
                table: "FunctionLines",
                column: "ProtocolVersionID",
                principalTable: "ProtocolVersions",
                principalColumn: "ProtocolVersionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FunctionReports_ProtocolVersions_ProtocolVersionID",
                table: "FunctionReports",
                column: "ProtocolVersionID",
                principalTable: "ProtocolVersions",
                principalColumn: "ProtocolVersionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FunctionResults_ProtocolVersions_ProtocolVersionID",
                table: "FunctionResults",
                column: "ProtocolVersionID",
                principalTable: "ProtocolVersions",
                principalColumn: "ProtocolVersionID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FunctionLines_ProtocolVersions_ProtocolVersionID",
                table: "FunctionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_FunctionReports_ProtocolVersions_ProtocolVersionID",
                table: "FunctionReports");

            migrationBuilder.DropForeignKey(
                name: "FK_FunctionResults_ProtocolVersions_ProtocolVersionID",
                table: "FunctionResults");

            migrationBuilder.DropIndex(
                name: "IX_ProtocolVersions_ProtocolID_VersionNumber",
                table: "ProtocolVersions");

            migrationBuilder.DropIndex(
                name: "IX_Protocols_UniqueCode",
                table: "Protocols");

            migrationBuilder.DropIndex(
                name: "IX_FunctionResults_ProtocolVersionID",
                table: "FunctionResults");

            migrationBuilder.DropIndex(
                name: "IX_FunctionReports_ProtocolVersionID",
                table: "FunctionReports");

            migrationBuilder.DropIndex(
                name: "IX_FunctionLines_ProtocolVersionID",
                table: "FunctionLines");

            migrationBuilder.DropColumn(
                name: "ProtocolVersionID",
                table: "FunctionResults");

            migrationBuilder.DropColumn(
                name: "ProtocolVersionID",
                table: "FunctionReports");

            migrationBuilder.DropColumn(
                name: "ProtocolVersionID",
                table: "FunctionLines");

            migrationBuilder.AlterColumn<string>(
                name: "VersionNumber",
                table: "ProtocolVersions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VersionNumber",
                table: "Protocols",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolVersions_ProtocolID",
                table: "ProtocolVersions",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_Protocols_UniqueCode_VersionNumber",
                table: "Protocols",
                columns: new[] { "UniqueCode", "VersionNumber" },
                unique: true,
                filter: "[UniqueCode] IS NOT NULL AND [VersionNumber] IS NOT NULL");
        }
    }
}
