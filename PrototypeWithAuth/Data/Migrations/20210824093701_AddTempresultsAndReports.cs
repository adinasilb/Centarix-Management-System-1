using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddTempresultsAndReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TempReportJsons",
                columns: table => new
                {
                    TempReportJsonID = table.Column<Guid>(nullable: false),
                    Json = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempReportJsons", x => x.TempReportJsonID);
                });

            migrationBuilder.CreateTable(
                name: "TempResultsJsons",
                columns: table => new
                {
                    TempResultsJsonID = table.Column<Guid>(nullable: false),
                    Json = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempResultsJsons", x => x.TempResultsJsonID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TempReportJsons");

            migrationBuilder.DropTable(
                name: "TempResultsJsons");
        }
    }
}
