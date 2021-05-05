using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class fixedFavoriteResourcesConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FavoriteResources_ResourceID",
                table: "FavoriteResources",
                column: "ResourceID");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteResources_Resources_ResourceID",
                table: "FavoriteResources",
                column: "ResourceID",
                principalTable: "Resources",
                principalColumn: "ResourceID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteResources_Resources_ResourceID",
                table: "FavoriteResources");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteResources_ResourceID",
                table: "FavoriteResources");
        }
    }
}
