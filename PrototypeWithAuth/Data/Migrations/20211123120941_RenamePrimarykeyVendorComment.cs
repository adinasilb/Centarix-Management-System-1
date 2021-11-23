using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RenamePrimarykeyVendorComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorComments",
                table: "VendorComments");

            migrationBuilder.RenameColumn(
                name: "VendorCommentID",
                table: "VendorComments",
                newName: "CommentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VendorComments",
                table: "VendorComments",
                column: "CommentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorComments",
                table: "VendorComments");

            migrationBuilder.RenameColumn(
                name: "CommentID",
                table: "VendorComments",
                newName: "VendorCommentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VendorComments",
                table: "VendorComments",
                column: "VendorCommentID");
        }
    }
}
