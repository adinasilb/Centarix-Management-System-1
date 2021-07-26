using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedTests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    TestID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ExperimentID = table.Column<int>(nullable: false),
                    TestCategoryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.TestID);
                    table.ForeignKey(
                        name: "FK_Tests_Experiments_ExperimentID",
                        column: x => x.ExperimentID,
                        principalTable: "Experiments",
                        principalColumn: "ExperimentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tests_TestCategories_TestCategoryID",
                        column: x => x.TestCategoryID,
                        principalTable: "TestCategories",
                        principalColumn: "TestCategoryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tests_ExperimentID",
                table: "Tests",
                column: "ExperimentID");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_TestCategoryID",
                table: "Tests",
                column: "TestCategoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tests");
        }
    }
}
