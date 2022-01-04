using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RequestCommentInheritsBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommentTypeFKID",
                table: "RequestComments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CommentTypes",
                columns: table => new
                {
                    TypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    IconActionClass = table.Column<string>(nullable: true),
                    DescriptionEnum = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentTypes", x => x.TypeID);
                });

            migrationBuilder.InsertData(
                table: "CommentTypes",
                columns: new[] { "TypeID", "Color", "Description", "DescriptionEnum", "Icon", "IconActionClass" },
                values: new object[] { 1, "#30BCC9", "Comment", "Comment", "icon-comment-24px", null });

            migrationBuilder.InsertData(
                table: "CommentTypes",
                columns: new[] { "TypeID", "Color", "Description", "DescriptionEnum", "Icon", "IconActionClass" },
                values: new object[] { 2, " var(--danger-color)", "Warning", "Warning", "icon-report_problem-24px", null });


            migrationBuilder.Sql("UPDATE RequestComments SET CommentTypeFKID= 1 WHERE CommentType ='Comment'");
            migrationBuilder.Sql("UPDATE RequestComments SET CommentTypeFKID= 2 WHERE CommentType ='Warning'");

            migrationBuilder.CreateIndex(
                name: "IX_RequestComments_CommentTypeFKID",
                table: "RequestComments",
                column: "CommentTypeFKID");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestComments_CommentTypes_CommentTypeFKID",
                table: "RequestComments",
                column: "CommentTypeFKID",
                principalTable: "CommentTypes",
                principalColumn: "TypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestComments_CommentTypes_CommentTypeFKID",
                table: "RequestComments");

            migrationBuilder.DropTable(
                name: "CommentTypes");

            migrationBuilder.DropIndex(
                name: "IX_RequestComments_CommentTypeFKID",
                table: "RequestComments");

            migrationBuilder.DropColumn(
                name: "CommentTypeFKID",
                table: "RequestComments");
        }
    }
}
