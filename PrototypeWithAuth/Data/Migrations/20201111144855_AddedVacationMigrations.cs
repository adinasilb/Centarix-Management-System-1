using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedVacationMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyDayOffTypes",
                columns: table => new
                {
                    CompanyDayOffTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyDayOffTypes", x => x.CompanyDayOffTypeID);
                });

            migrationBuilder.CreateTable(
                name: "CompanyDayOffs",
                columns: table => new
                {
                    CompanyDayOffID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    CompanyDayOffTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyDayOffs", x => x.CompanyDayOffID);
                    table.ForeignKey(
                        name: "FK_CompanyDayOffs_CompanyDayOffTypes_CompanyDayOffTypeID",
                        column: x => x.CompanyDayOffTypeID,
                        principalTable: "CompanyDayOffTypes",
                        principalColumn: "CompanyDayOffTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CompanyDayOffTypes",
                columns: new[] { "CompanyDayOffTypeID", "Name" },
                values: new object[,]
                {
                    { 1, "Purim 1" },
                    { 16, "Sukkot" },
                    { 15, "Erev Sukkot" },
                    { 14, "Yom Kippur" },
                    { 13, "Erev Yom Kippur" },
                    { 12, "Rosh Hashana 2" },
                    { 11, "Rosh Hashana 1" },
                    { 10, "Erev Rosh Hashana" },
                    { 9, "Shavuous" },
                    { 8, "Erev Shavuous" },
                    { 7, "Yom Hazmaut" },
                    { 6, "Shviei Pesach" },
                    { 5, "Erev Shviei Pesach" },
                    { 4, "Pesach" },
                    { 3, "Erev Pesach" },
                    { 2, "Purim 2" },
                    { 17, "Erev Simchat Torah" },
                    { 18, "Simchat Torah" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDayOffs_CompanyDayOffTypeID",
                table: "CompanyDayOffs",
                column: "CompanyDayOffTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyDayOffs");

            migrationBuilder.DropTable(
                name: "CompanyDayOffTypes");
        }
    }
}
