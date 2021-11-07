using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class DateCreatedToParitcipantEntries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Participants",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ExperimentEntries",
                nullable: false,
                defaultValueSql: "getdate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ExperimentEntries");
        }
    }
}
