using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.Data
{
    public static class ModelExtensionData
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CategoryType>().HasData(
                new CategoryType
                {
                    CategoryTypeID = 1,
                    CategoryTypeDescription = "Lab"
                },
                new CategoryType
                {
                    CategoryTypeID = 2,
                    CategoryTypeDescription = "Operational"
                }
            );
            //if any data is changed in parent categories - must edit parentRequestController monthly view models - had to hard code categories so if add any more have to adjust
            modelBuilder.Entity<ParentCategory>().HasData
           (
               new ParentCategory
               {
                   ParentCategoryID = 1,
                   ParentCategoryDescription = "Plastics",
                   CategoryTypeID = 1
               },
               new ParentCategory
               {
                   ParentCategoryID = 2,
                   ParentCategoryDescription = "Reagents",
                   CategoryTypeID = 1
               },
               new ParentCategory
               {
                   ParentCategoryID = 3,
                   ParentCategoryDescription = "Proprietry",
                   CategoryTypeID = 1
               },
                new ParentCategory
                {
                    ParentCategoryID = 4,
                    ParentCategoryDescription = "Reusable",
                    CategoryTypeID = 1
                },
                new ParentCategory
                {
                    ParentCategoryID = 5,
                    ParentCategoryDescription = "Equipment",
                    CategoryTypeID = 1
                },
                new ParentCategory
                {
                    ParentCategoryID = 6,
                    ParentCategoryDescription = "Operation",
                    CategoryTypeID = 2
                }
            );

            modelBuilder.Entity<ProductSubcategory>().HasData
              (

                new ProductSubcategory
                {
                    ProductSubcategoryID = 101,
                    ParentCategoryID = 1,
                    ProductSubcategoryDescription = "3D Cells Grow"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 102,
                    ParentCategoryID = 1,
                    ProductSubcategoryDescription = "PCR Plates",
                    ImageURL = "/images/css/CategoryImages/PCR.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 103,
                    ParentCategoryID = 1,
                    ProductSubcategoryDescription = "Blood Tubes",
                    ImageURL = "/images/css/CategoryImages/blood_tubes.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 104,
                    ParentCategoryID = 1,
                    ProductSubcategoryDescription = "Cell Culture Plates",
                    ImageURL = "/images/css/CategoryImages/culture_plates.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 105,
                    ParentCategoryID = 1,
                    ProductSubcategoryDescription = "Dishes"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 106,
                    ParentCategoryID = 1,
                    ProductSubcategoryDescription = "Tips"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 107,
                    ParentCategoryID = 1,
                    ProductSubcategoryDescription = "Pipets",
                    ImageURL = "/images/css/CategoryImages/pipettes.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 108,
                    ParentCategoryID = 1,
                    ProductSubcategoryDescription = "Tubes",
                    ImageURL = "/images/css/CategoryImages/tubes.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 201,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "Chemical Powder",
                    ImageURL = "/images/css/CategoryImages/chemical_powder.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 202,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "DNA Enzyme",
                    ImageURL = "/images/css/CategoryImages/dna_enzyme.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 203,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "Antibodies",
                    ImageURL = "/images/css/CategoryImages/antibody.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 204,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "Cell Media",
                    ImageURL = "/images/css/CategoryImages/cell_media.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 205,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "Chemicals Solution",
                    ImageURL = "/images/css/CategoryImages/chemical_solution.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 206,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "Kit",
                    ImageURL = "/images/css/CategoryImages/kit.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 207,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "PCR",
                    ImageURL = "/images/css/CategoryImages/PCR.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 208,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "ddPCR",
                    ImageURL = "/images/css/CategoryImages/ddPCR.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 209,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "RT-PCR"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 210,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "Probes",
                    ImageURL = "/images/css/CategoryImages/dna_probes.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 211,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "Oligo",
                    ImageURL = "/images/css/CategoryImages/oligo.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 212,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "Media Supplement",
                    ImageURL = "/images/css/CategoryImages/media_supplement.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 213,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "Plasmid Purification"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 301,
                    ParentCategoryID = 3,
                    ProductSubcategoryDescription = "Virus",
                    ImageURL = "/images/css/CategoryImages/virus.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 302,
                    ParentCategoryID = 3,
                    ProductSubcategoryDescription = "Plasmid",
                    ImageURL = "/images/css/CategoryImages/plasmid.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 303,
                    ParentCategoryID = 3,
                    ProductSubcategoryDescription = "Primers",
                    ImageURL = "/images/css/CategoryImages/primer.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 304,
                    ParentCategoryID = 3,
                    ProductSubcategoryDescription = "Probes"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 401,
                    ParentCategoryID = 4,
                    ProductSubcategoryDescription = "Beaker",
                    ImageURL = "/images/css/CategoryImages/beaker.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 402,
                    ParentCategoryID = 4,
                    ProductSubcategoryDescription = "Measuring",
                    ImageURL = "/images/css/CategoryImages/measuring.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 403,
                    ParentCategoryID = 4,
                    ProductSubcategoryDescription = "Tube Holders",
                    ImageURL = "/images/css/CategoryImages/tube_holder.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 404,
                    ParentCategoryID = 4,
                    ProductSubcategoryDescription = "Buckets",
                    ImageURL = "/images/css/CategoryImages/bucket.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 405,
                    ParentCategoryID = 4,
                    ProductSubcategoryDescription = "Cooling Racks"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 406,
                    ParentCategoryID = 4,
                    ProductSubcategoryDescription = "-196 Box",
                    ImageURL = "/images/css/CategoryImages/196box.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 407,
                    ParentCategoryID = 4,
                    ProductSubcategoryDescription = "-80 Box",
                    ImageURL = "/images/css/CategoryImages/80box.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 501,
                    ParentCategoryID = 5,
                    ProductSubcategoryDescription = "Instrument"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 502,
                    ParentCategoryID = 5,
                    ProductSubcategoryDescription = "Instrument Parts"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 503,
                    ParentCategoryID = 5,
                    ProductSubcategoryDescription = "Instrument Check"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 504,
                    ParentCategoryID = 5,
                    ProductSubcategoryDescription = "Instrument Fixing"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 505,
                    ParentCategoryID = 5,
                    ProductSubcategoryDescription = "Instrument Calibration"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 506,
                    ParentCategoryID = 5,
                    ProductSubcategoryDescription = "Instrument Warranty"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 507,
                    ParentCategoryID = 5,
                    ProductSubcategoryDescription = "Lab Software"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 508,
                    ParentCategoryID = 5,
                    ProductSubcategoryDescription = "Lab Furniture"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 601,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Computer",
                    ImageURL = "/images/css/CategoryImages/computer.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 602,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Rent",
                    ImageURL = "/images/css/CategoryImages/rent.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 603,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Communication",
                    ImageURL = "/images/css/CategoryImages/communications.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 604,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Branding",
                    ImageURL = "/images/css/CategoryImages/branding.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 605,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Travel",
                    ImageURL = "/images/css/CategoryImages/travel.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 606,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Shipment",
                    ImageURL = "/images/css/CategoryImages/shippment.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 607,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Transportation",
                    ImageURL = "/images/css/CategoryImages/transportation.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 608,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Rennovation",
                    ImageURL = "/images/css/CategoryImages/renovation.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 609,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Bookkeeping",
                    ImageURL = "/images/css/CategoryImages/bookeeping.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 610,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Law Advisement",
                    ImageURL = "/images/css/CategoryImages/legal.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 611,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Tax",
                    ImageURL = "/images/css/CategoryImages/taxes.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 612,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Books And Journal",
                    ImageURL = "/images/css/CategoryImages/books.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 613,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Regulations",
                    ImageURL = "/images/css/CategoryImages/regulations.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 614,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Clinical Regulation",
                    ImageURL = "/images/css/CategoryImages/clinical_regulation.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 615,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Conference",
                    ImageURL = "/images/css/CategoryImages/conference.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 616,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Company Events",
                    ImageURL = "/images/css/CategoryImages/company_events.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 617,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Insurance",
                    ImageURL = "/images/css/CategoryImages/insurance.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 618,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "General",
                    ImageURL = "/images/css/CategoryImages/general.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 619,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Software",
                    ImageURL = "/images/css/CategoryImages/software.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 620,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Hotels",
                    ImageURL = "/images/css/CategoryImages/hotels.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 621,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Flight Tickets",
                    ImageURL = "/images/css/CategoryImages/flight_tickets.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 622,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Scientific Advice",
                    ImageURL = "/images/css/CategoryImages/sciemtific_advice.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 623,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Brokerage"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 624,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Business Advice",
                    ImageURL = "/images/css/CategoryImages/business_advice.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 625,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Electric Appliances",
                    ImageURL = "/images/css/CategoryImages/appliances.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 626,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Safety",
                    ImageURL = "/images/css/CategoryImages/safety.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 627,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Food",
                    ImageURL = "/images/css/CategoryImages/food.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 628,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Stationary",
                    ImageURL = "/images/css/CategoryImages/stationary.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 629,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Furniture",
                    ImageURL = "/images/css/CategoryImages/furniture.png"
                }
              );
            modelBuilder.Entity<RequestStatus>().HasData
          (
              new RequestStatus
              {
                  RequestStatusID = 1,
                  RequestStatusDescription = "New" // request page, under new
              },
              new RequestStatus
              {
                  RequestStatusID = 2,
                  RequestStatusDescription = "Ordered" // request page, under order
              },
              new RequestStatus
              {
                  RequestStatusID = 3,
                  RequestStatusDescription = "RecievedAndIsInventory"  // request page, under recieved (only pass in the first 50) and under Inventory
              },
               new RequestStatus
               {
                   RequestStatusID = 4,
                   RequestStatusDescription = "Partial" // request page, under order
               },
                new RequestStatus
                {
                    RequestStatusID = 5,
                    RequestStatusDescription = "Clarify" // request page, under order
                },
                new RequestStatus
                {
                    RequestStatusID = 6,
                    RequestStatusDescription = "Approved" // request page, under order
                }
             );
            modelBuilder.Entity<UnitParentType>().HasData
         (
         new UnitParentType
         {
             UnitParentTypeID = 1,
             UnitParentTypeDescription = "Units"
         },
         new UnitParentType
         {
             UnitParentTypeID = 2,
             UnitParentTypeDescription = "Weight/Volume"
         },
         new UnitParentType
         {
             UnitParentTypeID = 3,
             UnitParentTypeDescription = "Test"
         }
         );

            modelBuilder.Entity<UnitType>().HasData
         (
             new UnitType
             {
                 UnitTypeID = 1,
                 UnitParentTypeID = 1,
                 UnitTypeDescription = "Bottle"
             },
             new UnitType
             {
                 UnitTypeID = 2,
                 UnitParentTypeID = 1,
                 UnitTypeDescription = "Box"
             },
             new UnitType
             {
                 UnitTypeID = 3,
                 UnitParentTypeID = 1,
                 UnitTypeDescription = "Pack"
             },
             new UnitType
             {
                 UnitTypeID = 4,
                 UnitParentTypeID = 1,
                 UnitTypeDescription = "Bag"
             },
             new UnitType
             {
                 UnitTypeID = 5,
                 UnitParentTypeID = 1,
                 UnitTypeDescription = "Unit"
             },
             new UnitType
             {
                 UnitTypeID = 6,
                 UnitParentTypeID = 1,
                 UnitTypeDescription = "Vial"
             },
             new UnitType
             {
                 UnitTypeID = 7,
                 UnitParentTypeID = 2,
                 UnitTypeDescription = "Kg"
             },
             new UnitType
             {
                 UnitTypeID = 8,
                 UnitParentTypeID = 2,
                 UnitTypeDescription = "gr"
             },
             new UnitType
             {
                 UnitTypeID = 9,
                 UnitParentTypeID = 2,
                 UnitTypeDescription = "mg"
             },
             new UnitType
             {
                 UnitTypeID = 10,
                 UnitParentTypeID = 2,
                 UnitTypeDescription = "ug"
             },
             new UnitType
             {
                 UnitTypeID = 11,
                 UnitParentTypeID = 2,
                 UnitTypeDescription = "Liter"
             },
             new UnitType
             {
                 UnitTypeID = 12,
                 UnitParentTypeID = 2,
                 UnitTypeDescription = "ml"
             },
             new UnitType
             {
                 UnitTypeID = 13,
                 UnitParentTypeID = 2,
                 UnitTypeDescription = "ul"
             },
             new UnitType
             {
                 UnitTypeID = 14,
                 UnitParentTypeID = 2,
                 UnitTypeDescription = "gal"
             },
             new UnitType
             {
                 UnitTypeID = 15,
                 UnitParentTypeID = 3,
                 UnitTypeDescription = "rxhs"
             },
             new UnitType
             {
                 UnitTypeID = 16,
                 UnitParentTypeID = 3,
                 UnitTypeDescription = "test"
             },
             new UnitType
             {
                 UnitTypeID = 17,
                 UnitParentTypeID = 3,
                 UnitTypeDescription = "preps"
             },
             new UnitType
             {
                 UnitTypeID = 18,
                 UnitParentTypeID = 3,
                 UnitTypeDescription = "assays"
             }
            );

            modelBuilder.Entity<Project>().HasData
            (
                new Project
                {
                    ProjectID = 1,
                    ProjectDescription = "Rejuvenation"
                },
                new Project
                {
                    ProjectID = 2,
                    ProjectDescription = "Delivery Systems"
                },
                new Project
                {
                    ProjectID = 3,
                    ProjectDescription = "Biomarkers"
                },
                new Project
                {
                    ProjectID = 4,
                    ProjectDescription = "Clinical Trials"
                },
                new Project
                {
                    ProjectID = 5,
                    ProjectDescription = "General"
                }
            );

            modelBuilder.Entity<SubProject>().HasData
            (
                new SubProject
                {
                    SubProjectID = 101,
                    ProjectID = 1,
                    SubProjectDescription = "Epigenetic Rejuvenation"
                },
                new SubProject
                {
                    SubProjectID = 102,
                    ProjectID = 1,
                    SubProjectDescription = "Plasma Rejuvenation"
                },
                new SubProject
                {
                    SubProjectID = 201,
                    ProjectID = 2,
                    SubProjectDescription = "AAV"
                },
                new SubProject
                {
                    SubProjectID = 301,
                    ProjectID = 3,
                    SubProjectDescription = "Epigenetic Clock"
                },
                new SubProject
                {
                    SubProjectID = 302,
                    ProjectID = 3,
                    SubProjectDescription = "Telomere Measurement"
                },
                new SubProject
                {
                    SubProjectID = 401,
                    ProjectID = 4,
                    SubProjectDescription = "Biomarker Trial"
                },
                new SubProject
                {
                    SubProjectID = 501,
                    ProjectID = 5,
                    SubProjectDescription = "General"
                }
            );

            modelBuilder.Entity<Menu>().HasData(
                new Menu
                {
                    menuID = 1,
                    MenuDescription = AppUtility.MenuItems.OrdersAndInventory.ToString(),
                    MenuViewName = "Orders & Inventory",
                    ControllerName = "Requests",
                    ActionName = "Index",
                    MenuImageURL = "/images/css/main_menu_icons/inventory.png"
                },
                new Menu
                {
                    menuID = 2,
                    MenuDescription = AppUtility.MenuItems.Protocols.ToString(),
                    MenuViewName = "Protocols",
                    ControllerName = "",
                    ActionName = "",
                    MenuImageURL = "/images/css/main_menu_icons/protocols.png"
                },
                new Menu
                {
                    menuID = 3,
                    MenuDescription = AppUtility.MenuItems.Operation.ToString(),
                    MenuViewName = "Operation",
                    ControllerName = "",
                    ActionName = "",
                    MenuImageURL = "/images/css/main_menu_icons/operation.png"
                },
                new Menu
                {
                    menuID = 4,
                    MenuDescription = AppUtility.MenuItems.Biomarkers.ToString(),
                    MenuViewName = "Biomarkers",
                    ControllerName = "",
                    ActionName = "",
                    MenuImageURL = "/images/css/main_menu_icons/biomarkers.png"
                },
                new Menu
                {
                    menuID = 5,
                    MenuDescription = AppUtility.MenuItems.TimeKeeper.ToString(),
                    MenuViewName = "Time Keeper",
                    ControllerName = "",
                    ActionName = "",
                    MenuImageURL = "/images/css/main_menu_icons/timekeeper.png"
                },
                new Menu
                {
                    menuID = 6,
                    MenuDescription = AppUtility.MenuItems.LabManagement.ToString(),
                    MenuViewName = "Lab Management",
                    ControllerName = "Vendors",
                    ActionName = "IndexForLabManage",
                    MenuImageURL = "/images/css/main_menu_icons/lab.png"
                },
                new Menu
                {
                    menuID = 7,
                    MenuDescription = AppUtility.MenuItems.Accounting.ToString(),
                    MenuViewName = "Accounting",
                    ControllerName = "Requests",
                    ActionName = "AccountingPayments",
                    MenuImageURL = "/images/css/main_menu_icons/accounting.png"
                },
                new Menu
                {
                    menuID = 8,
                    MenuDescription = AppUtility.MenuItems.Expenses.ToString(),
                    MenuViewName = "Expenses",
                    ControllerName = "ParentRequests",
                    ActionName = "ExpensesList",
                    MenuImageURL = "/images/css/main_menu_icons/expenses.png"
                },
                new Menu
                {
                    menuID = 9,
                    MenuDescription = AppUtility.MenuItems.Income.ToString(),
                    MenuViewName = "Income",
                    ControllerName = "",
                    ActionName = "",
                    MenuImageURL = "/images/css/main_menu_icons/income.png"
                },
                new Menu
                {
                    menuID = 10,
                    MenuDescription = AppUtility.MenuItems.Users.ToString(),
                    MenuViewName = "Users",
                    ControllerName = "Admin",
                    ActionName = "Index",
                    MenuImageURL = "/images/css/main_menu_icons/users.png"
                }
            );
            modelBuilder.Entity<QuoteStatus>().HasData(
                new QuoteStatus
                {
                    QuoteStatusID = -1,
                    QuoteStatusDescription = "NoStatus" // request page, under reorder
                },
                new QuoteStatus
                {
                    QuoteStatusID = 1,
                    QuoteStatusDescription = "AwaitingRequestOfQuote" // request page, under reorder
                },
                new QuoteStatus
                {
                    QuoteStatusID = 2,
                    QuoteStatusDescription = "AwaitingQuoteResponse" // lab quote manange page, under quotes
                },
                 new QuoteStatus
                 {
                     QuoteStatusID = 3,
                     QuoteStatusDescription = "AwaitingQuoteOrder" // lab quote manange page, under quotes
                 },
                  new QuoteStatus
                  {
                      QuoteStatusID = 4,
                      QuoteStatusDescription = "QuoteRecieved" 
                  }
            );

        }
    }
}
