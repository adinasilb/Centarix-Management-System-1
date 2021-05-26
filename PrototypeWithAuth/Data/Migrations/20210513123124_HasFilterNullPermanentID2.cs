using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class HasFilterNullPermanentID2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_TempLines_PermanentLineID",
                table: "TempLines",
                column: "PermanentLineID",
                unique: true);
            //Debbie: i editted the pricipal column because i saw no other way
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

            migrationBuilder.DropIndex(
                name: "IX_TempLines_PermanentLineID",
                table: "TempLines");

            migrationBuilder.AlterColumn<int>(
                name: "PermanentLineID",
                table: "TempLines",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
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
    }
}
