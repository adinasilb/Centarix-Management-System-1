using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddTempResultText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProtocolInstanceResults");

            migrationBuilder.DropTable(
                name: "ReportSections");

            migrationBuilder.AddColumn<string>(
                name: "ResultDescription",
                table: "ProtocolInstances",
                type: "ntext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TemporaryResultDescription",
                table: "ProtocolInstances",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResultDescription",
                table: "ProtocolInstances");

            migrationBuilder.DropColumn(
                name: "TemporaryResultDescription",
                table: "ProtocolInstances");

            migrationBuilder.CreateTable(
                name: "ProtocolInstanceResults",
                columns: table => new
                {
                    ResultID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProtocolInstanceID = table.Column<int>(type: "int", nullable: false),
                    ResultDesciption = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtocolInstanceResults", x => x.ResultID);
                    table.ForeignKey(
                        name: "FK_ProtocolInstanceResults_ProtocolInstances_ProtocolInstanceID",
                        column: x => x.ProtocolInstanceID,
                        principalTable: "ProtocolInstances",
                        principalColumn: "ProtocolInstanceID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportSections",
                columns: table => new
                {
                    ReportSectionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportID = table.Column<int>(type: "int", nullable: false),
                    ReportSectionContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SectionNumber = table.Column<int>(type: "int", nullable: false),
                    ProtocolID = table.Column<int>(type: "int", nullable: true),
                    RequestID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportSections", x => x.ReportSectionID);
                    table.ForeignKey(
                        name: "FK_ReportSections_Protocols_ProtocolID",
                        column: x => x.ProtocolID,
                        principalTable: "Protocols",
                        principalColumn: "ProtocolID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportSections_Reports_ReportID",
                        column: x => x.ReportID,
                        principalTable: "Reports",
                        principalColumn: "ReportID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportSections_Requests_RequestID",
                        column: x => x.RequestID,
                        principalTable: "Requests",
                        principalColumn: "RequestID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolInstanceResults_ProtocolInstanceID",
                table: "ProtocolInstanceResults",
                column: "ProtocolInstanceID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportSections_ProtocolID",
                table: "ReportSections",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportSections_ReportID",
                table: "ReportSections",
                column: "ReportID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportSections_RequestID",
                table: "ReportSections",
                column: "RequestID");
        }
    }
}
