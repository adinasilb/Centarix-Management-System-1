using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedHeaderGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TestGroup",
                table: "TestGroup");

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

            migrationBuilder.RenameTable(
                name: "TestGroup",
                newName: "TestGroups");

            migrationBuilder.AddColumn<bool>(
                name: "IsNone",
                table: "TestGroups",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TestHeaderGroupID",
                table: "TestGroups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestGroups",
                table: "TestGroups",
                column: "TestGroupID");

            migrationBuilder.CreateTable(
                name: "TestHeaderGroups",
                columns: table => new
                {
                    TestHeaderGroupID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    IsNone = table.Column<bool>(nullable: false),
                    TestID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestHeaderGroups", x => x.TestHeaderGroupID);
                    table.ForeignKey(
                        name: "FK_TestHeaderGroups_Tests_TestID",
                        column: x => x.TestID,
                        principalTable: "Tests",
                        principalColumn: "TestID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestGroups_TestHeaderGroupID",
                table: "TestGroups",
                column: "TestHeaderGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_TestHeaderGroups_TestID",
                table: "TestHeaderGroups",
                column: "TestID");

            migrationBuilder.AddForeignKey(
                name: "FK_TestGroups_TestHeaderGroups_TestHeaderGroupID",
                table: "TestGroups",
                column: "TestHeaderGroupID",
                principalTable: "TestHeaderGroups",
                principalColumn: "TestHeaderGroupID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestGroups_TestHeaderGroups_TestHeaderGroupID",
                table: "TestGroups");

            migrationBuilder.DropTable(
                name: "TestHeaderGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestGroups",
                table: "TestGroups");

            migrationBuilder.DropIndex(
                name: "IX_TestGroups_TestHeaderGroupID",
                table: "TestGroups");

            migrationBuilder.DropColumn(
                name: "IsNone",
                table: "TestGroups");

            migrationBuilder.DropColumn(
                name: "TestHeaderGroupID",
                table: "TestGroups");

            migrationBuilder.RenameTable(
                name: "TestGroups",
                newName: "TestGroup");

            migrationBuilder.AddColumn<string>(
                name: "Header1GroupID",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Header1ID",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Header1Name",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Header1Type",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestGroup",
                table: "TestGroup",
                column: "TestGroupID");
        }
    }
}
