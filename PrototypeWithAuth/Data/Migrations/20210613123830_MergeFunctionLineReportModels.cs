using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MergeFunctionLineReportModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "FunctionReportID",
                table: "FunctionReports");

            migrationBuilder.DropColumn(
                name: "ReportID",
                table: "FunctionReports");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "FunctionReports");

            migrationBuilder.DropColumn(
                name: "FunctionLineID",
                table: "FunctionLines");

            migrationBuilder.DropColumn(
                name: "LineID",
                table: "FunctionLines");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "FunctionLines");

            migrationBuilder.AddColumn<int>(
                name: "FunctionTextID",
                table: "FunctionReports",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "IsTemporaryDeleted",
                table: "FunctionReports",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TextID",
                table: "FunctionReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FunctionTextID",
                table: "FunctionLines",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FunctionLines",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TextID",
                table: "FunctionLines",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "IsTemporaryDeleted",
                table: "FunctionReports");

            migrationBuilder.DropColumn(
                name: "TextID",
                table: "FunctionReports");

            migrationBuilder.DropColumn(
                name: "FunctionTextID",
                table: "FunctionLines");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "FunctionLines");

            migrationBuilder.DropColumn(
                name: "TextID",
                table: "FunctionLines");

            migrationBuilder.AddColumn<int>(
                name: "FunctionReportID",
                table: "FunctionReports",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ReportID",
                table: "FunctionReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "FunctionReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FunctionLineID",
                table: "FunctionLines",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "LineID",
                table: "FunctionLines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "FunctionLines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FunctionReports",
                table: "FunctionReports",
                column: "FunctionReportID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FunctionLines",
                table: "FunctionLines",
                column: "FunctionLineID");

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
    }
}
