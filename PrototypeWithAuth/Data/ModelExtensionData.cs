using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
                }
              );
            modelBuilder.Entity<ProductSubcategory>().HasData
              (
                new ProductSubcategory
                {
                    ProductSubcategoryID = 11,
                    ParentCategoryID = 1,
                    ProductSubcategoryDescription = "Tubes"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 12,
                    ParentCategoryID = 1,
                    ProductSubcategoryDescription = "Pipets"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 13,
                    ParentCategoryID = 1,
                    ProductSubcategoryDescription = "Tips"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 14,
                    ParentCategoryID = 1,
                    ProductSubcategoryDescription = "Dishes"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 15,
                    ParentCategoryID = 1,
                    ProductSubcategoryDescription = "Cell Culture Plates"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 21,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "Chemical Powder"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 22,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "DNA Enzyme"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 23,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "Antibodies"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 24,
                    ParentCategoryID = 2,
                    ProductSubcategoryDescription = "Cell Media"
                },

                 new ProductSubcategory
                 {
                     ProductSubcategoryID = 31,
                     ParentCategoryID = 3,
                     ProductSubcategoryDescription = "Virus"
                 },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 32,
                    ParentCategoryID = 3,
                    ProductSubcategoryDescription = "Plasmid"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 33,
                    ParentCategoryID = 3,
                    ProductSubcategoryDescription = "Primers"
                },
             new ProductSubcategory
             {
                 ProductSubcategoryID = 41,
                 ParentCategoryID = 4,
                 ProductSubcategoryDescription = "Beaker"
             },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 42,
                    ParentCategoryID = 4,
                    ProductSubcategoryDescription = "Buckets"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 43,
                    ParentCategoryID = 4,
                    ProductSubcategoryDescription = "Measuring Instruments"
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



        }
    }
}
