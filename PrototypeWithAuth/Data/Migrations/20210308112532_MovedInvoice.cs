using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MovedInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceDate",
                table: "ParentRequests");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "ParentRequests");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentReferenceDate",
                table: "Payments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentReferenceDate",
                table: "Payments");

            migrationBuilder.AddColumn<DateTime>(
                name: "InvoiceDate",
                table: "ParentRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                table: "ParentRequests",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
