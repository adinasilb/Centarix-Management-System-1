using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class UpdateSubcategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 302);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 303);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 304);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 402);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 403);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 404);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 405);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 406);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 407);

            migrationBuilder.AddColumn<bool>(
                name: "isProprietary",
                table: "ParentCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 2,
                column: "ParentCategoryDescription",
                value: "Reagents And Chemicals");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 3,
                column: "ParentCategoryDescription",
                value: "Cells");

            migrationBuilder.InsertData(
                table: "ParentCategories",
                columns: new[] { "ParentCategoryID", "CategoryTypeID", "ParentCategoryDescription", "isProprietary" },
                values: new object[] { 7, 1, "Proprietry", true });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 105,
                column: "ProductSubcategoryDescription",
                value: "Petri Dishes");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 301,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { null, "Cells" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 401,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { null, "Reusables" });

            migrationBuilder.InsertData(
                table: "ProductSubcategories",
                columns: new[] { "ProductSubcategoryID", "ImageURL", "ParentCategoryID", "ProductSubcategoryDescription" },
                values: new object[,]
                {
                    { 701, "/images/css/CategoryImages/virus.png", 3, "Virus" },
                    { 702, "/images/css/CategoryImages/plasmid.png", 3, "Plasmid" },
                    { 703, null, 3, "Probes" },
                    { 704, null, 3, "Cells" },
                    { 705, null, 3, "Bacteria with Plasmids" },
                    { 706, null, 3, "Blood" },
                    { 707, null, 3, "Serum" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 701);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 702);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 703);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 704);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 705);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 706);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 707);

            migrationBuilder.DropColumn(
                name: "isProprietary",
                table: "ParentCategories");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 2,
                column: "ParentCategoryDescription",
                value: "Reagents");

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 3,
                column: "ParentCategoryDescription",
                value: "Proprietry");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 105,
                column: "ProductSubcategoryDescription",
                value: "Dishes");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 301,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/virus.png", "Virus" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 401,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/beaker.png", "Beaker" });

            migrationBuilder.InsertData(
                table: "ProductSubcategories",
                columns: new[] { "ProductSubcategoryID", "ImageURL", "ParentCategoryID", "ProductSubcategoryDescription" },
                values: new object[,]
                {
                    { 101, null, 1, "3D Cells Grow" },
                    { 213, null, 2, "Plasmid Purification" },
                    { 302, "/images/css/CategoryImages/plasmid.png", 3, "Plasmid" },
                    { 303, "/images/css/CategoryImages/primer.png", 3, "Primers" },
                    { 304, null, 3, "Probes" },
                    { 402, "/images/css/CategoryImages/measuring.png", 4, "Measuring" },
                    { 403, "/images/css/CategoryImages/tube_holder.png", 4, "Tube Holders" },
                    { 404, "/images/css/CategoryImages/bucket.png", 4, "Buckets" },
                    { 405, null, 4, "Cooling Racks" },
                    { 406, "/images/css/CategoryImages/196box.png", 4, "-196 Box" },
                    { 407, "/images/css/CategoryImages/80box.png", 4, "-80 Box" }
                });
        }
    }
}
