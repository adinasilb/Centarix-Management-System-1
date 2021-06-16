using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedShareProtocols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShareProtocols",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromApplicationUserID = table.Column<string>(nullable: true),
                    ToApplicationUserID = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    ProtocolID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareProtocols", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ShareProtocols_AspNetUsers_FromApplicationUserID",
                        column: x => x.FromApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareProtocols_Protocols_ProtocolID",
                        column: x => x.ProtocolID,
                        principalTable: "Protocols",
                        principalColumn: "ProtocolID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareProtocols_AspNetUsers_ToApplicationUserID",
                        column: x => x.ToApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShareProtocols_FromApplicationUserID",
                table: "ShareProtocols",
                column: "FromApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareProtocols_ProtocolID",
                table: "ShareProtocols",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareProtocols_ToApplicationUserID",
                table: "ShareProtocols",
                column: "ToApplicationUserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShareProtocols");
        }
    }
}
