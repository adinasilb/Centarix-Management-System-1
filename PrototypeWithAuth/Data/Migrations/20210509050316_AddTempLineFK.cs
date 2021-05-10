using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddTempLineFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TempLines_Lines_LineID",
                table: "TempLines");

            migrationBuilder.AddColumn<int>(
                name: "PermanentLineID",
                table: "TempLines",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TempLines_PermanentLineID",
                table: "TempLines",
                column: "PermanentLineID",
                unique: true,
                filter: "[PermanentLineID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_TempLines_Lines_PermanentLineID",
                table: "TempLines",
                column: "PermanentLineID",
                principalTable: "Lines",
                principalColumn: "LineID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TempLines_Lines_PermanentLineID",
                table: "TempLines");

            migrationBuilder.DropIndex(
                name: "IX_TempLines_PermanentLineID",
                table: "TempLines");

            migrationBuilder.DropColumn(
                name: "PermanentLineID",
                table: "TempLines");

            migrationBuilder.AddForeignKey(
                name: "FK_TempLines_Lines_LineID",
                table: "TempLines",
                column: "LineID",
                principalTable: "Lines",
                principalColumn: "LineID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
