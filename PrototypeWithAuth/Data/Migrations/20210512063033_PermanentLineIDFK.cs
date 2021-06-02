using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class PermanentLineIDFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TempLines_TempLines_ParentLineID",
                table: "TempLines");

            migrationBuilder.DropIndex(
                name: "IX_TempLines_ParentLineID",
                table: "TempLines");

            migrationBuilder.AddForeignKey(
                name: "FK_TempLines_TempLines_PermanentLineID",
                table: "TempLines",
                column: "PermanentLineID",
                principalTable: "TempLines",
                principalColumn: "LineID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TempLines_TempLines_PermanentLineID",
                table: "TempLines");

            migrationBuilder.CreateIndex(
                name: "IX_TempLines_ParentLineID",
                table: "TempLines",
                column: "ParentLineID");

            migrationBuilder.AddForeignKey(
                name: "FK_TempLines_TempLines_ParentLineID",
                table: "TempLines",
                column: "ParentLineID",
                principalTable: "TempLines",
                principalColumn: "LineID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
