using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedFunctionResults : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FunctionResults",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FunctionTypeID = table.Column<int>(nullable: false),
                    ProtocolID = table.Column<int>(nullable: true),
                    ProductID = table.Column<int>(nullable: true),
                    IsTemporaryDeleted = table.Column<bool>(nullable: false),
                    ProtocolInstanceID = table.Column<int>(nullable: false),
                    IsTemporary = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionResults", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FunctionResults_FunctionTypes_FunctionTypeID",
                        column: x => x.FunctionTypeID,
                        principalTable: "FunctionTypes",
                        principalColumn: "FunctionTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FunctionResults_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FunctionResults_Protocols_ProtocolID",
                        column: x => x.ProtocolID,
                        principalTable: "Protocols",
                        principalColumn: "ProtocolID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FunctionResults_ProtocolInstances_ProtocolInstanceID",
                        column: x => x.ProtocolInstanceID,
                        principalTable: "ProtocolInstances",
                        principalColumn: "ProtocolInstanceID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FunctionResults_FunctionTypeID",
                table: "FunctionResults",
                column: "FunctionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionResults_ProductID",
                table: "FunctionResults",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionResults_ProtocolID",
                table: "FunctionResults",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionResults_ProtocolInstanceID",
                table: "FunctionResults",
                column: "ProtocolInstanceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FunctionResults");
        }
    }
}
