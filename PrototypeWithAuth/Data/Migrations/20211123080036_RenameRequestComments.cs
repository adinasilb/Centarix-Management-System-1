using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RenameRequestComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_ApplicationUserID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Requests_RequestID",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "RequestComments");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_RequestID",
                table: "RequestComments",
                newName: "IX_RequestComments_RequestID");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ApplicationUserID",
                table: "RequestComments",
                newName: "IX_RequestComments_ApplicationUserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestComments",
                table: "RequestComments",
                column: "CommentID");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestComments_AspNetUsers_ApplicationUserID",
                table: "RequestComments",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestComments_Requests_RequestID",
                table: "RequestComments",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "RequestID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestComments_AspNetUsers_ApplicationUserID",
                table: "RequestComments");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestComments_Requests_RequestID",
                table: "RequestComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestComments",
                table: "RequestComments");

            migrationBuilder.RenameTable(
                name: "RequestComments",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_RequestComments_RequestID",
                table: "Comments",
                newName: "IX_Comments_RequestID");

            migrationBuilder.RenameIndex(
                name: "IX_RequestComments_ApplicationUserID",
                table: "Comments",
                newName: "IX_Comments_ApplicationUserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "CommentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_ApplicationUserID",
                table: "Comments",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Requests_RequestID",
                table: "Comments",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "RequestID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
