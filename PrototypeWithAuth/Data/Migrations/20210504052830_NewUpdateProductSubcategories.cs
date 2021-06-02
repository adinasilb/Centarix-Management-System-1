using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class NewUpdateProductSubcategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductSubcategories",
                columns: new[] { "ProductSubcategoryID", "ImageURL", "ParentCategoryID", "ProductSubcategoryDescription" },
                values: new object[,]
                {
                    { 101, "/images/css/CategoryImages/consumables/pcr_consumables.png", 1, "PCR" },
                    { 915, "/images/css/CategoryImages/conference.png", 9, "Rent" },
                    { 914, "/images/css/CategoryImages/conference.png", 9, "Renovation" },
                    { 913, "/images/css/CategoryImages/conference.png", 9, "Parking" },
                    { 912, "/images/css/CategoryImages/conference.png", 9, "Insurance" },
                    { 911, "/images/css/CategoryImages/conference.png", 9, "Graphic" },
                    { 910, "/images/css/CategoryImages/conference.png", 9, "General" },
                    { 909, "/images/css/CategoryImages/conference.png", 9, "Furniture" },
                    { 908, "/images/css/CategoryImages/conference.png", 9, "Food" },
                    { 907, "/images/css/CategoryImages/conference.png", 9, "Fees" },
                    { 906, "/images/css/CategoryImages/conference.png", 9, "Electricity" },
                    { 905, "/images/css/CategoryImages/conference.png", 9, "Bookeeping" },
                    { 904, "/images/css/CategoryImages/conference.png", 9, "Company Events" },
                    { 903, "/images/css/CategoryImages/conference.png", 9, "Branding" },
                    { 902, "/images/css/CategoryImages/conference.png", 9, "Books" },
                    { 901, "/images/css/CategoryImages/conference.png", 9, "Bookeeping" },
                    { 804, "/images/css/CategoryImages/bookeeping.png", 8, "General" },
                    { 803, "/images/css/CategoryImages/renovation.png", 8, "Hardware" },
                    { 802, "/images/css/CategoryImages/shippment.png", 8, "Cybersecurity" },
                    { 801, "/images/css/CategoryImages/branding.png", 8, "Communications" },
                    { 916, "/images/css/CategoryImages/conference.png", 9, "Shipment" },
                    { 1001, "/images/css/CategoryImages/legal.png", 10, "Conference" },
                    { 1002, "/images/css/CategoryImages/legal.png", 10, "Flight Tickets" },
                    { 1003, "/images/css/CategoryImages/legal.png", 10, "Food" },
                    { 1407, "/images/css/CategoryImages/samples/serum.png", 14, "Serum" },
                    { 1406, "/images/css/CategoryImages/samples/blood.png", 14, "Blood" },
                    { 1405, "/images/css/CategoryImages/samples/bacteria2.png", 14, "Bacteria with Plasmids" },
                    { 1404, "/images/css/CategoryImages/samples/cell1.png", 14, "Cells" },
                    { 1403, "/images/css/CategoryImages/samples/dna_probes2.png", 14, "Probes" },
                    { 1402, "/images/css/CategoryImages/samples/plasmid.png", 14, "Plasmid" },
                    { 1401, "/images/css/CategoryImages/samples/virus.png", 14, "Virus" },
                    { 1302, "/images/css/CategoryImages/general.png", 13, "General" },
                    { 1301, "/images/css/CategoryImages/general.png", 13, "Taxes" },
                    { 703, "/images/css/CategoryImages/clinical/sample_collection.png", 7, "Sample Collection and Processing (Blood, Saliva Collection, Swabsticks, Sepmate, and Ficoll for PBMC, Extraction kits for DNA/RNA)" },
                    { 1203, "/images/css/CategoryImages/taxes.png", 12, "General" },
                    { 1201, "/images/css/CategoryImages/taxes.png", 12, "Regulations" },
                    { 1105, "/images/css/CategoryImages/general.png", 11, "Scientific Advice" },
                    { 1104, "/images/css/CategoryImages/general.png", 11, "Legal" },
                    { 1103, "/images/css/CategoryImages/general.png", 11, "General" },
                    { 1102, "/images/css/CategoryImages/general.png", 11, "Clinical Regulations" },
                    { 1101, "/images/css/CategoryImages/general.png", 11, "Business Advice" },
                    { 1006, "/images/css/CategoryImages/legal.png", 10, "General" },
                    { 1005, "/images/css/CategoryImages/legal.png", 10, "Travel" },
                    { 1004, "/images/css/CategoryImages/legal.png", 10, "Hotels" },
                    { 1202, "/images/css/CategoryImages/taxes.png", 12, "Safety" },
                    { 702, "/images/css/CategoryImages/clinical/serum_tubes.png", 7, "Serum Tubes" },
                    { 701, "/images/css/CategoryImages/clinical/edta_tubes3.png", 7, "EDTA Tubes" },
                    { 601, "/images/css/CategoryImages/general/general.png", 6, "General" },
                    { 201, "/images/css/CategoryImages/reagents/chemical_powder.png", 2, "Chemical Powder" },
                    { 120, "/images/css/CategoryImages/consumables/general.png", 1, "General" },
                    { 119, "/images/css/CategoryImages/consumables/sequencing.png", 1, "Sequencing" },
                    { 118, "/images/css/CategoryImages/consumables/tapestation_consumables.png", 1, "Tapestation Consumables(Screentapes: gDNA/HS/RNA; Markers, Loading Buffers, Loading Tips)" },
                    { 117, "/images/css/CategoryImages/consumables/covaris_consumables.png", 1, "Covaris Consumables" },
                    { 116, "/images/css/CategoryImages/consumables/syringes.png", 1, "Syringes" },
                    { 115, "/images/css/CategoryImages/consumables/bags.png", 1, "Bags" },
                    { 114, "/images/css/CategoryImages/consumables/flasks.png", 1, "Flasks" },
                    { 113, "/images/css/CategoryImages/consumables/filteration_system.png", 1, "Filtration system" },
                    { 202, "/images/css/CategoryImages/reagents/antibody.png", 2, "Antibodies" },
                    { 112, "/images/css/CategoryImages/consumables/column.png", 1, "Column" },
                    { 110, "/images/css/CategoryImages/consumables/fplc_consumables.png", 1, "FPLC Consumables" },
                    { 109, "/images/css/CategoryImages/consumables/rtpcr_consumables.png", 1, "RT-PCR" },
                    { 108, "/images/css/CategoryImages/consumables/ddpcr_consumables.png", 1, "DdPCR Consumables(Gaskets, Cartridges, Microplates, Foil seal)" },
                    { 107, "/images/css/CategoryImages/consumables/robot_consumables_tips.png", 1, "Robot Consumables(Tips,Microplates, Reservoirs)" },
                    { 106, "/images/css/CategoryImages/consumables/tubes.png", 1, "Tubes" },
                    { 105, "/images/css/CategoryImages/consumables/pipettes.png", 1, "Pipets" },
                    { 104, "/images/css/CategoryImages/consumables/tips2.png", 1, "Tips" },
                    { 103, "/images/css/CategoryImages/consumables/petri_dish.png", 1, "Petri Dish" },
                    { 102, "/images/css/CategoryImages/consumables/culture_plates.png", 1, "Cell Culture Plates" },
                    { 111, "/images/css/CategoryImages/consumables/tff_consumables.png", 1, "TFF Consumables" },
                    { 1408, "/images/css/CategoryImages/samples/buffer2.png", 14, "Buffer" },
                    { 203, "/images/css/CategoryImages/reagents/cell_media.png", 2, "Cell Media" },
                    { 205, "/images/css/CategoryImages/reagents/kit.png", 2, "Kit" },
                    { 502, "/images/css/CategoryImages/sagety/safety.png", 5, "Lab Safety" },
                    { 501, "/images/css/CategoryImages/safety/protective_wear.png", 5, "PPE (Personal Protective Equipment)" },
                    { 401, "/images/css/CategoryImages/reusable/all_reusables.png", 4, "Reusable" },
                    { 305, "/images/css/CategoryImages/biological/general.png", 3, "General" },
                    { 304, "/images/css/CategoryImages/biological/bacteria.png", 3, "Bacteria" },
                    { 303, "/images/css/CategoryImages/biological/plasmid2.png", 3, "Plasmid" },
                    { 302, "/images/css/CategoryImages/biological/virus.png", 3, "Virus" },
                    { 301, "/images/css/CategoryImages/biological/cell1.png", 3, "Cells" },
                    { 217, "/images/css/CategoryImages/reagents/general_reagents.png", 2, "General" },
                    { 204, "/images/css/CategoryImages/reagents/chemical_solution2.png", 2, "Solution" },
                    { 216, "/images/css/CategoryImages/reagents/nucleic_acid_quantitation.png", 2, "Nucleic Acid Quantitation(DNA/RNA qubit assay, Picogreen assay)" },
                    { 214, "/images/css/CategoryImages/reagents/fplc_reagent.png", 2, "FPLC Reagent" },
                    { 213, "/images/css/CategoryImages/reagents/rna_enzyme.png", 2, "Enzyme RNA" },
                    { 212, "/images/css/CategoryImages/reagents/restriction_enzyme.png", 2, "Enzyme Restriction" },
                    { 211, "/images/css/CategoryImages/reagents/antibiotics.png", 2, "Antibiotics" },
                    { 210, "/images/css/CategoryImages/reagents/media_supplement.png", 2, "Media Supplement" },
                    { 209, "/images/css/CategoryImages/reagents/primer.png", 2, "Primers" },
                    { 208, "/images/css/CategoryImages/reagents/dna_probes2.png", 2, "Probes" },
                    { 207, "/images/css/CategoryImages/reagents/ddPCR_reagent2.png", 2, "RT-PCR" },
                    { 206, "/images/css/CategoryImages/reagents/PCR_reagent.png", 2, "PCR" },
                    { 215, "/images/css/CategoryImages/reagents/TFF_reagent.png", 2, "TFF Reagent" },
                    { 1409, "/images/css/CategoryImages/samples/media2.png", 14, "Media" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 209);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 214);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 215);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 216);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 217);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 301);

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
                keyValue: 305);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 401);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 501);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 502);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 601);

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
                keyValue: 801);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 802);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 803);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 804);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 901);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 902);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 903);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 904);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 905);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 906);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 907);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 908);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 909);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 910);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 911);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 912);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 913);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 914);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 915);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 916);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1003);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1004);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1005);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1006);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1101);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1102);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1103);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1104);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1105);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1201);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1202);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1203);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1301);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1302);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1401);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1402);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1403);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1404);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1405);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1406);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1407);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1408);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1409);
        }
    }
}
