using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ProductSubcategoryNameFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 108,
                column: "ProductSubcategoryDescription",
                value: "DdPCR Consumables (Gaskets, Cartridges, Microplates, Foil seal)");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 118,
                column: "ProductSubcategoryDescription",
                value: "Tapestation Consumables (Screentapes: gDNA/HS/RNA; Markers, Loading Buffers, Loading Tips)");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 216,
                column: "ProductSubcategoryDescription",
                value: "Nucleic Acid Quantitation (DNA/RNA qubit assay, Picogreen assay)");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 502,
                column: "ImageURL",
                value: "/images/css/CategoryImages/safety/safety.png");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 108,
                column: "ProductSubcategoryDescription",
                value: "DdPCR Consumables(Gaskets, Cartridges, Microplates, Foil seal)");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 118,
                column: "ProductSubcategoryDescription",
                value: "Tapestation Consumables(Screentapes: gDNA/HS/RNA; Markers, Loading Buffers, Loading Tips)");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 216,
                column: "ProductSubcategoryDescription",
                value: "Nucleic Acid Quantitation(DNA/RNA qubit assay, Picogreen assay)");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 502,
                column: "ImageURL",
                value: "/images/css/CategoryImages/sagety/safety.png");
        }
    }
}
