using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedShareResources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShareResources",
                columns: table => new
                {
                    ShareResourceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceID = table.Column<int>(nullable: false),
                    FromApplicationUserID = table.Column<string>(nullable: true),
                    ToApplicationUserID = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareResources", x => x.ShareResourceID);
                    table.ForeignKey(
                        name: "FK_ShareResources_AspNetUsers_FromApplicationUserID",
                        column: x => x.FromApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareResources_Resources_ResourceID",
                        column: x => x.ResourceID,
                        principalTable: "Resources",
                        principalColumn: "ResourceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareResources_AspNetUsers_ToApplicationUserID",
                        column: x => x.ToApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShareResources_FromApplicationUserID",
                table: "ShareResources",
                column: "FromApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareResources_ResourceID",
                table: "ShareResources",
                column: "ResourceID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareResources_ToApplicationUserID",
                table: "ShareResources",
                column: "ToApplicationUserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShareResources");
        }
    }
}
