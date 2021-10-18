using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddTempIDsTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FunctionLines",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    FunctionTypeID = table.Column<int>(nullable: false),
                    ProtocolID = table.Column<int>(nullable: true),
                    ProductID = table.Column<int>(nullable: true),
                    IsTemporaryDeleted = table.Column<bool>(nullable: false),
                    LineID = table.Column<int>(nullable: false),
                    Timer = table.Column<TimeSpan>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionLines", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FunctionLines_FunctionTypes_FunctionTypeID",
                        column: x => x.FunctionTypeID,
                        principalTable: "FunctionTypes",
                        principalColumn: "FunctionTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FunctionLines_Lines_LineID",
                        column: x => x.LineID,
                        principalTable: "Lines",
                        principalColumn: "LineID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FunctionLines_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FunctionLines_Protocols_ProtocolID",
                        column: x => x.ProtocolID,
                        principalTable: "Protocols",
                        principalColumn: "ProtocolID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FunctionLines_FunctionTypeID",
                table: "FunctionLines",
                column: "FunctionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionLines_LineID",
                table: "FunctionLines",
                column: "LineID");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionLines_ProductID",
                table: "FunctionLines",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionLines_ProtocolID",
                table: "FunctionLines",
                column: "ProtocolID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FunctionLines");
        }
    }
}
