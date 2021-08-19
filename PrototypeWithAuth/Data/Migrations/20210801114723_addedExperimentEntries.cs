using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedExperimentEntries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestValues_Participants_ParticipantID",
                table: "TestValues");

            migrationBuilder.DropIndex(
                name: "IX_TestValues_ParticipantID",
                table: "TestValues");

            migrationBuilder.DropColumn(
                name: "ParticipantID",
                table: "TestValues");

            migrationBuilder.AddColumn<int>(
                name: "ExperimentEntryID",
                table: "TestValues",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SiteID",
                table: "Tests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ExperimentEntries",
                columns: table => new
                {
                    ExperimentEntryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(nullable: false),
                    ParticipantID = table.Column<int>(nullable: false),
                    VisitNumber = table.Column<int>(nullable: false),
                    ApplicationUserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentEntries", x => x.ExperimentEntryID);
                    table.ForeignKey(
                        name: "FK_ExperimentEntries_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExperimentEntries_Participants_ParticipantID",
                        column: x => x.ParticipantID,
                        principalTable: "Participants",
                        principalColumn: "ParticipantID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestValues_ExperimentEntryID",
                table: "TestValues",
                column: "ExperimentEntryID");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_SiteID",
                table: "Tests",
                column: "SiteID");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentEntries_ApplicationUserID",
                table: "ExperimentEntries",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentEntries_ParticipantID",
                table: "ExperimentEntries",
                column: "ParticipantID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Sites_SiteID",
                table: "Tests",
                column: "SiteID",
                principalTable: "Sites",
                principalColumn: "SiteID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestValues_ExperimentEntries_ExperimentEntryID",
                table: "TestValues",
                column: "ExperimentEntryID",
                principalTable: "ExperimentEntries",
                principalColumn: "ExperimentEntryID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Sites_SiteID",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_TestValues_ExperimentEntries_ExperimentEntryID",
                table: "TestValues");

            migrationBuilder.DropTable(
                name: "ExperimentEntries");

            migrationBuilder.DropIndex(
                name: "IX_TestValues_ExperimentEntryID",
                table: "TestValues");

            migrationBuilder.DropIndex(
                name: "IX_Tests_SiteID",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "ExperimentEntryID",
                table: "TestValues");

            migrationBuilder.DropColumn(
                name: "SiteID",
                table: "Tests");

            migrationBuilder.AddColumn<int>(
                name: "ParticipantID",
                table: "TestValues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TestValues_ParticipantID",
                table: "TestValues",
                column: "ParticipantID");

            migrationBuilder.AddForeignKey(
                name: "FK_TestValues_Participants_ParticipantID",
                table: "TestValues",
                column: "ParticipantID",
                principalTable: "Participants",
                principalColumn: "ParticipantID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
