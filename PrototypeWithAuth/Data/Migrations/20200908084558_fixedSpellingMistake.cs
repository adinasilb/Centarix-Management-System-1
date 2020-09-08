using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class fixedSpellingMistake : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "JobCategoryTypeID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobCategoryType",
                columns: table => new
                {
                    JobCategoryTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCategoryType", x => x.JobCategoryTypeID);
                });

            migrationBuilder.InsertData(
                table: "JobCategoryType",
                columns: new[] { "JobCategoryTypeID", "Description" },
                values: new object[,]
                {
                    { 1, "Executive" },
                    { 2, "Senior Manager" },
                    { 3, "Manager" },
                    { 4, "Senior Bioinformatician" },
                    { 5, "Bioinformatician" },
                    { 6, "Senior Scientist" },
                    { 7, "Lab Technician" },
                    { 8, "Research Associate" },
                    { 9, "Software Developer" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_JobCategoryTypeID",
                table: "AspNetUsers",
                column: "JobCategoryTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_JobCategoryType_JobCategoryTypeID",
                table: "AspNetUsers",
                column: "JobCategoryTypeID",
                principalTable: "JobCategoryType",
                principalColumn: "JobCategoryTypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_JobCategoryType_JobCategoryTypeID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "JobCategoryType");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_JobCategoryTypeID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JobCategoryTypeID",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "JobCategoryID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JobCategoryTypesJobCategoryTypeID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobCategoryTypes",
                columns: table => new
                {
                    JobCategoryTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCategoryTypes", x => x.JobCategoryTypeID);
                });

            migrationBuilder.InsertData(
                table: "JobCategoryTypes",
                columns: new[] { "JobCategoryTypeID", "Description" },
                values: new object[,]
                {
                    { 1, "Executive" },
                    { 2, "Senior Manager" },
                    { 3, "Manager" },
                    { 4, "Senior Bioinformatician" },
                    { 5, "Bioinformatician" },
                    { 6, "Senior Scientist" },
                    { 7, "Lab Technician" },
                    { 8, "Research Associate" },
                    { 9, "Software Developer" }
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
    }
}
