using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddReportSectionTypeModal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportSections",
                columns: table => new
                {
                    ReportSectionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportSectionContent = table.Column<string>(nullable: true),
                    ReportID = table.Column<int>(nullable: false),
                    SectionNumber = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    ProtocolID = table.Column<int>(nullable: true),
                    RequestID = table.Column<int>(nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportSections");
        }
    }
}
