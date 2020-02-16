using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class SeedProductSubcategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductSubcategory",
                columns: table => new
                {
                    ProductSubcategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentCategoryID = table.Column<int>(nullable: false),
                    ProductSubcategoryDescription = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSubcategory", x => x.ProductSubcategoryID);
                });

            migrationBuilder.InsertData(
                table: "ProductSubcategory",
                columns: new[] { "ProductSubcategoryID", "ParentCategoryID", "ProductSubcategoryDescription" },
                values: new object[,]
                {
                    { 11, 1, "Tubes" },
                    { 12, 1, "Pipets" },
                    { 13, 1, "Dishes" },
                    { 14, 1, "Dishes" },
                    { 15, 1, "Cell Culture Plates" },
                    { 21, 2, "Chemical Powder" },
                    { 22, 2, "DNA Enzyme" },
                    { 23, 2, "Antibodies" },
                    { 24, 2, "Cell Media" },
                    { 31, 3, "Virus" },
                    { 32, 3, "Plasmid" },
                    { 33, 3, "Primers" },
                    { 41, 4, "Beaker" },
                    { 42, 4, "Buckets" },
                    { 43, 4, "Measuring Instruments" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSubcategory");
        }
    }
}
