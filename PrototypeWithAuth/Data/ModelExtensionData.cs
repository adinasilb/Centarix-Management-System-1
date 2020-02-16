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


        }
    }
}
