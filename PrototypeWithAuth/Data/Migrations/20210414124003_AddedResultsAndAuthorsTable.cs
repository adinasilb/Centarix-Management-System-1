using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedResultsAndAuthorsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialDescription",
                table: "MaterialCategories");

            migrationBuilder.AddColumn<string>(
                name: "MaterialCategoryDescription",
                table: "MaterialCategories",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorID);
                });

            migrationBuilder.CreateTable(
                name: "ProtocolInstanceResults",
                columns: table => new
                {
                    ResultID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResultDesciption = table.Column<string>(type: "ntext", nullable: true),
                    ProtocolInstanceID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtocolInstanceResults", x => x.ResultID);
                    table.ForeignKey(
                        name: "FK_ProtocolInstanceResults_ProtocolInstances_ProtocolInstanceID",
                        column: x => x.ProtocolInstanceID,
                        principalTable: "ProtocolInstances",
                        principalColumn: "ProtocolInstanceID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuthorProtocols",
                columns: table => new
                {
                    ProtocolID = table.Column<int>(nullable: false),
                    AuthorID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorProtocols", x => new { x.AuthorID, x.ProtocolID });
                    table.ForeignKey(
                        name: "FK_AuthorProtocols_Authors_AuthorID",
                        column: x => x.AuthorID,
                        principalTable: "Authors",
                        principalColumn: "AuthorID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuthorProtocols_Protocols_ProtocolID",
                        column: x => x.ProtocolID,
                        principalTable: "Protocols",
                        principalColumn: "ProtocolID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorProtocols_ProtocolID",
                table: "AuthorProtocols",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolInstanceResults_ProtocolInstanceID",
                table: "ProtocolInstanceResults",
                column: "ProtocolInstanceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorProtocols");

            migrationBuilder.DropTable(
                name: "ProtocolInstanceResults");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropColumn(
                name: "MaterialCategoryDescription",
                table: "MaterialCategories");

            migrationBuilder.AddColumn<string>(
                name: "MaterialDescription",
                table: "MaterialCategories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
