using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ChangesSomeCategoryNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 4,
                column: "ParentCategoryDescription",
                value: "Reusable");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 107,
                column: "ProductSubcategoryDescription",
                value: "Robot Consumables");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 108,
                column: "ProductSubcategoryDescription",
                value: "DD-PCR Plastics");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 109,
                column: "ProductSubcategoryDescription",
                value: "RT-PCR Plastics");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 118,
                column: "ProductSubcategoryDescription",
                value: "Tapestation consumables");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 120,
                column: "ProductSubcategoryDescription",
                value: "General Consumables");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 206,
                column: "ProductSubcategoryDescription",
                value: "PCR Reagents");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 207,
                column: "ProductSubcategoryDescription",
                value: "RT-PCR Reagents");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 210,
                column: "ProductSubcategoryDescription",
                value: "Cell Media Supplements");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 212,
                column: "ProductSubcategoryDescription",
                value: "Restriction Enzyme");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 213,
                column: "ProductSubcategoryDescription",
                value: "RNA Enzyme");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 217,
                column: "ProductSubcategoryDescription",
                value: "General Reagents and Chemicals");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 304,
                column: "ProductSubcategoryDescription",
                value: "Bacterial Stock");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 305,
                column: "ProductSubcategoryDescription",
                value: "General Biological");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 601,
                column: "ProductSubcategoryDescription",
                value: "General Safety");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 909,
                column: "ProductSubcategoryDescription",
                value: "General Day To Day");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1005,
                column: "ProductSubcategoryDescription",
                value: "General Travel");

            migrationBuilder.InsertData(
                table: "ProductSubcategories",
                columns: new[] { "ProductSubcategoryID", "ImageURL", "IsOldSubCategory", "ParentCategoryID", "ProductSubcategoryDescription" },
                values: new object[,]
                {
                    { 219, "/images/css/CategoryImages/reagents/gas_refilling2.png", false, 2, "Gas Refilling" },
                    { 220, "/images/css/CategoryImages/reagents/ddPCR_reagent3.png", false, 2, "DD-PCR  Reagents" },
                    { 218, "/images/css/CategoryImages/reagents/dna_enzyme.png", false, 2, "DNA Enzyme" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 218);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 219);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 220);

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 4,
                column: "ParentCategoryDescription",
                value: "Reusables");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 107,
                column: "ProductSubcategoryDescription",
                value: "Robot Consumables(Tips,Microplates, Reservoirs)");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 108,
                column: "ProductSubcategoryDescription",
                value: "DdPCR Consumables (Gaskets, Cartridges, Microplates, Foil seal)");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 109,
                column: "ProductSubcategoryDescription",
                value: "RT-PCR");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 118,
                column: "ProductSubcategoryDescription",
                value: "Tapestation Consumables (Screentapes: gDNA/HS/RNA; Markers, Loading Buffers, Loading Tips)");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 120,
                column: "ProductSubcategoryDescription",
                value: "General");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 206,
                column: "ProductSubcategoryDescription",
                value: "PCR");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 207,
                column: "ProductSubcategoryDescription",
                value: "RT-PCR");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 210,
                column: "ProductSubcategoryDescription",
                value: "Media Supplement");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 212,
                column: "ProductSubcategoryDescription",
                value: "Enzyme Restriction");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 213,
                column: "ProductSubcategoryDescription",
                value: "Enzyme RNA");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 217,
                column: "ProductSubcategoryDescription",
                value: "General");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 304,
                column: "ProductSubcategoryDescription",
                value: "Bacteria");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 305,
                column: "ProductSubcategoryDescription",
                value: "General");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 601,
                column: "ProductSubcategoryDescription",
                value: "General");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 909,
                column: "ProductSubcategoryDescription",
                value: "General");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1005,
                column: "ProductSubcategoryDescription",
                value: "Travel");
        }
    }
}
