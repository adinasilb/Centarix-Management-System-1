using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ChangedInvoiceFromRequestToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceDate",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "Requests");

            migrationBuilder.AddColumn<int>(
                name: "InvoiceID",
                table: "Requests",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    InvoiceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<string>(nullable: true),
                    InvoiceDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.InvoiceID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_InvoiceID",
                table: "Requests",
                column: "InvoiceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Invoice_InvoiceID",
                table: "Requests",
                column: "InvoiceID",
                principalTable: "Invoice",
                principalColumn: "InvoiceID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Invoice_InvoiceID",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Requests_InvoiceID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "InvoiceID",
                table: "Requests");

            migrationBuilder.AddColumn<DateTime>(
                name: "InvoiceDate",
                table: "Requests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
