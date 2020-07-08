using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.Data
{
    public static class ModelExtensionData
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //if any data is changed in parent categories - must edit parentRequestController monthly view models - had to hard code categories so if add any more have to adjust
            modelBuilder.Entity<ParentCategory>().HasData
           (
               new ParentCategory
               {
                   ParentCategoryID = 1,
                   ParentCategoryDescription = "Plastics"
               },
               new ParentCategory
               {
                   ParentCategoryID = 2,
                   ParentCategoryDescription = "Reagents"
               },
               new ParentCategory
               {
                   ParentCategoryID = 3,
                   ParentCategoryDescription = "Proprietry"
               },
                new ParentCategory
                {
                    ParentCategoryID = 4,
                    ParentCategoryDescription = "Reusable"
                },
                new ParentCategory
                {
                    ParentCategoryID = 5,
                    ParentCategoryDescription = "Equipment"
                },
                new ParentCategory
                {
                    ParentCategoryID = 6,
                    ParentCategoryDescription = "Operation"
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
                    ProductSubcategoryDescription = "PCR Plates"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 103,
                    ParentCategoryID = 1,
                    ProductSubcategoryDescription = "Blood"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 104,
                    ParentCategoryID = 1,
                    ProductSubcategoryDescription = "Cell Culture Plates"
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
                    ProductSubcategoryDescription = "Pipets"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 108,
                    ParentCategoryID = 1,
                    ProductSubcategoryDescription = "Tubes"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 201,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "Chemical Powder"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 202,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "DNA Enzyme"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 203,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "Antibodies"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 204,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "Cell Media"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 205,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "Chemicals Solution"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 206,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "Kit"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 207,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "PCR"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 208,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "ddPCR"
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
                    ProductSubcategoryDescription = "Probes"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 211,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "Oligo"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 212,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "Media Supplement"
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
                    ProductSubcategoryDescription = "Virus"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 302,
                    ParentCategoryID = 3,
                    ProductSubcategoryDescription = "Plasmid"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 303,
                    ParentCategoryID = 3,
                    ProductSubcategoryDescription = "Primers"
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
                    ProductSubcategoryDescription = "Beaker"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 402,
                    ParentCategoryID = 4,
                    ProductSubcategoryDescription = "Measuring"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 403,
                    ParentCategoryID = 4,
                    ProductSubcategoryDescription = "Tube Holders"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 404,
                    ParentCategoryID = 4,
                    ProductSubcategoryDescription = "Buckets"
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
                    ProductSubcategoryDescription = "-196 Box"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 407,
                    ParentCategoryID = 4,
                    ProductSubcategoryDescription = "-80 Box"
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
                    ProductSubcategoryDescription = "Computer"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 602,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Rent"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 603,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Communication"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 604,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Branding"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 605,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Travel"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 606,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Shipment"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 607,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Transportation"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 608,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Rennovation"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 609,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Bookkeeping"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 610,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Law Advisement"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 611,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Tax"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 612,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Books And Journal"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 613,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Regulations"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 614,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Clinical Regulation"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 615,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Conference"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 616,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Company Events"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 617,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Insurance"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 618,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "General"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 619,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Software"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 620,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Hotels"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 621,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Flight Tickets"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 622,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Scientific Advice"
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
                    ProductSubcategoryDescription = "Business Advice"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 625,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Electric Appliances"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 626,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Safety"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 627,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Food"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 628,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Stationary"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 629,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Furniture"
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
                }
                ,
                new RequestStatus
                {
                    RequestStatusID = 6,
                    RequestStatusDescription = "AwaitingOrder" // request page, under order
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


        }
    }
}
