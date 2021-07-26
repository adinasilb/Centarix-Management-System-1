using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddFunctionReportTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FunctionReports",
                columns: table => new
                {
                    FunctionReportID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportID = table.Column<int>(nullable: false),
                    FunctionTypeID = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    ProtocolID = table.Column<int>(nullable: true),
                    ProductID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionReports", x => x.FunctionReportID);
                    table.ForeignKey(
                        name: "FK_FunctionReports_FunctionTypes_FunctionTypeID",
                        column: x => x.FunctionTypeID,
                        principalTable: "FunctionTypes",
                        principalColumn: "FunctionTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FunctionReports_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FunctionReports_Protocols_ProtocolID",
                        column: x => x.ProtocolID,
                        principalTable: "Protocols",
                        principalColumn: "ProtocolID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FunctionReports_Reports_ReportID",
                        column: x => x.ReportID,
                        principalTable: "Reports",
                        principalColumn: "ReportID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FunctionReports_FunctionTypeID",
                table: "FunctionReports",
                column: "FunctionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionReports_ProductID",
                table: "FunctionReports",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionReports_ProtocolID",
                table: "FunctionReports",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionReports_ReportID",
                table: "FunctionReports",
                column: "ReportID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FunctionReports");
        }
    }
}
