using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedJobCategoryType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobCategoryID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JobCategoryTypesJobCategoryTypeID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobCategoryTypes",
                columns: table => new
                {
                    JobCategoryTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCategoryTypes", x => x.JobCategoryTypeID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_JobCategoryTypesJobCategoryTypeID",
                table: "AspNetUsers",
                column: "JobCategoryTypesJobCategoryTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_JobCategoryTypes_JobCategoryTypesJobCategoryTypeID",
                table: "AspNetUsers",
                column: "JobCategoryTypesJobCategoryTypeID",
                principalTable: "JobCategoryTypes",
                principalColumn: "JobCategoryTypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_JobCategoryTypes_JobCategoryTypesJobCategoryTypeID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "JobCategoryTypes");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_JobCategoryTypesJobCategoryTypeID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JobCategoryID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JobCategoryTypesJobCategoryTypeID",
                table: "AspNetUsers");
        }
    }
}
