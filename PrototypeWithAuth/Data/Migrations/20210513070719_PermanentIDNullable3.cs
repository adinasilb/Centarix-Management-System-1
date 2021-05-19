using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class PermanentIDNullable3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TempLines_PermanentLineID",
                table: "TempLines");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TempLines_PermanentLineID",
                table: "TempLines",
                column: "PermanentLineID",
                unique: true);
        }
    }
}
