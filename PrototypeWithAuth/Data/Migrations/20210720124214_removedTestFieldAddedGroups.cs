using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class removedTestFieldAddedGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestFieldHeaders");

            migrationBuilder.AddColumn<string>(
                name: "Header1GroupID",
                table: "Tests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Header1ID",
                table: "Tests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Header1Name",
                table: "Tests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Header1Type",
                table: "Tests",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TestGroup",
                columns: table => new
                {
                    TestGroupID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestGroup", x => x.TestGroupID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestGroup");

            migrationBuilder.DropColumn(
                name: "Header1GroupID",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Header1ID",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Header1Name",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Header1Type",
                table: "Tests");

            migrationBuilder.CreateTable(
                name: "TestFieldHeaders",
                columns: table => new
                {
                    TestFieldHeaderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FieldNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FieldTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestID = table.Column<int>(type: "int", nullable: false)
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
    }
}
