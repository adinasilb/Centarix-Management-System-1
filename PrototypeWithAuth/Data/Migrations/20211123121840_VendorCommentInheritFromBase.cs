using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class VendorCommentInheritFromBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommentTypeFKID",
                table: "VendorComments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "VendorComments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql("UPDATE VendorComments SET CommentTypeFKID= 1 WHERE CommentType ='Comment'");
            migrationBuilder.Sql("UPDATE VendorComments SET CommentTypeFKID= 2 WHERE CommentType ='Warning'");

            migrationBuilder.CreateIndex(
                name: "IX_VendorComments_CommentTypeFKID",
                table: "VendorComments",
                column: "CommentTypeFKID");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorComments_CommentTypes_CommentTypeFKID",
                table: "VendorComments",
                column: "CommentTypeFKID",
                principalTable: "CommentTypes",
                principalColumn: "TypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorComments_CommentTypes_CommentTypeFKID",
                table: "VendorComments");

            migrationBuilder.DropIndex(
                name: "IX_VendorComments_CommentTypeFKID",
                table: "VendorComments");

            migrationBuilder.DropColumn(
                name: "CommentTypeFKID",
                table: "VendorComments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "VendorComments");
        }
    }
}
