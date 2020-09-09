using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MoreEMployeeFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_JobCategoryType_JobCategoryTypeID",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobCategoryType",
                table: "JobCategoryType");

            migrationBuilder.RenameTable(
                name: "JobCategoryType",
                newName: "JobCategoryTypes");

            migrationBuilder.AddColumn<string>(
                name: "Citizenship",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Degree",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelationshipStatus",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobCategoryTypes",
                table: "JobCategoryTypes",
                column: "JobCategoryTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_JobCategoryTypes_JobCategoryTypeID",
                table: "AspNetUsers",
                column: "JobCategoryTypeID",
                principalTable: "JobCategoryTypes",
                principalColumn: "JobCategoryTypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_JobCategoryTypes_JobCategoryTypeID",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobCategoryTypes",
                table: "JobCategoryTypes");

            migrationBuilder.DropColumn(
                name: "Citizenship",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Degree",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RelationshipStatus",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "JobCategoryTypes",
                newName: "JobCategoryType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobCategoryType",
                table: "JobCategoryType",
                column: "JobCategoryTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_JobCategoryType_JobCategoryTypeID",
                table: "AspNetUsers",
                column: "JobCategoryTypeID",
                principalTable: "JobCategoryType",
                principalColumn: "JobCategoryTypeID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
