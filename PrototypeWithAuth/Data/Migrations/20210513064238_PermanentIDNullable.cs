using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class PermanentIDNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TempLines_TempLines_ParentLineID",
                table: "TempLines");

            migrationBuilder.DropForeignKey(
                name: "FK_TempLines_Lines_PermanentLineID",
                table: "TempLines");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_TempLines_PermanentLineID",
                table: "TempLines");

            migrationBuilder.DropIndex(
                name: "IX_TempLines_PermanentLineID",
                table: "TempLines");

            migrationBuilder.DropColumn(
                name: "PermanentLineID",
                table: "TempLines");

            migrationBuilder.AddColumn<int>(
                name: "PermanentLineIDs",
                table: "TempLines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_TempLines_PermanentLineIDs",
                table: "TempLines",
                column: "PermanentLineIDs");

            migrationBuilder.CreateIndex(
                name: "IX_TempLines_PermanentLineIDs",
                table: "TempLines",
                column: "PermanentLineIDs",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TempLines_TempLines_ParentLineID",
                table: "TempLines",
                column: "ParentLineID",
                principalTable: "TempLines",
                principalColumn: "PermanentLineIDs",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TempLines_Lines_PermanentLineIDs",
                table: "TempLines",
                column: "PermanentLineIDs",
                principalTable: "Lines",
                principalColumn: "LineID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TempLines_TempLines_ParentLineID",
                table: "TempLines");

            migrationBuilder.DropForeignKey(
                name: "FK_TempLines_Lines_PermanentLineIDs",
                table: "TempLines");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_TempLines_PermanentLineIDs",
                table: "TempLines");

            migrationBuilder.DropIndex(
                name: "IX_TempLines_PermanentLineIDs",
                table: "TempLines");

            migrationBuilder.DropColumn(
                name: "PermanentLineIDs",
                table: "TempLines");

            migrationBuilder.AddColumn<int>(
                name: "PermanentLineID",
                table: "TempLines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_TempLines_PermanentLineID",
                table: "TempLines",
                column: "PermanentLineID");

            migrationBuilder.CreateIndex(
                name: "IX_TempLines_PermanentLineID",
                table: "TempLines",
                column: "PermanentLineID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TempLines_TempLines_ParentLineID",
                table: "TempLines",
                column: "ParentLineID",
                principalTable: "TempLines",
                principalColumn: "PermanentLineID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TempLines_Lines_PermanentLineID",
                table: "TempLines",
                column: "PermanentLineID",
                principalTable: "Lines",
                principalColumn: "LineID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
