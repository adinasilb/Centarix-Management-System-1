using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MaterialProtocolFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProtocolID",
                table: "Materials",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ProtocolID",
                table: "Materials",
                column: "ProtocolID");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Protocols_ProtocolID",
                table: "Materials",
                column: "ProtocolID",
                principalTable: "Protocols",
                principalColumn: "ProtocolID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Protocols_ProtocolID",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_ProtocolID",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ProtocolID",
                table: "Materials");
        }
    }
}
