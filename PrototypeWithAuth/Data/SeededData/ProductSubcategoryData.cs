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
                ID = 101,
                ParentCategoryID = 1,
                Description = "PCR",
                ImageURL = "/images/css/CategoryImages/consumables/pcr_consumables.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 102,
                ParentCategoryID = 1,
                Description = "Cell Culture Plates",
                ImageURL = "/images/css/CategoryImages/consumables/culture_plates.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 103,
                ParentCategoryID = 1,
                Description = "Petri Dish",
                ImageURL = "/images/css/CategoryImages/consumables/petri_dish.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 104,
                ParentCategoryID = 1,
                Description = "Tips",
                ImageURL = "/images/css/CategoryImages/consumables/tips2.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 105,
                ParentCategoryID = 1,
                Description = "Pipets",
                ImageURL = "/images/css/CategoryImages/consumables/pipettes.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 106,
                ParentCategoryID = 1,
                Description = "Tubes",
                ImageURL = "/images/css/CategoryImages/consumables/tubes.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 107,
                ParentCategoryID = 1,
                Description = "Robot Consumables",
                ImageURL = "/images/css/CategoryImages/consumables/robot_consumables_tips.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 108,
                ParentCategoryID = 1,
                Description = "DD-PCR Plastics",
                ImageURL = "/images/css/CategoryImages/consumables/ddpcr_consumables.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 109,
                ParentCategoryID = 1,
                Description = "Q-PCR Plastics",
                ImageURL = "/images/css/CategoryImages/consumables/rtpcr_consumables.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 110,
                ParentCategoryID = 1,
                Description = "FPLC Consumables",
                ImageURL = "/images/css/CategoryImages/consumables/fplc_consumables.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 111,
                ParentCategoryID = 1,
                Description = "TFF Consumables",
                ImageURL = "/images/css/CategoryImages/consumables/tff_consumables.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 112,
                ParentCategoryID = 1,
                Description = "Column",
                ImageURL = "/images/css/CategoryImages/consumables/column.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 113,
                ParentCategoryID = 1,
                Description = "Filtration system",
                ImageURL = "/images/css/CategoryImages/consumables/filteration_system.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 114,
                ParentCategoryID = 1,
                Description = "Flasks",
                ImageURL = "/images/css/CategoryImages/consumables/flasks.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 115,
                ParentCategoryID = 1,
                Description = "Bags",
                ImageURL = "/images/css/CategoryImages/consumables/bags.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 116,
                ParentCategoryID = 1,
                Description = "Syringes",
                ImageURL = "/images/css/CategoryImages/consumables/syringes.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 117,
                ParentCategoryID = 1,
                Description = "Covaris Consumables",
                ImageURL = "/images/css/CategoryImages/consumables/covaris_consumables.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 118,
                ParentCategoryID = 1,
                Description = "Tapestation consumables",
                ImageURL = "/images/css/CategoryImages/consumables/tapestation_consumables.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 119,
                ParentCategoryID = 1,
                Description = "Sequencing",
                ImageURL = "/images/css/CategoryImages/consumables/sequencing.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 120,
                ParentCategoryID = 1,
                Description = "General Consumables",
                ImageURL = "/images/css/CategoryImages/consumables/general.png" // update
            });
            list.Add(new ProductSubcategory
            {
                ID = 201,
                ParentCategoryID = 2,
                Description = "Chemical Powder",
                ImageURL = "/images/css/CategoryImages/reagents/chemical_powder.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 202,
                ParentCategoryID = 2,
                Description = "Antibody",
                ImageURL = "/images/css/CategoryImages/reagents/antibody.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 203,
                ParentCategoryID = 2,
                Description = "Cell Media",
                ImageURL = "/images/css/CategoryImages/reagents/cell_media.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 204,
                ParentCategoryID = 2,
                Description = "Solutions",
                ImageURL = "/images/css/CategoryImages/reagents/chemical_solution2.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 205,
                ParentCategoryID = 2,
                Description = "Kit",
                ImageURL = "/images/css/CategoryImages/reagents/kit.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 206,
                ParentCategoryID = 2,
                Description = "PCR Reagents",
                ImageURL = "/images/css/CategoryImages/reagents/PCR_reagent.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 207,
                ParentCategoryID = 2,
                Description = "Q-PCR Reagents",
                ImageURL = "/images/css/CategoryImages/reagents/ddPCR_reagent2.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 208,
                ParentCategoryID = 2,
                Description = "Probes",
                ImageURL = "/images/css/CategoryImages/reagents/dna_probes2.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 209,
                ParentCategoryID = 2,
                Description = "Primers and Oligos",
                ImageURL = "/images/css/CategoryImages/reagents/primer.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 210,
                ParentCategoryID = 2,
                Description = "Cell Media Supplements",
                ImageURL = "/images/css/CategoryImages/reagents/media_supplement.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 211,
                ParentCategoryID = 2,
                Description = "Antibiotics",
                ImageURL = "/images/css/CategoryImages/reagents/antibiotics.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 212,
                ParentCategoryID = 2,
                Description = "Restriction Enzyme",
                ImageURL = "/images/css/CategoryImages/reagents/restriction_enzyme.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 213,
                ParentCategoryID = 2,
                Description = "RNA Enzyme",
                ImageURL = "/images/css/CategoryImages/reagents/rna_enzyme.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 214,
                ParentCategoryID = 2,
                Description = "FPLC Reagent",
                ImageURL = "/images/css/CategoryImages/reagents/fplc_reagent.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 215,
                ParentCategoryID = 2,
                Description = "TFF Reagent",
                ImageURL = "/images/css/CategoryImages/reagents/TFF_reagent.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 216,
                ParentCategoryID = 2,
                Description = "Nucleic Acid Quantitation (DNA/RNA qubit assay, Picogreen assay)",
                ImageURL = "/images/css/CategoryImages/reagents/nucleic_acid_quantitation.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 217,
                ParentCategoryID = 2,
                Description = "General Reagents and Chemicals",
                ImageURL = "/images/css/CategoryImages/reagents/general_reagents.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 218,
                ParentCategoryID = 2,
                Description = "DNA Enzymes",
                ImageURL = "/images/css/CategoryImages/reagents/dna_enzyme.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 219,
                ParentCategoryID = 2,
                Description = "Gas Refilling",
                ImageURL = "/images/css/CategoryImages/reagents/gas_refilling2.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 220,
                ParentCategoryID = 2,
                Description = "DD-PCR Reagents",
                ImageURL = "/images/css/CategoryImages/reagents/ddPCR_reagent3.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 301,
                ParentCategoryID = 3,
                Description = "Cells",
                ImageURL = "/images/css/CategoryImages/biological/cell1.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 302,
                ParentCategoryID = 3,
                Description = "Virus",
                ImageURL = "/images/css/CategoryImages/biological/virus.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 303,
                ParentCategoryID = 3,
                Description = "Plasmid",
                ImageURL = "/images/css/CategoryImages/biological/plasmid2.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 304,
                ParentCategoryID = 3,
                Description = "Bacterial Stock",
                ImageURL = "/images/css/CategoryImages/biological/bacteria.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 305,
                ParentCategoryID = 3,
                Description = "General Biological",
                ImageURL = "/images/css/CategoryImages/biological/general.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 401,
                ParentCategoryID = 4,
                Description = "Reusable",
                ImageURL = "/images/css/CategoryImages/reusable/all_reusables.png"

            });
            list.Add(new ProductSubcategory
            {
                ID = 501,
                ParentCategoryID = 5,
                Description = "PPE (Personal Protective Equipment)",
                ImageURL = "/images/css/CategoryImages/safety/protective_wear.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 502,
                ParentCategoryID = 5,
                Description = "Lab Safety",
                ImageURL = "/images/css/CategoryImages/safety/safety.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 601,
                ParentCategoryID = 6,
                Description = "General",
                ImageURL = "/images/css/CategoryImages/general/general.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 701,
                ParentCategoryID = 7,
                Description = "EDTA Tubes",
                ImageURL = "/images/css/CategoryImages/clinical/edta_tubes3.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 702,
                ParentCategoryID = 7,
                Description = "Serum Tubes",
                ImageURL = "/images/css/CategoryImages/clinical/serum_tubes.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 703,
                ParentCategoryID = 7,
                Description = "Sample Collection and Processing (Blood, Saliva Collection, Swabsticks, Sepmate, and Ficoll for PBMC, Extraction kits for DNA/RNA)",
                ImageURL = "/images/css/CategoryImages/clinical/sample_collection.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 801,
                ParentCategoryID = 8,
                Description = "Communications",
                ImageURL = "/images/css/CategoryImages/it/communications.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 802,
                ParentCategoryID = 8,
                Description = "Cybersecurity",
                ImageURL = "/images/css/CategoryImages/it/cybersecurity.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 803,
                ParentCategoryID = 8,
                Description = "Hardware",
                ImageURL = "/images/css/CategoryImages/it/hardware3.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 804,
                ParentCategoryID = 8,
                Description = "General",
                ImageURL = "/images/css/CategoryImages/it/general.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 901,
                ParentCategoryID = 9,
                Description = "Bookeeping",
                ImageURL = "/images/css/CategoryImages/daytoday/taxes2.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 902,
                ParentCategoryID = 9,
                Description = "Books",
                ImageURL = "/images/css/CategoryImages/daytoday/books.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 903,
                ParentCategoryID = 9,
                Description = "Branding",
                ImageURL = "/images/css/CategoryImages/daytoday/branding.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 904,
                ParentCategoryID = 9,
                Description = "Company Events",
                ImageURL = "/images/css/CategoryImages/daytoday/company_events.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 905,
                ParentCategoryID = 9,
                Description = "Electricity",
                ImageURL = "/images/css/CategoryImages/daytoday/electricity2.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 906,
                ParentCategoryID = 9,
                Description = "Fees",
                ImageURL = "/images/css/CategoryImages/daytoday/fees.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 907,
                ParentCategoryID = 9,
                Description = "Food",
                ImageURL = "/images/css/CategoryImages/daytoday/food.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 908,
                ParentCategoryID = 9,
                Description = "Furniture",
                ImageURL = "/images/css/CategoryImages/daytoday/furniture2.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 909,
                ParentCategoryID = 9,
                Description = "General Day To Day",
                ImageURL = "/images/css/CategoryImages/daytoday/general.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 910,
                ParentCategoryID = 9,
                Description = "Graphic",
                ImageURL = "/images/css/CategoryImages/daytoday/graphics.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 911,
                ParentCategoryID = 9,
                Description = "Insurance",
                ImageURL = "/images/css/CategoryImages/daytoday/insurance.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 912,
                ParentCategoryID = 9,
                Description = "Parking",
                ImageURL = "/images/css/CategoryImages/daytoday/parking2.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 913,
                ParentCategoryID = 9,
                Description = "Renovation",
                ImageURL = "/images/css/CategoryImages/daytoday/renovation.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 914,
                ParentCategoryID = 9,
                Description = "Rent",
                ImageURL = "/images/css/CategoryImages/daytoday/rent2.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 915,
                ParentCategoryID = 9,
                Description = "Shipment",
                ImageURL = "/images/css/CategoryImages/daytoday/shippment.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1001,
                ParentCategoryID = 10,
                Description = "Conference",
                ImageURL = "/images/css/CategoryImages/travel/conference3.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1002,
                ParentCategoryID = 10,
                Description = "Flight Tickets",
                ImageURL = "/images/css/CategoryImages/travel/flight_tickets.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1003,
                ParentCategoryID = 10,
                Description = "Food",
                ImageURL = "/images/css/CategoryImages/travel/food.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1004,
                ParentCategoryID = 10,
                Description = "Hotels",
                ImageURL = "/images/css/CategoryImages/travel/hotels3.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1005,
                ParentCategoryID = 10,
                Description = "General Travel",
                ImageURL = "/images/css/CategoryImages/travel/travel.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1006,
                ParentCategoryID = 10,
                Description = "General",
                ImageURL = "/images/css/CategoryImages/travel/general.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1101,
                ParentCategoryID = 11,
                Description = "Business Advice",
                ImageURL = "/images/css/CategoryImages/advice/business_advice.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1102,
                ParentCategoryID = 11,
                Description = "Clinical Regulations",
                ImageURL = "/images/css/CategoryImages/advice/clinical_regulation2.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1103,
                ParentCategoryID = 11,
                Description = "General",
                ImageURL = "/images/css/CategoryImages/advice/general.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1104,
                ParentCategoryID = 11,
                Description = "Legal",
                ImageURL = "/images/css/CategoryImages/advice/legal.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1105,
                ParentCategoryID = 11,
                Description = "Scientific Advice",
                ImageURL = "/images/css/CategoryImages/advice/scientific_advice3.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1201,
                ParentCategoryID = 12,
                Description = "Regulations",
                ImageURL = "/images/css/CategoryImages/regulations/regulations.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1202,
                ParentCategoryID = 12,
                Description = "Safety",
                ImageURL = "/images/css/CategoryImages/regulations/safety.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1203,
                ParentCategoryID = 12,
                Description = "General",
                ImageURL = "/images/css/CategoryImages/regulations/general.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1301,
                ParentCategoryID = 13,
                Description = "Taxes",
                ImageURL = "/images/css/CategoryImages/government/taxes4.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1302,
                ParentCategoryID = 13,
                Description = "General",
                ImageURL = "/images/css/CategoryImages/government/general.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1401,
                ParentCategoryID = 14,
                Description = "Virus",
                ImageURL = "/images/css/CategoryImages/samples/virus.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1402,
                ParentCategoryID = 14,
                Description = "Plasmid",
                ImageURL = "/images/css/CategoryImages/samples/plasmid.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1403,
                ParentCategoryID = 14,
                Description = "Probes",
                ImageURL = "/images/css/CategoryImages/samples/dna_probes2.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1404,
                ParentCategoryID = 14,
                Description = "Cells",
                ImageURL = "/images/css/CategoryImages/samples/cell1.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1405,
                ParentCategoryID = 14,
                Description = "Bacteria with Plasmids",
                ImageURL = "/images/css/CategoryImages/samples/bacteria2.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1406,
                ParentCategoryID = 14,
                Description = "Blood",
                ImageURL = "/images/css/CategoryImages/samples/blood.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1407,
                ParentCategoryID = 14,
                Description = "Serum",
                ImageURL = "/images/css/CategoryImages/samples/serum.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1408,
                ParentCategoryID = 14,
                Description = "Buffer",
                ImageURL = "/images/css/CategoryImages/samples/buffer2.png"
            });
            list.Add(new ProductSubcategory
            {
                ID = 1409,
                ParentCategoryID = 14,
                Description = "Media",
                ImageURL = "/images/css/CategoryImages/samples/media2.png"
            });

            list.Add(new ProductSubcategory
            {
                ID = 1501,
                ParentCategoryID = 1,
                Description = "Old Sub category",
                ImageURL = "/images/css/CategoryImages/consumables/general.png",
                IsOldSubCategory = true
            });
            list.Add(new ProductSubcategory
            {
                ID = 1502,
                ParentCategoryID = 2,
                Description = "Old Sub category",
                ImageURL = "/images/css/CategoryImages/reagents/general_reagents.png",
                IsOldSubCategory = true
            });
            list.Add(new ProductSubcategory
            {
                ID = 1503,
                ParentCategoryID = 3,
                Description = "Old Sub category",
                ImageURL = "/images/css/CategoryImages/biological/general.png",
                IsOldSubCategory = true
            });
            list.Add(new ProductSubcategory
            {
                ID = 1504,
                ParentCategoryID = 4,
                Description = "Old Sub category",
                ImageURL = "/images/css/CategoryImages/reusable/all_reusables.png",
                IsOldSubCategory = true
            });
            list.Add(new ProductSubcategory
            {
                ID = 1505,
                ParentCategoryID = 5,
                Description = "Old Sub category",
                ImageURL = "/images/css/CategoryImages/safety/safety.png",
                IsOldSubCategory = true
            });
            return list;
        }
    }
}
