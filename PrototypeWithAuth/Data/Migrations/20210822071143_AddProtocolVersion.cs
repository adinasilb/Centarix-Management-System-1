using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddProtocolVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProtocolVersionID",
                table: "ShareProtocols",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProtocolVersionID",
                table: "ProtocolInstances",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProtocolVersionID",
                table: "Materials",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProtocolVersionID",
                table: "Links",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProtocolVersionID",
                table: "Lines",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProtocolVersionID",
                table: "FavoriteProtocols",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProtocolVersions",
                columns: table => new
                {
                    ProtocolVersionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VersionNumber = table.Column<string>(nullable: true),
                    ShortDescription = table.Column<string>(nullable: true),
                    Theory = table.Column<string>(nullable: true),
                    ApplicationUserCreatorID = table.Column<string>(nullable: true),
                    ProtocolTypeID = table.Column<int>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ProtocolID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtocolVersions", x => x.ProtocolVersionID);
                    table.ForeignKey(
                        name: "FK_ProtocolVersions_AspNetUsers_ApplicationUserCreatorID",
                        column: x => x.ApplicationUserCreatorID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtocolVersions_Protocols_ProtocolID",
                        column: x => x.ProtocolID,
                        principalTable: "Protocols",
                        principalColumn: "ProtocolID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProtocolVersions_ProtocolTypes_ProtocolTypeID",
                        column: x => x.ProtocolTypeID,
                        principalTable: "ProtocolTypes",
                        principalColumn: "ProtocolTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShareProtocols_ProtocolVersionID",
                table: "ShareProtocols",
                column: "ProtocolVersionID");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolInstances_ProtocolVersionID",
                table: "ProtocolInstances",
                column: "ProtocolVersionID");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ProtocolVersionID",
                table: "Materials",
                column: "ProtocolVersionID");

            migrationBuilder.CreateIndex(
                name: "IX_Links_ProtocolVersionID",
                table: "Links",
                column: "ProtocolVersionID");

            migrationBuilder.CreateIndex(
                name: "IX_Lines_ProtocolVersionID",
                table: "Lines",
                column: "ProtocolVersionID");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProtocols_ProtocolVersionID",
                table: "FavoriteProtocols",
                column: "ProtocolVersionID");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolVersions_ApplicationUserCreatorID",
                table: "ProtocolVersions",
                column: "ApplicationUserCreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolVersions_ProtocolID",
                table: "ProtocolVersions",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolVersions_ProtocolTypeID",
                table: "ProtocolVersions",
                column: "ProtocolTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteProtocols_ProtocolVersions_ProtocolVersionID",
                table: "FavoriteProtocols",
                column: "ProtocolVersionID",
                principalTable: "ProtocolVersions",
                principalColumn: "ProtocolVersionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_ProtocolVersions_ProtocolVersionID",
                table: "Lines",
                column: "ProtocolVersionID",
                principalTable: "ProtocolVersions",
                principalColumn: "ProtocolVersionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Links_ProtocolVersions_ProtocolVersionID",
                table: "Links",
                column: "ProtocolVersionID",
                principalTable: "ProtocolVersions",
                principalColumn: "ProtocolVersionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_ProtocolVersions_ProtocolVersionID",
                table: "Materials",
                column: "ProtocolVersionID",
                principalTable: "ProtocolVersions",
                principalColumn: "ProtocolVersionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProtocolInstances_ProtocolVersions_ProtocolVersionID",
                table: "ProtocolInstances",
                column: "ProtocolVersionID",
                principalTable: "ProtocolVersions",
                principalColumn: "ProtocolVersionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareProtocols_ProtocolVersions_ProtocolVersionID",
                table: "ShareProtocols",
                column: "ProtocolVersionID",
                principalTable: "ProtocolVersions",
                principalColumn: "ProtocolVersionID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteProtocols_ProtocolVersions_ProtocolVersionID",
                table: "FavoriteProtocols");

            migrationBuilder.DropForeignKey(
                name: "FK_Lines_ProtocolVersions_ProtocolVersionID",
                table: "Lines");

            migrationBuilder.DropForeignKey(
                name: "FK_Links_ProtocolVersions_ProtocolVersionID",
                table: "Links");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_ProtocolVersions_ProtocolVersionID",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_ProtocolInstances_ProtocolVersions_ProtocolVersionID",
                table: "ProtocolInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareProtocols_ProtocolVersions_ProtocolVersionID",
                table: "ShareProtocols");

            migrationBuilder.DropTable(
                name: "ProtocolVersions");

            migrationBuilder.DropIndex(
                name: "IX_ShareProtocols_ProtocolVersionID",
                table: "ShareProtocols");

            migrationBuilder.DropIndex(
                name: "IX_ProtocolInstances_ProtocolVersionID",
                table: "ProtocolInstances");

            migrationBuilder.DropIndex(
                name: "IX_Materials_ProtocolVersionID",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Links_ProtocolVersionID",
                table: "Links");

            migrationBuilder.DropIndex(
                name: "IX_Lines_ProtocolVersionID",
                table: "Lines");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteProtocols_ProtocolVersionID",
                table: "FavoriteProtocols");

            migrationBuilder.DropColumn(
                name: "ProtocolVersionID",
                table: "ShareProtocols");

            migrationBuilder.DropColumn(
                name: "ProtocolVersionID",
                table: "ProtocolInstances");

            migrationBuilder.DropColumn(
                name: "ProtocolVersionID",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ProtocolVersionID",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "ProtocolVersionID",
                table: "Lines");

            migrationBuilder.DropColumn(
                name: "ProtocolVersionID",
                table: "FavoriteProtocols");
        }
    }
}
