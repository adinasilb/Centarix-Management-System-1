using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class removedArticles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_ResourceTypes_ResourceTypeID",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_TagArticles_Articles_ArticleID",
                table: "TagArticles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Articles",
                table: "Articles");

            migrationBuilder.RenameTable(
                name: "Articles",
                newName: "Article");

            migrationBuilder.RenameIndex(
                name: "IX_Articles_ResourceTypeID",
                table: "Article",
                newName: "IX_Article_ResourceTypeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Article",
                table: "Article",
                column: "ArticleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Article_ResourceTypes_ResourceTypeID",
                table: "Article",
                column: "ResourceTypeID",
                principalTable: "ResourceTypes",
                principalColumn: "ResourceTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TagArticles_Article_ArticleID",
                table: "TagArticles",
                column: "ArticleID",
                principalTable: "Article",
                principalColumn: "ArticleID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_ResourceTypes_ResourceTypeID",
                table: "Article");

            migrationBuilder.DropForeignKey(
                name: "FK_TagArticles_Article_ArticleID",
                table: "TagArticles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Article",
                table: "Article");

            migrationBuilder.RenameTable(
                name: "Article",
                newName: "Articles");

            migrationBuilder.RenameIndex(
                name: "IX_Article_ResourceTypeID",
                table: "Articles",
                newName: "IX_Articles_ResourceTypeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Articles",
                table: "Articles",
                column: "ArticleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_ResourceTypes_ResourceTypeID",
                table: "Articles",
                column: "ResourceTypeID",
                principalTable: "ResourceTypes",
                principalColumn: "ResourceTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TagArticles_Articles_ArticleID",
                table: "TagArticles",
                column: "ArticleID",
                principalTable: "Articles",
                principalColumn: "ArticleID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
