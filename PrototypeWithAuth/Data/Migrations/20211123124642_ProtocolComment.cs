using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ProtocolComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProtocolComments_AspNetUsers_ApplicationUserCreatorID",
                table: "ProtocolComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProtocolComments",
                table: "ProtocolComments");

            migrationBuilder.DropIndex(
                name: "IX_ProtocolComments_ApplicationUserCreatorID",
                table: "ProtocolComments");

            migrationBuilder.DropColumn(
                name: "CommentType",
                table: "VendorComments");

            migrationBuilder.DropColumn(
                name: "CommentType",
                table: "RequestComments");

            migrationBuilder.DropColumn(
                name: "ProtocolCommentID",
                table: "ProtocolComments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserCreatorID",
                table: "ProtocolComments");

            migrationBuilder.DropColumn(
                name: "ProtocolCommentDescription",
                table: "ProtocolComments");

            migrationBuilder.DropColumn(
                name: "ProtocolCommmentType",
                table: "ProtocolComments");

            migrationBuilder.AddColumn<int>(
                name: "CommentID",
                table: "ProtocolComments",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserID",
                table: "ProtocolComments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommentText",
                table: "ProtocolComments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommentTypeFKID",
                table: "ProtocolComments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProtocolComments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ObjectID",
                table: "ProtocolComments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProtocolComments",
                table: "ProtocolComments",
                column: "CommentID");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolComments_ApplicationUserID",
                table: "ProtocolComments",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolComments_CommentTypeFKID",
                table: "ProtocolComments",
                column: "CommentTypeFKID");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolComments_ObjectID",
                table: "ProtocolComments",
                column: "ObjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProtocolComments_AspNetUsers_ApplicationUserID",
                table: "ProtocolComments",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProtocolComments_CommentTypes_CommentTypeFKID",
                table: "ProtocolComments",
                column: "CommentTypeFKID",
                principalTable: "CommentTypes",
                principalColumn: "TypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProtocolComments_Protocols_ObjectID",
                table: "ProtocolComments",
                column: "ObjectID",
                principalTable: "Protocols",
                principalColumn: "ProtocolID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProtocolComments_AspNetUsers_ApplicationUserID",
                table: "ProtocolComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProtocolComments_CommentTypes_CommentTypeFKID",
                table: "ProtocolComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProtocolComments_Protocols_ObjectID",
                table: "ProtocolComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProtocolComments",
                table: "ProtocolComments");

            migrationBuilder.DropIndex(
                name: "IX_ProtocolComments_ApplicationUserID",
                table: "ProtocolComments");

            migrationBuilder.DropIndex(
                name: "IX_ProtocolComments_CommentTypeFKID",
                table: "ProtocolComments");

            migrationBuilder.DropIndex(
                name: "IX_ProtocolComments_ObjectID",
                table: "ProtocolComments");

            migrationBuilder.DropColumn(
                name: "CommentID",
                table: "ProtocolComments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserID",
                table: "ProtocolComments");

            migrationBuilder.DropColumn(
                name: "CommentText",
                table: "ProtocolComments");

            migrationBuilder.DropColumn(
                name: "CommentTypeFKID",
                table: "ProtocolComments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProtocolComments");

            migrationBuilder.DropColumn(
                name: "ObjectID",
                table: "ProtocolComments");

            migrationBuilder.AddColumn<string>(
                name: "CommentType",
                table: "VendorComments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommentType",
                table: "RequestComments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProtocolCommentID",
                table: "ProtocolComments",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserCreatorID",
                table: "ProtocolComments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProtocolCommentDescription",
                table: "ProtocolComments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProtocolCommmentType",
                table: "ProtocolComments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProtocolComments",
                table: "ProtocolComments",
                column: "ProtocolCommentID");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolComments_ApplicationUserCreatorID",
                table: "ProtocolComments",
                column: "ApplicationUserCreatorID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProtocolComments_AspNetUsers_ApplicationUserCreatorID",
                table: "ProtocolComments",
                column: "ApplicationUserCreatorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
