using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class HasFilterNullPermanentID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TempLines_TempLines_ParentLineID",
                table: "TempLines");

            migrationBuilder.DropIndex(
                name: "IX_TempLines_PermanentLineID",
                table: "TempLines");

            migrationBuilder.AlterColumn<int>(
                name: "PermanentLineID",
                table: "TempLines",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_TempLines_PermanentLineID",
                table: "TempLines",
                column: "PermanentLineID");

            migrationBuilder.CreateIndex(
                name: "IX_TempLines_PermanentLineID",
                table: "TempLines",
                column: "PermanentLineID",
                unique: true,
                filter: "[PermanentLineID] IS NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_TempLines_TempLines_ParentLineID",
                table: "TempLines",
                column: "ParentLineID",
                principalTable: "TempLines",
                principalColumn: "PermanentLineID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TempLines_TempLines_ParentLineID",
                table: "TempLines");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_TempLines_PermanentLineID",
                table: "TempLines");

            migrationBuilder.DropIndex(
                name: "IX_TempLines_PermanentLineID",
                table: "TempLines");

            migrationBuilder.AlterColumn<int>(
                name: "PermanentLineID",
                table: "TempLines",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_TempLines_PermanentLineID",
                table: "TempLines",
                column: "PermanentLineID",
                unique: true,
                filter: "[PermanentLineID] IS NOT NULL");

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
