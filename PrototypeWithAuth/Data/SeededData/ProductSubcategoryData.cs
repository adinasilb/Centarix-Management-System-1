using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class ProductSubcategoryData
    {
        public static List<ProductSubcategory> Get()
        {
            List<ProductSubcategory> list = new List<ProductSubcategory>();
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 101,
                ParentCategoryID = 1,
                ProductSubcategoryDescription = "PCR",
                ImageURL = "/images/css/CategoryImages/consumables/pcr_consumables.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 102,
                ParentCategoryID = 1,
                ProductSubcategoryDescription = "Cell Culture Plates",
                ImageURL = "/images/css/CategoryImages/consumables/culture_plates.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 103,
                ParentCategoryID = 1,
                ProductSubcategoryDescription = "Petri Dish",
                ImageURL = "/images/css/CategoryImages/consumables/petri_dish.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 104,
                ParentCategoryID = 1,
                ProductSubcategoryDescription = "Tips",
                ImageURL = "/images/css/CategoryImages/consumables/tips2.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 105,
                ParentCategoryID = 1,
                ProductSubcategoryDescription = "Pipets",
                ImageURL = "/images/css/CategoryImages/consumables/pipettes.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 106,
                ParentCategoryID = 1,
                ProductSubcategoryDescription = "Tubes",
                ImageURL = "/images/css/CategoryImages/consumables/tubes.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 107,
                ParentCategoryID = 1,
                ProductSubcategoryDescription = "Robot Consumables",
                ImageURL = "/images/css/CategoryImages/consumables/robot_consumables_tips.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 108,
                ParentCategoryID = 1,
                ProductSubcategoryDescription = "DD-PCR Plastics",
                ImageURL = "/images/css/CategoryImages/consumables/ddpcr_consumables.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 109,
                ParentCategoryID = 1,
                ProductSubcategoryDescription = "Q-PCR Plastics",
                ImageURL = "/images/css/CategoryImages/consumables/rtpcr_consumables.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 110,
                ParentCategoryID = 1,
                ProductSubcategoryDescription = "FPLC Consumables",
                ImageURL = "/images/css/CategoryImages/consumables/fplc_consumables.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 111,
                ParentCategoryID = 1,
                ProductSubcategoryDescription = "TFF Consumables",
                ImageURL = "/images/css/CategoryImages/consumables/tff_consumables.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 112,
                ParentCategoryID = 1,
                ProductSubcategoryDescription = "Column",
                ImageURL = "/images/css/CategoryImages/consumables/column.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 113,
                ParentCategoryID = 1,
                ProductSubcategoryDescription = "Filtration system",
                ImageURL = "/images/css/CategoryImages/consumables/filteration_system.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 114,
                ParentCategoryID = 1,
                ProductSubcategoryDescription = "Flasks",
                ImageURL = "/images/css/CategoryImages/consumables/flasks.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 115,
                ParentCategoryID = 1,
                ProductSubcategoryDescription = "Bags",
                ImageURL = "/images/css/CategoryImages/consumables/bags.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 116,
                ParentCategoryID = 1,
                ProductSubcategoryDescription = "Syringes",
                ImageURL = "/images/css/CategoryImages/consumables/syringes.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 117,
                ParentCategoryID = 1,
                ProductSubcategoryDescription = "Covaris Consumables",
                ImageURL = "/images/css/CategoryImages/consumables/covaris_consumables.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 118,
                ParentCategoryID = 1,
                ProductSubcategoryDescription = "Tapestation consumables",
                ImageURL = "/images/css/CategoryImages/consumables/tapestation_consumables.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 119,
                ParentCategoryID = 1,
                ProductSubcategoryDescription = "Sequencing",
                ImageURL = "/images/css/CategoryImages/consumables/sequencing.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 120,
                ParentCategoryID = 1,
                ProductSubcategoryDescription = "General Consumables",
                ImageURL = "/images/css/CategoryImages/consumables/general.png" // update
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 201,
                ParentCategoryID = 2,
                ProductSubcategoryDescription = "Chemical Powder",
                ImageURL = "/images/css/CategoryImages/reagents/chemical_powder.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 202,
                ParentCategoryID = 2,
                ProductSubcategoryDescription = "Antibody",
                ImageURL = "/images/css/CategoryImages/reagents/antibody.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 203,
                ParentCategoryID = 2,
                ProductSubcategoryDescription = "Cell Media",
                ImageURL = "/images/css/CategoryImages/reagents/cell_media.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 204,
                ParentCategoryID = 2,
                ProductSubcategoryDescription = "",
                ImageURL = "/images/css/CategoryImages/reagents/chemical_solution2.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 205,
                ParentCategoryID = 2,
                ProductSubcategoryDescription = "Kit",
                ImageURL = "/images/css/CategoryImages/reagents/kit.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 206,
                ParentCategoryID = 2,
                ProductSubcategoryDescription = "PCR Reagents",
                ImageURL = "/images/css/CategoryImages/reagents/PCR_reagent.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 207,
                ParentCategoryID = 2,
                ProductSubcategoryDescription = "Q-PCR Reagents",
                ImageURL = "/images/css/CategoryImages/reagents/ddPCR_reagent2.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 208,
                ParentCategoryID = 2,
                ProductSubcategoryDescription = "Probes",
                ImageURL = "/images/css/CategoryImages/reagents/dna_probes2.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 209,
                ParentCategoryID = 2,
                ProductSubcategoryDescription = "Primers and Oligos",
                ImageURL = "/images/css/CategoryImages/reagents/primer.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 210,
                ParentCategoryID = 2,
                ProductSubcategoryDescription = "Cell Media Supplements",
                ImageURL = "/images/css/CategoryImages/reagents/media_supplement.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 211,
                ParentCategoryID = 2,
                ProductSubcategoryDescription = "Antibiotics",
                ImageURL = "/images/css/CategoryImages/reagents/antibiotics.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 212,
                ParentCategoryID = 2,
                ProductSubcategoryDescription = "Restriction Enzyme",
                ImageURL = "/images/css/CategoryImages/reagents/restriction_enzyme.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 213,
                ParentCategoryID = 2,
                ProductSubcategoryDescription = "RNA Enzyme",
                ImageURL = "/images/css/CategoryImages/reagents/rna_enzyme.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 214,
                ParentCategoryID = 2,
                ProductSubcategoryDescription = "FPLC Reagent",
                ImageURL = "/images/css/CategoryImages/reagents/fplc_reagent.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 215,
                ParentCategoryID = 2,
                ProductSubcategoryDescription = "TFF Reagent",
                ImageURL = "/images/css/CategoryImages/reagents/TFF_reagent.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 216,
                ParentCategoryID = 2,
                ProductSubcategoryDescription = "Nucleic Acid Quantitation (DNA/RNA qubit assay, Picogreen assay)",
                ImageURL = "/images/css/CategoryImages/reagents/nucleic_acid_quantitation.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 217,
                ParentCategoryID = 2,
                ProductSubcategoryDescription = "General Reagents and Chemicals",
                ImageURL = "/images/css/CategoryImages/reagents/general_reagents.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 218,
                ParentCategoryID = 2,
                ProductSubcategoryDescription = "DNA Enzymes",
                ImageURL = "/images/css/CategoryImages/reagents/dna_enzyme.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 219,
                ParentCategoryID = 2,
                ProductSubcategoryDescription = "Gas Refilling",
                ImageURL = "/images/css/CategoryImages/reagents/gas_refilling2.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 220,
                ParentCategoryID = 2,
                ProductSubcategoryDescription = "DD-PCR Reagents",
                ImageURL = "/images/css/CategoryImages/reagents/ddPCR_reagent3.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 301,
                ParentCategoryID = 3,
                ProductSubcategoryDescription = "Cells",
                ImageURL = "/images/css/CategoryImages/biological/cell1.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 302,
                ParentCategoryID = 3,
                ProductSubcategoryDescription = "Virus",
                ImageURL = "/images/css/CategoryImages/biological/virus.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 303,
                ParentCategoryID = 3,
                ProductSubcategoryDescription = "Plasmid",
                ImageURL = "/images/css/CategoryImages/biological/plasmid2.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 304,
                ParentCategoryID = 3,
                ProductSubcategoryDescription = "Bacterial Stock",
                ImageURL = "/images/css/CategoryImages/biological/bacteria.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 305,
                ParentCategoryID = 3,
                ProductSubcategoryDescription = "General Biological",
                ImageURL = "/images/css/CategoryImages/biological/general.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 401,
                ParentCategoryID = 4,
                ProductSubcategoryDescription = "Reusable",
                ImageURL = "/images/css/CategoryImages/reusable/all_reusables.png"

            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 501,
                ParentCategoryID = 5,
                ProductSubcategoryDescription = "PPE (Personal Protective Equipment)",
                ImageURL = "/images/css/CategoryImages/safety/protective_wear.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 502,
                ParentCategoryID = 5,
                ProductSubcategoryDescription = "Lab Safety",
                ImageURL = "/images/css/CategoryImages/safety/safety.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 601,
                ParentCategoryID = 6,
                ProductSubcategoryDescription = "General",
                ImageURL = "/images/css/CategoryImages/general/general.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 701,
                ParentCategoryID = 7,
                ProductSubcategoryDescription = "EDTA Tubes",
                ImageURL = "/images/css/CategoryImages/clinical/edta_tubes3.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 702,
                ParentCategoryID = 7,
                ProductSubcategoryDescription = "Serum Tubes",
                ImageURL = "/images/css/CategoryImages/clinical/serum_tubes.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 703,
                ParentCategoryID = 7,
                ProductSubcategoryDescription = "Sample Collection and Processing (Blood, Saliva Collection, Swabsticks, Sepmate, and Ficoll for PBMC, Extraction kits for DNA/RNA)",
                ImageURL = "/images/css/CategoryImages/clinical/sample_collection.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 801,
                ParentCategoryID = 8,
                ProductSubcategoryDescription = "Communications",
                ImageURL = "/images/css/CategoryImages/it/communications.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 802,
                ParentCategoryID = 8,
                ProductSubcategoryDescription = "Cybersecurity",
                ImageURL = "/images/css/CategoryImages/it/cybersecurity.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 803,
                ParentCategoryID = 8,
                ProductSubcategoryDescription = "Hardware",
                ImageURL = "/images/css/CategoryImages/it/hardware3.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 804,
                ParentCategoryID = 8,
                ProductSubcategoryDescription = "General",
                ImageURL = "/images/css/CategoryImages/it/general.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 901,
                ParentCategoryID = 9,
                ProductSubcategoryDescription = "Bookeeping",
                ImageURL = "/images/css/CategoryImages/daytoday/taxes2.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 902,
                ParentCategoryID = 9,
                ProductSubcategoryDescription = "Books",
                ImageURL = "/images/css/CategoryImages/daytoday/books.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 903,
                ParentCategoryID = 9,
                ProductSubcategoryDescription = "Branding",
                ImageURL = "/images/css/CategoryImages/daytoday/branding.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 904,
                ParentCategoryID = 9,
                ProductSubcategoryDescription = "Company Events",
                ImageURL = "/images/css/CategoryImages/daytoday/company_events.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 905,
                ParentCategoryID = 9,
                ProductSubcategoryDescription = "Electricity",
                ImageURL = "/images/css/CategoryImages/daytoday/electricity2.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 906,
                ParentCategoryID = 9,
                ProductSubcategoryDescription = "Fees",
                ImageURL = "/images/css/CategoryImages/daytoday/fees.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 907,
                ParentCategoryID = 9,
                ProductSubcategoryDescription = "Food",
                ImageURL = "/images/css/CategoryImages/daytoday/food.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 908,
                ParentCategoryID = 9,
                ProductSubcategoryDescription = "Furniture",
                ImageURL = "/images/css/CategoryImages/daytoday/furniture2.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 909,
                ParentCategoryID = 9,
                ProductSubcategoryDescription = "General Day To Day",
                ImageURL = "/images/css/CategoryImages/daytoday/general.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 910,
                ParentCategoryID = 9,
                ProductSubcategoryDescription = "Graphic",
                ImageURL = "/images/css/CategoryImages/daytoday/graphics.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 911,
                ParentCategoryID = 9,
                ProductSubcategoryDescription = "Insurance",
                ImageURL = "/images/css/CategoryImages/daytoday/insurance.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 912,
                ParentCategoryID = 9,
                ProductSubcategoryDescription = "Parking",
                ImageURL = "/images/css/CategoryImages/daytoday/parking2.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 913,
                ParentCategoryID = 9,
                ProductSubcategoryDescription = "Renovation",
                ImageURL = "/images/css/CategoryImages/daytoday/renovation.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 914,
                ParentCategoryID = 9,
                ProductSubcategoryDescription = "Rent",
                ImageURL = "/images/css/CategoryImages/daytoday/rent2.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 915,
                ParentCategoryID = 9,
                ProductSubcategoryDescription = "Shipment",
                ImageURL = "/images/css/CategoryImages/daytoday/shippment.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1001,
                ParentCategoryID = 10,
                ProductSubcategoryDescription = "Conference",
                ImageURL = "/images/css/CategoryImages/travel/conference3.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1002,
                ParentCategoryID = 10,
                ProductSubcategoryDescription = "Flight Tickets",
                ImageURL = "/images/css/CategoryImages/travel/flight_tickets.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1003,
                ParentCategoryID = 10,
                ProductSubcategoryDescription = "Food",
                ImageURL = "/images/css/CategoryImages/travel/food.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1004,
                ParentCategoryID = 10,
                ProductSubcategoryDescription = "Hotels",
                ImageURL = "/images/css/CategoryImages/travel/hotels3.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1005,
                ParentCategoryID = 10,
                ProductSubcategoryDescription = "General Travel",
                ImageURL = "/images/css/CategoryImages/travel/travel.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1006,
                ParentCategoryID = 10,
                ProductSubcategoryDescription = "General",
                ImageURL = "/images/css/CategoryImages/travel/general.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1101,
                ParentCategoryID = 11,
                ProductSubcategoryDescription = "Business Advice",
                ImageURL = "/images/css/CategoryImages/advice/business_advice.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1102,
                ParentCategoryID = 11,
                ProductSubcategoryDescription = "Clinical Regulations",
                ImageURL = "/images/css/CategoryImages/advice/clinical_regulation2.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1103,
                ParentCategoryID = 11,
                ProductSubcategoryDescription = "General",
                ImageURL = "/images/css/CategoryImages/advice/general.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1104,
                ParentCategoryID = 11,
                ProductSubcategoryDescription = "Legal",
                ImageURL = "/images/css/CategoryImages/advice/legal.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1105,
                ParentCategoryID = 11,
                ProductSubcategoryDescription = "Scientific Advice",
                ImageURL = "/images/css/CategoryImages/advice/scientific_advice3.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1201,
                ParentCategoryID = 12,
                ProductSubcategoryDescription = "Regulations",
                ImageURL = "/images/css/CategoryImages/regulations/regulations.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1202,
                ParentCategoryID = 12,
                ProductSubcategoryDescription = "Safety",
                ImageURL = "/images/css/CategoryImages/regulations/safety.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1203,
                ParentCategoryID = 12,
                ProductSubcategoryDescription = "General",
                ImageURL = "/images/css/CategoryImages/regulations/general.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1301,
                ParentCategoryID = 13,
                ProductSubcategoryDescription = "Taxes",
                ImageURL = "/images/css/CategoryImages/government/taxes4.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1302,
                ParentCategoryID = 13,
                ProductSubcategoryDescription = "General",
                ImageURL = "/images/css/CategoryImages/government/general.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1401,
                ParentCategoryID = 14,
                ProductSubcategoryDescription = "Virus",
                ImageURL = "/images/css/CategoryImages/samples/virus.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1402,
                ParentCategoryID = 14,
                ProductSubcategoryDescription = "Plasmid",
                ImageURL = "/images/css/CategoryImages/samples/plasmid.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1403,
                ParentCategoryID = 14,
                ProductSubcategoryDescription = "Probes",
                ImageURL = "/images/css/CategoryImages/samples/dna_probes2.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1404,
                ParentCategoryID = 14,
                ProductSubcategoryDescription = "Cells",
                ImageURL = "/images/css/CategoryImages/samples/cell1.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1405,
                ParentCategoryID = 14,
                ProductSubcategoryDescription = "Bacteria with Plasmids",
                ImageURL = "/images/css/CategoryImages/samples/bacteria2.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1406,
                ParentCategoryID = 14,
                ProductSubcategoryDescription = "Blood",
                ImageURL = "/images/css/CategoryImages/samples/blood.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1407,
                ParentCategoryID = 14,
                ProductSubcategoryDescription = "Serum",
                ImageURL = "/images/css/CategoryImages/samples/serum.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1408,
                ParentCategoryID = 14,
                ProductSubcategoryDescription = "Buffer",
                ImageURL = "/images/css/CategoryImages/samples/buffer2.png"
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1409,
                ParentCategoryID = 14,
                ProductSubcategoryDescription = "Media",
                ImageURL = "/images/css/CategoryImages/samples/media2.png"
            });

            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1501,
                ParentCategoryID = 1,
                ProductSubcategoryDescription = "Old Sub category",
                ImageURL = "/images/css/CategoryImages/consumables/general.png",
                IsOldSubCategory = true
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1502,
                ParentCategoryID = 2,
                ProductSubcategoryDescription = "Old Sub category",
                ImageURL = "/images/css/CategoryImages/reagents/general_reagents.png",
                IsOldSubCategory = true
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1503,
                ParentCategoryID = 3,
                ProductSubcategoryDescription = "Old Sub category",
                ImageURL = "/images/css/CategoryImages/biological/general.png",
                IsOldSubCategory = true
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1504,
                ParentCategoryID = 4,
                ProductSubcategoryDescription = "Old Sub category",
                ImageURL = "/images/css/CategoryImages/reusable/all_reusables.png",
                IsOldSubCategory = true
            });
            list.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1505,
                ParentCategoryID = 5,
                ProductSubcategoryDescription = "Old Sub category",
                ImageURL = "/images/css/CategoryImages/safety/safety.png",
                IsOldSubCategory = true
            });
            return list;
        }
    }
}
