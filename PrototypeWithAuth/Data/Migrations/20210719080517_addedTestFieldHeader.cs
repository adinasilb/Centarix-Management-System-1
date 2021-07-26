using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedTestFieldHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestFieldHeaders",
                columns: table => new
                {
                    TestFieldHeaderID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestID = table.Column<int>(nullable: false),
                    FieldNames = table.Column<string>(nullable: true),
                    FieldTypes = table.Column<string>(nullable: true),
                    FieldList = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestFieldHeaders", x => x.TestFieldHeaderID);
                    table.ForeignKey(
                        name: "FK_TestFieldHeaders_Tests_TestID",
                        column: x => x.TestID,
                        principalTable: "Tests",
                        principalColumn: "TestID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestFieldHeaders_TestID",
                table: "TestFieldHeaders",
                column: "TestID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestFieldHeaders");
        }
    }
}
