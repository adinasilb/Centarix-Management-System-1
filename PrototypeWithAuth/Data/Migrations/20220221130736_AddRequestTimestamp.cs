using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddRequestTimestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "Requests",
                nullable: false,
                defaultValueSql: "getdate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Requests");
        }
    }
}
