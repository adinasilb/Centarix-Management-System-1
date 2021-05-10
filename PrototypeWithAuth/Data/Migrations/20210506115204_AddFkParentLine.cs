using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddFkParentLine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentLineID",
                table: "TempLines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParentLineID",
                table: "Lines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TempLines_ParentLineID",
                table: "TempLines",
                column: "ParentLineID");

            migrationBuilder.CreateIndex(
                name: "IX_Lines_ParentLineID",
                table: "Lines",
                column: "ParentLineID");

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_Lines_ParentLineID",
                table: "Lines",
                column: "ParentLineID",
                principalTable: "Lines",
                principalColumn: "LineID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TempLines_TempLines_ParentLineID",
                table: "TempLines",
                column: "ParentLineID",
                principalTable: "TempLines",
                principalColumn: "LineID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lines_Lines_ParentLineID",
                table: "Lines");

            migrationBuilder.DropForeignKey(
                name: "FK_TempLines_TempLines_ParentLineID",
                table: "TempLines");

            migrationBuilder.DropIndex(
                name: "IX_TempLines_ParentLineID",
                table: "TempLines");

            migrationBuilder.DropIndex(
                name: "IX_Lines_ParentLineID",
                table: "Lines");

            migrationBuilder.DropColumn(
                name: "ParentLineID",
                table: "TempLines");

            migrationBuilder.DropColumn(
                name: "ParentLineID",
                table: "Lines");
        }
    }
}
