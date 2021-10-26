using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class experimentTestManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Experiments_ExperimentID",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_ExperimentID",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "ExperimentID",
                table: "Tests");

            migrationBuilder.CreateTable(
                name: "ExperimentTest",
                columns: table => new
                {
                    ExperimentID = table.Column<int>(nullable: false),
                    TestID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentTest", x => new { x.ExperimentID, x.TestID });
                    table.ForeignKey(
                        name: "FK_ExperimentTest_Experiments_ExperimentID",
                        column: x => x.ExperimentID,
                        principalTable: "Experiments",
                        principalColumn: "ExperimentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExperimentTest_Tests_TestID",
                        column: x => x.TestID,
                        principalTable: "Tests",
                        principalColumn: "TestID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentTest_TestID",
                table: "ExperimentTest",
                column: "TestID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExperimentTest");

            migrationBuilder.AddColumn<int>(
                name: "ExperimentID",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tests_ExperimentID",
                table: "Tests",
                column: "ExperimentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Experiments_ExperimentID",
                table: "Tests",
                column: "ExperimentID",
                principalTable: "Experiments",
                principalColumn: "ExperimentID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
