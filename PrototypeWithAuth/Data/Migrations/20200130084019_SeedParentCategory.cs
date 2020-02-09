using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class SeedParentCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParentCategory",
                columns: table => new
                {
                    ParentCategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentCategoryDescription = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentCategory", x => x.ParentCategoryID);
                });

            migrationBuilder.InsertData(
                table: "ParentCategory",
                columns: new[] { "ParentCategoryID", "ParentCategoryDescription" },
                values: new object[,]
                {
                    { 1, "Plastics" },
                    { 2, "Reagents" },
                    { 3, "Proprietry" },
                    { 4, "Reusable" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParentCategory");
        }
    }
}
