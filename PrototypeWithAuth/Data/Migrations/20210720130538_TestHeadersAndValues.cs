using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class TestHeadersAndValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestGroups_TestHeaderGroups_TestHeaderGroupID",
                table: "TestGroups");

            migrationBuilder.DropTable(
                name: "TestHeaderGroups");

            migrationBuilder.DropIndex(
                name: "IX_TestGroups_TestHeaderGroupID",
                table: "TestGroups");

            migrationBuilder.DropColumn(
                name: "TestHeaderGroupID",
                table: "TestGroups");

            migrationBuilder.AddColumn<int>(
                name: "TestOuterGroupID",
                table: "TestGroups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TestHeaders",
                columns: table => new
                {
                    TestHeaderID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestGroupID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    List = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestHeaders", x => x.TestHeaderID);
                    table.ForeignKey(
                        name: "FK_TestHeaders_TestGroups_TestGroupID",
                        column: x => x.TestGroupID,
                        principalTable: "TestGroups",
                        principalColumn: "TestGroupID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestOuterGroups",
                columns: table => new
                {
                    TestOuterGroupID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    IsNone = table.Column<bool>(nullable: false),
                    TestID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestOuterGroups", x => x.TestOuterGroupID);
                    table.ForeignKey(
                        name: "FK_TestOuterGroups_Tests_TestID",
                        column: x => x.TestID,
                        principalTable: "Tests",
                        principalColumn: "TestID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestValues",
                columns: table => new
                {
                    TestValueID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestHeaderID = table.Column<int>(nullable: false),
                    ParticipantID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestValues", x => x.TestValueID);
                    table.ForeignKey(
                        name: "FK_TestValues_Participants_ParticipantID",
                        column: x => x.ParticipantID,
                        principalTable: "Participants",
                        principalColumn: "ParticipantID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestValues_TestHeaders_TestHeaderID",
                        column: x => x.TestHeaderID,
                        principalTable: "TestHeaders",
                        principalColumn: "TestHeaderID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestGroups_TestOuterGroupID",
                table: "TestGroups",
                column: "TestOuterGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_TestHeaders_TestGroupID",
                table: "TestHeaders",
                column: "TestGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_TestOuterGroups_TestID",
                table: "TestOuterGroups",
                column: "TestID");

            migrationBuilder.CreateIndex(
                name: "IX_TestValues_ParticipantID",
                table: "TestValues",
                column: "ParticipantID");

            migrationBuilder.CreateIndex(
                name: "IX_TestValues_TestHeaderID",
                table: "TestValues",
                column: "TestHeaderID");

            migrationBuilder.AddForeignKey(
                name: "FK_TestGroups_TestOuterGroups_TestOuterGroupID",
                table: "TestGroups",
                column: "TestOuterGroupID",
                principalTable: "TestOuterGroups",
                principalColumn: "TestOuterGroupID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestGroups_TestOuterGroups_TestOuterGroupID",
                table: "TestGroups");

            migrationBuilder.DropTable(
                name: "TestOuterGroups");

            migrationBuilder.DropTable(
                name: "TestValues");

            migrationBuilder.DropTable(
                name: "TestHeaders");

            migrationBuilder.DropIndex(
                name: "IX_TestGroups_TestOuterGroupID",
                table: "TestGroups");

            migrationBuilder.DropColumn(
                name: "TestOuterGroupID",
                table: "TestGroups");

            migrationBuilder.AddColumn<int>(
                name: "TestHeaderGroupID",
                table: "TestGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TestHeaderGroups",
                columns: table => new
                {
                    TestHeaderGroupID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsNone = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestID = table.Column<int>(type: "int", nullable: false)
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
    }
}
