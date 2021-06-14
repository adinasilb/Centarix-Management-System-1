using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MoveTextIdToChildrenModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FunctionLines_Lines_TextID",
                table: "FunctionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_FunctionReports_Reports_TextID",
                table: "FunctionReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FunctionReports",
                table: "FunctionReports");

            migrationBuilder.DropIndex(
                name: "IX_FunctionReports_TextID",
                table: "FunctionReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FunctionLines",
                table: "FunctionLines");

            migrationBuilder.DropIndex(
                name: "IX_FunctionLines_TextID",
                table: "FunctionLines");

            migrationBuilder.DropColumn(
                name: "FunctionTextID",
                table: "FunctionReports");

            migrationBuilder.DropColumn(
                name: "TextID",
                table: "FunctionReports");

            migrationBuilder.DropColumn(
                name: "FunctionTextID",
                table: "FunctionLines");

            migrationBuilder.DropColumn(
                name: "TextID",
                table: "FunctionLines");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "FunctionReports",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ReportID",
                table: "FunctionReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "FunctionLines",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "LineID",
                table: "FunctionLines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FunctionReports",
                table: "FunctionReports",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FunctionLines",
                table: "FunctionLines",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionReports_ReportID",
                table: "FunctionReports",
                column: "ReportID");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionLines_LineID",
                table: "FunctionLines",
                column: "LineID");

            migrationBuilder.AddForeignKey(
                name: "FK_FunctionLines_Lines_LineID",
                table: "FunctionLines",
                column: "LineID",
                principalTable: "Lines",
                principalColumn: "LineID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FunctionReports_Reports_ReportID",
                table: "FunctionReports",
                column: "ReportID",
                principalTable: "Reports",
                principalColumn: "ReportID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FunctionLines_Lines_LineID",
                table: "FunctionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_FunctionReports_Reports_ReportID",
                table: "FunctionReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FunctionReports",
                table: "FunctionReports");

            migrationBuilder.DropIndex(
                name: "IX_FunctionReports_ReportID",
                table: "FunctionReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FunctionLines",
                table: "FunctionLines");

            migrationBuilder.DropIndex(
                name: "IX_FunctionLines_LineID",
                table: "FunctionLines");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "FunctionReports");

            migrationBuilder.DropColumn(
                name: "ReportID",
                table: "FunctionReports");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "FunctionLines");

            migrationBuilder.DropColumn(
                name: "LineID",
                table: "FunctionLines");

            migrationBuilder.AddColumn<int>(
                name: "FunctionTextID",
                table: "FunctionReports",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "TextID",
                table: "FunctionReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FunctionTextID",
                table: "FunctionLines",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "TextID",
                table: "FunctionLines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FunctionReports",
                table: "FunctionReports",
                column: "FunctionTextID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FunctionLines",
                table: "FunctionLines",
                column: "FunctionTextID");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionReports_TextID",
                table: "FunctionReports",
                column: "TextID");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionLines_TextID",
                table: "FunctionLines",
                column: "TextID");

            migrationBuilder.AddForeignKey(
                name: "FK_FunctionLines_Lines_TextID",
                table: "FunctionLines",
                column: "TextID",
                principalTable: "Lines",
                principalColumn: "LineID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FunctionReports_Reports_TextID",
                table: "FunctionReports",
                column: "TextID",
                principalTable: "Reports",
                principalColumn: "ReportID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
