using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedTempRequestJson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TempRequestJsons",
                columns: table => new
                {
                    CookieGUID = table.Column<string>(nullable: false),
                    ApplicationUserID = table.Column<string>(nullable: true),
                    RequestJson = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempRequestJsons", x => x.CookieGUID);
                    table.ForeignKey(
                        name: "FK_TempRequestJsons_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TempRequestJsons_ApplicationUserID",
                table: "TempRequestJsons",
                column: "ApplicationUserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TempRequestJsons");
        }
    }
}
