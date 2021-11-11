using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RenameRequestList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListRequest");

            migrationBuilder.DropTable(
                name: "Lists");

            migrationBuilder.CreateTable(
                name: "RequestLists",
                columns: table => new
                {
                    ListID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    ApplicationUserOwnerID = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLists", x => x.ListID);
                    table.ForeignKey(
                        name: "FK_RequestLists_AspNetUsers_ApplicationUserOwnerID",
                        column: x => x.ApplicationUserOwnerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestListRequest",
                columns: table => new
                {
                    ListID = table.Column<int>(nullable: false),
                    RequestID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestListRequest", x => new { x.ListID, x.RequestID });
                    table.ForeignKey(
                        name: "FK_RequestListRequest_RequestLists_ListID",
                        column: x => x.ListID,
                        principalTable: "RequestLists",
                        principalColumn: "ListID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestListRequest_Requests_RequestID",
                        column: x => x.RequestID,
                        principalTable: "Requests",
                        principalColumn: "RequestID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestListRequest_RequestID",
                table: "RequestListRequest",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestLists_ApplicationUserOwnerID",
                table: "RequestLists",
                column: "ApplicationUserOwnerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestListRequest");

            migrationBuilder.DropTable(
                name: "RequestLists");

            migrationBuilder.CreateTable(
                name: "Lists",
                columns: table => new
                {
                    ListID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserOwnerID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lists", x => x.ListID);
                    table.ForeignKey(
                        name: "FK_Lists_AspNetUsers_ApplicationUserOwnerID",
                        column: x => x.ApplicationUserOwnerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ListRequest",
                columns: table => new
                {
                    ListID = table.Column<int>(type: "int", nullable: false),
                    RequestID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListRequest", x => new { x.ListID, x.RequestID });
                    table.ForeignKey(
                        name: "FK_ListRequest_Lists_ListID",
                        column: x => x.ListID,
                        principalTable: "Lists",
                        principalColumn: "ListID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ListRequest_Requests_RequestID",
                        column: x => x.RequestID,
                        principalTable: "Requests",
                        principalColumn: "RequestID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListRequest_RequestID",
                table: "ListRequest",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_Lists_ApplicationUserOwnerID",
                table: "Lists",
                column: "ApplicationUserOwnerID");
        }
    }
}
