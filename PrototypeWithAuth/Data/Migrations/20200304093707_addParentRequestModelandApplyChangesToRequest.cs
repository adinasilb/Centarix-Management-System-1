using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addParentRequestModelandApplyChangesToRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_ApplicationUserID",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ApplicationUserID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ApplicationUserID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "InvoiceDate",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "Requests");

            migrationBuilder.AddColumn<int>(
                name: "ParentRequestID",
                table: "Requests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ParentRequests",
                columns: table => new
                {
                    ParentRequestID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserID = table.Column<string>(nullable: true),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    OrderNumber = table.Column<int>(nullable: true),
                    InvoiceNumber = table.Column<string>(nullable: true),
                    InvoiceDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentRequests", x => x.ParentRequestID);
                    table.ForeignKey(
                        name: "FK_ParentRequests_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ParentRequestID",
                table: "Requests",
                column: "ParentRequestID");

            migrationBuilder.CreateIndex(
                name: "IX_ParentRequests_ApplicationUserID",
                table: "ParentRequests",
                column: "ApplicationUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_ParentRequests_ParentRequestID",
                table: "Requests",
                column: "ParentRequestID",
                principalTable: "ParentRequests",
                principalColumn: "ParentRequestID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_ParentRequests_ParentRequestID",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "ParentRequests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ParentRequestID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ParentRequestID",
                table: "Requests");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserID",
                table: "Requests",
                type: "nvarchar(450)",
                nullable: true);

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

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "Requests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                table: "Requests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ApplicationUserID",
                table: "Requests",
                column: "ApplicationUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_ApplicationUserID",
                table: "Requests",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
