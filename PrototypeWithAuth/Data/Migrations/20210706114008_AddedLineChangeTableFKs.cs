using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedLineChangeTableFKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LineChanges_ProtocolInstanceID",
                table: "LineChanges",
                column: "ProtocolInstanceID");

            migrationBuilder.AddForeignKey(
                name: "FK_LineChanges_Lines_LineID",
                table: "LineChanges",
                column: "LineID",
                principalTable: "Lines",
                principalColumn: "LineID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LineChanges_ProtocolInstances_ProtocolInstanceID",
                table: "LineChanges",
                column: "ProtocolInstanceID",
                principalTable: "ProtocolInstances",
                principalColumn: "ProtocolInstanceID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineChanges_Lines_LineID",
                table: "LineChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_LineChanges_ProtocolInstances_ProtocolInstanceID",
                table: "LineChanges");

            migrationBuilder.DropIndex(
                name: "IX_LineChanges_ProtocolInstanceID",
                table: "LineChanges");
        }
    }
}
