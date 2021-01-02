using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedJobSubcategoryTypesAndChangedEmployeeFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_JobCategoryTypes_JobCategoryTypeID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_JobCategoryTypeID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JobCategoryTypeID",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "JobSubategoryTypeID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JobSubcategoryTypeID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobSubcategoryTypes",
                columns: table => new
                {
                    JobSubcategoryTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    JobCategoryTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSubcategoryTypes", x => x.JobSubcategoryTypeID);
                    table.ForeignKey(
                        name: "FK_JobSubcategoryTypes_JobCategoryTypes_JobCategoryTypeID",
                        column: x => x.JobCategoryTypeID,
                        principalTable: "JobCategoryTypes",
                        principalColumn: "JobCategoryTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_JobSubcategoryTypeID",
                table: "AspNetUsers",
                column: "JobSubcategoryTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_JobSubcategoryTypes_JobCategoryTypeID",
                table: "JobSubcategoryTypes",
                column: "JobCategoryTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_JobSubcategoryTypes_JobSubcategoryTypeID",
                table: "AspNetUsers",
                column: "JobSubcategoryTypeID",
                principalTable: "JobSubcategoryTypes",
                principalColumn: "JobSubcategoryTypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_JobSubcategoryTypes_JobSubcategoryTypeID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "JobSubcategoryTypes");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_JobSubcategoryTypeID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JobSubategoryTypeID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JobSubcategoryTypeID",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "JobCategoryTypeID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_JobCategoryTypeID",
                table: "AspNetUsers",
                column: "JobCategoryTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_JobCategoryTypes_JobCategoryTypeID",
                table: "AspNetUsers",
                column: "JobCategoryTypeID",
                principalTable: "JobCategoryTypes",
                principalColumn: "JobCategoryTypeID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
