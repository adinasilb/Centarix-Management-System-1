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
            modelBuilder.Entity<PaymentStatus>().HasData(
                new PaymentStatus
                {
                    PaymentStatusID = 2,
                    PaymentStatusDescription = "+ 30"
                },
                new PaymentStatus
                {
                    PaymentStatusID = 3,
                    PaymentStatusDescription = "Pay Now"
                },
                new PaymentStatus
                {
                    PaymentStatusID = 4,
                    PaymentStatusDescription = "Pay Upon Arrival"
                },
                new PaymentStatus
                {
                    PaymentStatusID = 5,
                    PaymentStatusDescription = "Installments"
                },
                 new PaymentStatus
                 {
                     PaymentStatusID = 7,
                     PaymentStatusDescription = "Standing Order"
                 },
                new PaymentStatus
                {
                    PaymentStatusID = 8,
                    PaymentStatusDescription = "Specify Payment Later"
                }
            );

            modelBuilder.Entity<PaymentType>().HasData(
               new PaymentType
               {
                   PaymentTypeID = 1,
                   PaymentTypeDescription = "Credit Card"

               },
               new PaymentType
               {
                   PaymentTypeID = 2,
                   PaymentTypeDescription = "Check"
               },
               new PaymentType
               {
                   PaymentTypeID = 3,
                   PaymentTypeDescription = "Wire"
               }
           );

            modelBuilder.Entity<CompanyAccount>().HasData(
                new CompanyAccount
                {
                    CompanyAccountID = 1,
                    CompanyBankName = "Discount"
                },
                new CompanyAccount
                {
                    CompanyAccountID = 2,
                    CompanyBankName = "Mercantile"
                },
                new CompanyAccount
                {
                    CompanyAccountID = 3,
                    CompanyBankName = "Leumi"
                },
                new CompanyAccount
                {
                    CompanyAccountID = 4,
                    CompanyBankName = "Payoneer"
                }
            );

            modelBuilder.Entity<CreditCard>().HasData(
                new CreditCard
                {
                    CreditCardID = 1,
                    CompanyAccountID = 2,
                    CardNumber = "2543"
                },
                new CreditCard
                {
                    CreditCardID = 2,
                    CompanyAccountID = 2,
                    CardNumber = "4694"
                },
                new CreditCard
                {
                    CreditCardID = 3,
                    CompanyAccountID = 2,
                    CardNumber = "3485"
                },
                new CreditCard
                {
                    CreditCardID = 4,
                    CompanyAccountID = 2,
                    CardNumber = "0054"
                },
                new CreditCard
                {
                    CreditCardID = 5,
                    CompanyAccountID = 1,
                    CardNumber = "4971"
                },
                new CreditCard
                {
                    CreditCardID = 6,
                    CompanyAccountID = 1,
                    CardNumber = "4424"
                },
                new CreditCard
                {
                    CreditCardID = 7,
                    CompanyAccountID = 1,
                    CardNumber = "4432"
                },
                new CreditCard
                {
                    CreditCardID = 8,
                    CompanyAccountID = 3,
                    CardNumber = "7972"
                }
           );
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
                   ParentCategoryDescription = "Reagents And Chemicals",
                   CategoryTypeID = 1
               },
               new ParentCategory
               {
                   ParentCategoryID = 3,
                   ParentCategoryDescription = "Cells",
                   CategoryTypeID = 1
               },
                new ParentCategory
                {
                    ParentCategoryID = 4,
                    ParentCategoryDescription = "Reusables",
                    CategoryTypeID = 1
                },
                //new ParentCategory
                //{
                //    ParentCategoryID = 5,
                //    ParentCategoryDescription = "Equipment",
                //    CategoryTypeID = 1
                //},
                new ParentCategory
                {
                    ParentCategoryID = 6,
                    ParentCategoryDescription = "IT",
                    CategoryTypeID = 2
                },
                new ParentCategory
                {
                    ParentCategoryID = 8,
                    ParentCategoryDescription = "Day To Day",
                    CategoryTypeID = 2
                },
                new ParentCategory
                {
                    ParentCategoryID = 9,
                    ParentCategoryDescription = "Travel",
                    CategoryTypeID = 2
                },
                new ParentCategory
                {
                    ParentCategoryID = 10,
                    ParentCategoryDescription = "Advisment",
                    CategoryTypeID = 2
                },
                new ParentCategory
                {
                    ParentCategoryID = 11,
                    ParentCategoryDescription = "Regulations",
                    CategoryTypeID = 2
                },
                new ParentCategory
                {
                    ParentCategoryID = 12,
                    ParentCategoryDescription = "Governments",
                    CategoryTypeID = 2
                },
                new ParentCategory
                {
                    ParentCategoryID = 13,
                    ParentCategoryDescription = "General",
                    CategoryTypeID = 2
                },
                new ParentCategory
                {
                    ParentCategoryID = 7,
                    ParentCategoryDescription = "Proprietary",
                    CategoryTypeID = 1,
                    isProprietary = true
                }
            );

            modelBuilder.Entity<UnitTypeParentCategory>().HasData
              (
                new UnitTypeParentCategory
                {
                    UnitTypeID = 1,
                    ParentCategoryID = 1
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 2,
                    ParentCategoryID = 1
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 19,
                    ParentCategoryID = 1
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 3,
                    ParentCategoryID = 1
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 5,
                    ParentCategoryID = 1
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 24,
                    ParentCategoryID = 1
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 17,
                    ParentCategoryID = 2
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 18,
                    ParentCategoryID = 2
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 1,
                    ParentCategoryID = 2
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 2,
                    ParentCategoryID = 2
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 19,
                    ParentCategoryID = 2
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 3,
                    ParentCategoryID = 2
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 5,
                    ParentCategoryID = 2
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 7,
                    ParentCategoryID = 2
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 8,
                    ParentCategoryID = 2
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 9,
                    ParentCategoryID = 2
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 10,
                    ParentCategoryID = 2
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 11,
                    ParentCategoryID = 2
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 12,
                    ParentCategoryID = 2
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 13,
                    ParentCategoryID = 2
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 20,
                    ParentCategoryID = 2
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 21,
                    ParentCategoryID = 2
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 22,
                    ParentCategoryID = 2
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 23,
                    ParentCategoryID = 2
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 24,
                    ParentCategoryID = 2
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 5,
                    ParentCategoryID = 3
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 1,
                    ParentCategoryID = 4
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 2,
                    ParentCategoryID = 4
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 19,
                    ParentCategoryID = 4
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 3,
                    ParentCategoryID = 4
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 5,
                    ParentCategoryID = 4
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 24,
                    ParentCategoryID = 4
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 5,
                    ParentCategoryID = 7
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 10,
                    ParentCategoryID = 7
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 9,
                    ParentCategoryID = 7
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 20,
                    ParentCategoryID = 7
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 21,
                    ParentCategoryID = 7
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 22,
                    ParentCategoryID = 7
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 13,
                    ParentCategoryID = 7
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 12,
                    ParentCategoryID = 7
                },
                new UnitTypeParentCategory
                {
                    UnitTypeID = 11,
                    ParentCategoryID = 7
                }
              );

            modelBuilder.Entity<ProductSubcategory>().HasData
              (

                //new ProductSubcategory
                //{
                //    ProductSubcategoryID = 101,
                //    ParentCategoryID = 1,
                //    ProductSubcategoryDescription = "3D Cells Grow"
                //},
                new ProductSubcategory
                {
                    ProductSubcategoryID = 102,
                    ParentCategoryID = 1,
                    ProductSubcategoryDescription = "PCR",
                    ImageURL = "/images/css/CategoryImages/PCR.png"
                },
                //new ProductSubcategory
                //{
                //    ProductSubcategoryID = 103,
                //    ParentCategoryID = 1,
                //    ProductSubcategoryDescription = "Blood Tubes",
                //    ImageURL = "/images/css/CategoryImages/blood_tubes.png"
                //},
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
                    ProductSubcategoryDescription = "Petri Dishes"
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
                    ProductSubcategoryID = 109,
                    ParentCategoryID = 1,
                    ProductSubcategoryDescription = "Robot Tips"
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
                    ProductSubcategoryDescription = "Enzyme",
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
                //new ProductSubcategory
                //{
                //    ProductSubcategoryID = 213,
                //    ParentCategoryID = 2,
                //    ProductSubcategoryDescription = "Plasmid Purification"
                //},
                new ProductSubcategory
                {
                    ProductSubcategoryID = 301,
                    ParentCategoryID = 3,
                    ProductSubcategoryDescription = "Cells"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 401,
                    ParentCategoryID = 4,
                    ProductSubcategoryDescription = "Reusables"
                },
                //new ProductSubcategory
                //{
                //    ProductSubcategoryID = 401,
                //    ParentCategoryID = 4,
                //    ProductSubcategoryDescription = "Beaker",
                //    ImageURL = "/images/css/CategoryImages/beaker.png"
                //},
                //new ProductSubcategory
                //{
                //    ProductSubcategoryID = 402,
                //    ParentCategoryID = 4,
                //    ProductSubcategoryDescription = "Measuring",
                //    ImageURL = "/images/css/CategoryImages/measuring.png"
                //},
                //new ProductSubcategory
                //{
                //    ProductSubcategoryID = 403,
                //    ParentCategoryID = 4,
                //    ProductSubcategoryDescription = "Tube Holders",
                //    ImageURL = "/images/css/CategoryImages/tube_holder.png"
                //},
                //new ProductSubcategory
                //{
                //    ProductSubcategoryID = 404,
                //    ParentCategoryID = 4,
                //    ProductSubcategoryDescription = "Buckets",
                //    ImageURL = "/images/css/CategoryImages/bucket.png"
                //},
                //new ProductSubcategory
                //{
                //    ProductSubcategoryID = 405,
                //    ParentCategoryID = 4,
                //    ProductSubcategoryDescription = "Cooling Racks"
                //},
                //new ProductSubcategory
                //{
                //    ProductSubcategoryID = 406,
                //    ParentCategoryID = 4,
                //    ProductSubcategoryDescription = "-196 Box",
                //    ImageURL = "/images/css/CategoryImages/196box.png"
                //},
                //new ProductSubcategory
                //{
                //    ProductSubcategoryID = 407,
                //    ParentCategoryID = 4,
                //    ProductSubcategoryDescription = "-80 Box",
                //    ImageURL = "/images/css/CategoryImages/80box.png"
                //},
                //new ProductSubcategory
                //{
                //    ProductSubcategoryID = 501,
                //    ParentCategoryID = 5,
                //    ProductSubcategoryDescription = "Instrument"
                //},
                //new ProductSubcategory
                //{
                //    ProductSubcategoryID = 502,
                //    ParentCategoryID = 5,
                //    ProductSubcategoryDescription = "Instrument Parts"
                //},
                //new ProductSubcategory
                //{
                //    ProductSubcategoryID = 503,
                //    ParentCategoryID = 5,
                //    ProductSubcategoryDescription = "Instrument Check"
                //},
                //new ProductSubcategory
                //{
                //    ProductSubcategoryID = 504,
                //    ParentCategoryID = 5,
                //    ProductSubcategoryDescription = "Instrument Fixing"
                //},
                //new ProductSubcategory
                //{
                //    ProductSubcategoryID = 505,
                //    ParentCategoryID = 5,
                //    ProductSubcategoryDescription = "Instrument Calibration"
                //},
                //new ProductSubcategory
                //{
                //    ProductSubcategoryID = 506,
                //    ParentCategoryID = 5,
                //    ProductSubcategoryDescription = "Instrument Warranty"
                //},
                //new ProductSubcategory
                //{
                //    ProductSubcategoryID = 507,
                //    ParentCategoryID = 5,
                //    ProductSubcategoryDescription = "Lab Software"
                //},
                //new ProductSubcategory
                //{
                //    ProductSubcategoryID = 508,
                //    ParentCategoryID = 5,
                //    ProductSubcategoryDescription = "Lab Furniture"
                //},
                new ProductSubcategory
                {
                    ProductSubcategoryID = 801,
                    ParentCategoryID = 8,
                    ProductSubcategoryDescription = "Rent",
                    ImageURL = "/images/css/CategoryImages/rent.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 602,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Communication",
                    ImageURL = "/images/css/CategoryImages/communications.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 808,
                    ParentCategoryID = 8,
                    ProductSubcategoryDescription = "Branding",
                    ImageURL = "/images/css/CategoryImages/branding.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 809,
                    ParentCategoryID = 8,
                    ProductSubcategoryDescription = "Shipment",
                    ImageURL = "/images/css/CategoryImages/shippment.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 804,
                    ParentCategoryID = 8,
                    ProductSubcategoryDescription = "Renovation",
                    ImageURL = "/images/css/CategoryImages/renovation.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 812,
                    ParentCategoryID = 8,
                    ProductSubcategoryDescription = "Bookkeeping",
                    ImageURL = "/images/css/CategoryImages/bookeeping.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 1001,
                    ParentCategoryID = 10,
                    ProductSubcategoryDescription = "Law",
                    ImageURL = "/images/css/CategoryImages/legal.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 1201,
                    ParentCategoryID = 12,
                    ProductSubcategoryDescription = "Tax",
                    ImageURL = "/images/css/CategoryImages/taxes.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 811,
                    ParentCategoryID = 8,
                    ProductSubcategoryDescription = "Books And Journal",
                    ImageURL = "/images/css/CategoryImages/books.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 1004,
                    ParentCategoryID = 10,
                    ProductSubcategoryDescription = "Clinical Experiments",
                    ImageURL = "/images/css/CategoryImages/clinical_regulation.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 901,
                    ParentCategoryID = 9,
                    ProductSubcategoryDescription = "Conference",
                    ImageURL = "/images/css/CategoryImages/conference.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 807,
                    ParentCategoryID = 8,
                    ProductSubcategoryDescription = "Company Events",
                    ImageURL = "/images/css/CategoryImages/company_events.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 805,
                    ParentCategoryID = 8,
                    ProductSubcategoryDescription = "Insurance",
                    ImageURL = "/images/css/CategoryImages/insurance.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 1301,
                    ParentCategoryID = 13,
                    ProductSubcategoryDescription = "General",
                    ImageURL = "/images/css/CategoryImages/general.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 1102,
                    ParentCategoryID = 11,
                    ProductSubcategoryDescription = "General",
                    ImageURL = "/images/css/CategoryImages/general.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 604,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "General",
                    ImageURL = "/images/css/CategoryImages/general.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 814,
                    ParentCategoryID = 8,
                    ProductSubcategoryDescription = "General",
                    ImageURL = "/images/css/CategoryImages/general.png"
                 },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 1203,
                    ParentCategoryID = 12,
                    ProductSubcategoryDescription = "General",
                    ImageURL = "/images/css/CategoryImages/general.png"
                },
                new ProductSubcategory
                 {
                     ProductSubcategoryID = 1005,
                     ParentCategoryID = 10,
                     ProductSubcategoryDescription = "General",
                     ImageURL = "/images/css/CategoryImages/general.png"
                 },
                new ProductSubcategory
                 {
                     ProductSubcategoryID = 905,
                     ParentCategoryID = 9,
                     ProductSubcategoryDescription = "General",
                     ImageURL = "/images/css/CategoryImages/general.png"
                 },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 601,
                    ParentCategoryID = 6,
                    ProductSubcategoryDescription = "Hardware",
                    ImageURL = "/images/css/CategoryImages/software.png"
                },
                new ProductSubcategory
                 {
                     ProductSubcategoryID = 603,
                     ParentCategoryID = 6,
                     ProductSubcategoryDescription = "Cybersecurity",
                     ImageURL = "/images/css/CategoryImages/software.png"
                 },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 903,
                    ParentCategoryID = 9,
                    ProductSubcategoryDescription = "Hotels",
                    ImageURL = "/images/css/CategoryImages/hotels.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 902,
                    ParentCategoryID = 9,
                    ProductSubcategoryDescription = "Flight Tickets",
                    ImageURL = "/images/css/CategoryImages/flight_tickets.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 1002,
                    ParentCategoryID = 10,
                    ProductSubcategoryDescription = "Scientific",
                    ImageURL = "/images/css/CategoryImages/sciemtific_advice.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 1003,
                    ParentCategoryID = 10,
                    ProductSubcategoryDescription = "Business",
                    ImageURL = "/images/css/CategoryImages/business_advice.png"
                },                
                new ProductSubcategory
                {
                    ProductSubcategoryID = 1101,
                    ParentCategoryID = 11,
                    ProductSubcategoryDescription = "Safety",
                    ImageURL = "/images/css/CategoryImages/safety.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 904,
                    ParentCategoryID = 9,
                    ProductSubcategoryDescription = "Food",
                    ImageURL = "/images/css/CategoryImages/food.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 810,
                    ParentCategoryID = 8,
                    ProductSubcategoryDescription = "Food",
                    ImageURL = "/images/css/CategoryImages/food.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 806,
                    ParentCategoryID = 8,
                    ProductSubcategoryDescription = "Furniture",
                    ImageURL = "/images/css/CategoryImages/furniture.png"
                },
                 new ProductSubcategory
                 {
                     ProductSubcategoryID = 802,
                     ParentCategoryID = 8,
                     ProductSubcategoryDescription = "Electricity",
                 },
                  new ProductSubcategory
                  {
                      ProductSubcategoryID = 803,
                      ParentCategoryID = 8,
                      ProductSubcategoryDescription = "Parking",
                  },
                   new ProductSubcategory
                   {
                       ProductSubcategoryID = 813,
                       ParentCategoryID = 8,
                       ProductSubcategoryDescription = "Graphics",
                       ImageURL = "/images/css/CategoryImages/furniture.png"
                   },
                    new ProductSubcategory
                    {
                        ProductSubcategoryID = 1202,
                        ParentCategoryID = 12,
                        ProductSubcategoryDescription = "Fees",
                        ImageURL = "/images/css/CategoryImages/furniture.png"
                    },
                new ProductSubcategory
                 {
                     ProductSubcategoryID = 701,
                     ParentCategoryID = 7,
                     ProductSubcategoryDescription = "Virus",
                     ImageURL = "/images/css/CategoryImages/virus.png"
                 },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 702,
                    ParentCategoryID = 7,
                    ProductSubcategoryDescription = "Plasmid",
                    ImageURL = "/images/css/CategoryImages/plasmid.png"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 703,
                    ParentCategoryID = 7,
                    ProductSubcategoryDescription = "Probes"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 704,
                    ParentCategoryID = 7,
                    ProductSubcategoryDescription = "Cells"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 705,
                    ParentCategoryID = 7,
                    ProductSubcategoryDescription = "Bacteria with Plasmids"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 706,
                    ParentCategoryID = 7,
                    ProductSubcategoryDescription = "Blood"
                },
                new ProductSubcategory
                {
                    ProductSubcategoryID = 707,
                    ParentCategoryID = 7,
                    ProductSubcategoryDescription = "Serum"
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
              // new RequestStatus
              // {
              //     RequestStatusID = 4,
              //     RequestStatusDescription = "Partial" // request page, under order
              // },
              //  new RequestStatus
              //  {
              //      RequestStatusID = 5,
              //      RequestStatusDescription = "Clarify" // request page, under order
              //  },
                new RequestStatus
                {
                    RequestStatusID = 6,
                    RequestStatusDescription = "Approved" // request page, under order
                },
                new RequestStatus
                {
                    RequestStatusID = 7,
                    RequestStatusDescription = "Saved To Inventory" 
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
                 UnitTypeID = 19,
                 UnitParentTypeID = 1,
                 UnitTypeDescription = "Case"
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
                 UnitTypeID = 24,
                 UnitParentTypeID = 1,
                 UnitTypeDescription = "Tube"
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
                UnitTypeID = 20,
                UnitParentTypeID = 2,
                UnitTypeDescription = "pmol"
            },
            new UnitType
            {
                UnitTypeID = 21,
                UnitParentTypeID = 2,
                UnitTypeDescription = "nmol"
            },
            new UnitType
            {
                UnitTypeID = 22,
                UnitParentTypeID = 2,
                UnitTypeDescription = "umol"
            },
            new UnitType
            {
                UnitTypeID = 23,
                UnitParentTypeID = 2,
                UnitTypeDescription = "mol"
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
                    SubProjectDescription = "General"
                },
                new SubProject
                {
                    SubProjectID = 102,
                    ProjectID = 1,
                    SubProjectDescription = "Epigenetic Rejuvenation"
                },
                new SubProject
                {
                    SubProjectID = 103,
                    ProjectID = 1,
                    SubProjectDescription = "Plasma Rejuvenation"
                },
                new SubProject
                {
                    SubProjectID = 201,
                    ProjectID = 2,
                    SubProjectDescription = "General"
                },
                new SubProject
                {
                    SubProjectID = 202,
                    ProjectID = 2,
                    SubProjectDescription = "AAV"
                },
                new SubProject
                {
                    SubProjectID = 301,
                    ProjectID = 3,
                    SubProjectDescription = "General"
                },
                new SubProject
                {
                    SubProjectID = 302,
                    ProjectID = 3,
                    SubProjectDescription = "Epigenetic Clock"
                },
                new SubProject
                {
                    SubProjectID = 303,
                    ProjectID = 3,
                    SubProjectDescription = "Telomere Measurement"
                },
                new SubProject
                {
                    SubProjectID = 401,
                    ProjectID = 4,
                    SubProjectDescription = "General"
                },
                new SubProject
                {
                    SubProjectID = 402,
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
            modelBuilder.Entity<RequestNotificationStatus>().HasData(
                new RequestNotificationStatus
                {
                    NotificationStatusID = 1,
                    Icon = "icon-priority_high-24px",
                    Color = "--notifications-orderlate-color",
                    Description = "OrderLate"
                },
                new RequestNotificationStatus
                {
                    NotificationStatusID = 2,
                    Icon = "icon-centarix-icons-03",
                    Color = "--notifications-ordered-color",
                    Description = "ItemOrdered"
                },
                new RequestNotificationStatus
                {
                    NotificationStatusID = 3,
                    Icon = "icon-done-24px",
                    Color = "--notifications-approved-color",
                    Description = "ItemApproved"
                },
                new RequestNotificationStatus
                {
                    NotificationStatusID = 4,
                    Icon = "icon-local_mall-24px",
                    Color = "--notifications-received-color",
                    Description = "ItemReceived"
                }
          );
            modelBuilder.Entity<TimekeeperNotificationStatus>().HasData(
                new RequestNotificationStatus
                {
                    NotificationStatusID = 5,
                    Icon = "icon-notification_timekeeper-24px",
                    Color = "--timekeeper-color",
                    Description = "UpdateHours"
                }
          );

            modelBuilder.Entity<LocationType>().HasData(
                new LocationType
                {
                    LocationTypeID = 100,
                    LocationTypeName = "-196°C",
                    LocationTypePluralName = "-196°C",
                    LocationTypeChildID = 101,
                    Depth = 0
                },
                new LocationType
                {
                    LocationTypeID = 101,
                    LocationTypeName = "Rack",
                    LocationTypePluralName = "Racks",
                    LocationTypeNameAbbre = "R",
                    LocationTypeParentID = 100,
                    LocationTypeChildID = 102,
                    Depth = 1
                },
                new LocationType
                {
                    LocationTypeID = 102,
                    LocationTypeName = "Box",
                    LocationTypePluralName = "Boxes",
                    LocationTypeNameAbbre = "B",
                    LocationTypeParentID = 101,
                    LocationTypeChildID = 103,
                    Depth = 2
                },
                new LocationType
                {
                    LocationTypeID = 103,
                    LocationTypeName = "Box Unit",
                    LocationTypeNameAbbre = "B",
                    LocationTypePluralName = "Box Units",
                    LocationTypeParentID = 102,
                    Limit = 1,
                    Depth = 3
                },
                new LocationType
                {
                    LocationTypeID = 200,
                    LocationTypeName = "-80°C",
                    LocationTypePluralName = "-80°C",
                    LocationTypeChildID = 201,
                    Depth = 0
                },
                new LocationType
                {
                    LocationTypeID = 201,
                    LocationTypeName = "Floor",
                    LocationTypePluralName = "Floors",
                    LocationTypeNameAbbre = "F",
                    LocationTypeParentID = 200,
                    LocationTypeChildID = 202,
                    Depth = 1
                },
                new LocationType
                {
                    LocationTypeID = 202,
                    LocationTypeName = "Rack",
                    LocationTypePluralName = "Racks",
                    LocationTypeNameAbbre = "R",
                    LocationTypeParentID = 201,
                    LocationTypeChildID = 203,
                    Depth = 2
                },
                new LocationType
                {
                    LocationTypeID = 203,
                    LocationTypeName = "Shelf",
                    LocationTypePluralName = "Shelves",
                    LocationTypeNameAbbre = "S",
                    LocationTypeParentID = 202,
                    LocationTypeChildID = 204,
                    Depth = 3
                },
                new LocationType
                {
                    LocationTypeID = 204,
                    LocationTypeName = "Box",
                    LocationTypePluralName = "Boxes",
                    LocationTypeNameAbbre = "B",
                    LocationTypeParentID = 203,
                    LocationTypeChildID = 205,
                    Depth = 4
                },
                   new LocationType
                   {
                       LocationTypeID = 205,
                       LocationTypeName = "Box Unit",
                       LocationTypePluralName = "Box Units",
                       LocationTypeNameAbbre = "B",
                       LocationTypeParentID = 204,
                       Limit = 1,
                       Depth = 5
                   },
                new LocationType
                {
                    LocationTypeID = 300,
                    LocationTypeName = "-20°C",
                    LocationTypePluralName = "-20°C",
                    LocationTypeChildID = 301,
                    Depth = 0
                },
                new LocationType
                {
                    LocationTypeID = 301,
                    LocationTypeName = "Shelf",
                    LocationTypePluralName = "Shelves",
                    LocationTypeNameAbbre = "S",
                    LocationTypeParentID = 300,
                    Depth = 1
                },
                new LocationType
                {
                    LocationTypeID = 400,
                    LocationTypeName = "4°C",
                    LocationTypePluralName = "4°C",
                    LocationTypeChildID = 401,
                    Depth = 0
                },
                new LocationType
                {
                    LocationTypeID = 401,
                    LocationTypeName = "Shelf",
                    LocationTypePluralName = "Shelves",
                    LocationTypeNameAbbre = "S",
                    LocationTypeParentID = 400,
                    Depth = 1
                },
                new LocationType
                {
                    LocationTypeID = 500,
                    LocationTypeName = "25°C",
                    LocationTypePluralName = "25°C",
                    LocationTypeChildID = 501,
                    Depth = 0
                },
                new LocationType
                {
                    LocationTypeID = 501,
                    LocationTypeName = "Location",
                    LocationTypePluralName = "Locations",
                    LocationTypeParentID = 500,
                    LocationTypeChildID = 502,
                    Depth = 1
                },
                 new LocationType
                 {
                     LocationTypeID = 502,
                     LocationTypeName = "Lab Part",
                     LocationTypePluralName = "Lab Parts",
                     LocationTypeParentID = 501,
                     LocationTypeChildID = 503,
                     Depth = 2
                 },
                new LocationType
                {
                    LocationTypeID = 503,
                    LocationTypeName = "Section",
                    LocationTypePluralName = "Sections",
                    LocationTypeNameAbbre ="S",
                    LocationTypeParentID = 502,
                    Depth = 3
                }
            );
            modelBuilder.Entity<LocationRoomType>().HasData(
                    new LocationRoomType
                    {
                        LocationRoomTypeID = 1,
                        LocationRoomTypeDescription = "Laboratory",
                        LocationAbbreviation = "L"
                    },
                    new LocationRoomType
                    {
                        LocationRoomTypeID = 2,
                        LocationRoomTypeDescription = "Tissue Culture",
                        LocationAbbreviation = "TC"
                    },
                    new LocationRoomType
                    {
                        LocationRoomTypeID = 3,
                        LocationRoomTypeDescription = "Equipment Room",
                        LocationAbbreviation = "E"
                    },
                    new LocationRoomType
                    {
                        LocationRoomTypeID = 4,
                        LocationRoomTypeDescription = "Refrigerator Room",
                        LocationAbbreviation = "R"
                    },
                    new LocationRoomType
                    {
                        LocationRoomTypeID = 5,
                        LocationRoomTypeDescription = "Washing Room",
                        LocationAbbreviation = "W"
                    },
                    new LocationRoomType
                    {
                        LocationRoomTypeID = 6,
                        LocationRoomTypeDescription = "Storage Room",
                        LocationAbbreviation = "S"
                    }
                );
            modelBuilder.Entity<LocationInstance>().HasData(
                       new LocationInstance
                       {
                           LocationInstanceID = 1,
                           LocationInstanceName = "25°C",
                           Height = 7,
                           Width =1,
                           LocationTypeID = 500

                       },
                       new LocationInstance
                       {
                           LocationInstanceID = 2,
                           LocationInstanceParentID =1,
                           LocationRoomTypeID = 1,
                            LocationInstanceName = "Laboratory 1",
                           LocationInstanceAbbrev = "L1",
                           LocationTypeID = 501,
                            Width = 1,
                       },
                       new LocationInstance
                       {
                           LocationInstanceID = 3,
                           LocationInstanceParentID = 1,
                           LocationRoomTypeID = 1,
                           LocationInstanceName = "Laboratory 2",
                           LocationInstanceAbbrev ="L2",
                           LocationTypeID = 501,
                            Width = 1,
                       },
                       new LocationInstance
                       {
                           LocationInstanceID = 4,
                           LocationInstanceParentID = 1,
                           LocationInstanceName = "Tissue Culture 1",
                           LocationInstanceAbbrev = "TC1",
                           LocationTypeID = 501,
                           LocationRoomTypeID = 2,
                           Width = 1,
                       },
                        new LocationInstance
                        {
                            LocationInstanceID = 5,
                            LocationInstanceParentID = 1,
                            LocationTypeID = 501,
                            LocationRoomTypeID = 3,
                            LocationInstanceName = "Equipment Room 1",
                            LocationInstanceAbbrev = "E1",
                             Width = 1,

                        },
                        new LocationInstance
                        {
                            LocationInstanceID = 6,
                            LocationInstanceParentID = 1,
                            LocationTypeID = 501,
                            LocationRoomTypeID = 4,
                            LocationInstanceName = "Refrigerator Room 1",
                            LocationInstanceAbbrev = "R1",
                             Width = 1,

                        },
                        new LocationInstance
                        {
                            LocationInstanceID = 7,
                            LocationInstanceParentID = 1,
                            LocationTypeID = 501,
                            LocationRoomTypeID = 5,
                            LocationInstanceName = "Washing Room 1",
                            LocationInstanceAbbrev = "W1",
                             Width = 1,

                        },
                        new LocationInstance
                        {
                            LocationInstanceID= 8,
                            LocationInstanceParentID = 1,
                            LocationTypeID = 501,
                            LocationRoomTypeID = 6,
                            LocationInstanceName = "Storage Room 1",
                            LocationInstanceAbbrev = "S1",
                            Width = 1,

                        }
                 );
            modelBuilder.Entity<LabPart>().HasData(
                    new LabPart
                    {
                        LabPartID = 1,
                        LabPartName = "Closet" ,
                        LabPartNameAbbrev ="C",
                        HasShelves = true
                    },
                    new LabPart
                    {
                        LabPartID = 2,
                        LabPartName = "Glass Closet",
                        LabPartNameAbbrev = "G",
                        HasShelves = true
                    },
                    new LabPart
                    {
                        LabPartID = 3,
                        LabPartNameAbbrev = "T",
                        LabPartName = "Table",
                    },
                    new LabPart
                    {
                        LabPartID = 4,
                        LabPartNameAbbrev = "D",
                        LabPartName = "Drawer",
                    },
                    new LabPart
                    {
                        LabPartID = 5,
                        LabPartNameAbbrev = "S",
                        LabPartName = "Shelf"
                    },
                    new LabPart
                    {
                        LabPartID = 6,
                        LabPartNameAbbrev = "B",
                        LabPartName = "Bench"
                    }
               );
            modelBuilder.Entity<OffDayType>().HasData(
                        new OffDayType
                        {
                            OffDayTypeID = 1,
                            Description = "Sick Day"
                        },
                         new OffDayType
                         {
                             OffDayTypeID = 2,
                             Description = "Vacation Day"
                         },
                         new OffDayType
                         {
                             OffDayTypeID = 3,
                             Description = "Maternity Leave"
                         }
                         ,
                         new OffDayType
                         {
                             OffDayTypeID = 4,
                             Description = "Special Day"
                         }
                  );
            modelBuilder.Entity<EmployeeHoursStatus>().HasData(
                new EmployeeHoursStatus
                {
                    EmployeeHoursStatusID = 1,
                    Description = "Work from home"
                },
                new EmployeeHoursStatus
                {
                    EmployeeHoursStatusID = 2,
                    Description = "Edit existing hours"
                },
                new EmployeeHoursStatus
                {
                    EmployeeHoursStatusID = 3,
                    Description = "Forgot to report"
                }
            );
            modelBuilder.Entity<EmployeeStatus>().HasData(
           new EmployeeStatus
           {
               EmployeeStatusID = 1,
               Description = "Employee",
               Abbreviation = "E",
               LastCentarixID = 0
           },
           new EmployeeStatus
           {
               EmployeeStatusID = 2,
               Description = "Freelancer",
               Abbreviation = "F",
               LastCentarixID = 0
           },
           new EmployeeStatus
           {
               EmployeeStatusID = 3,
               Description = "Advisor",
               Abbreviation = "A",
               LastCentarixID = 0
           },
           new EmployeeStatus
           {
               EmployeeStatusID = 4,
               Description = "User",
               Abbreviation = "U",
               LastCentarixID = 0
           }
       );
            modelBuilder.Entity<JobCategoryType>().HasData(
                  new JobCategoryType
                  {
                      JobCategoryTypeID = 1,
                      Description = "Executive"
                  },
                  new JobCategoryType
                  {
                      JobCategoryTypeID = 2,
                      Description = "Rejuvenation"
                  },
                  new JobCategoryType
                  {
                      JobCategoryTypeID = 3,
                      Description = "Biomarker"
                  },
                  new JobCategoryType
                  {
                      JobCategoryTypeID = 4,
                      Description = "Delivery Systems"
                  },
                  new JobCategoryType
                  {
                      JobCategoryTypeID = 5,
                      Description = "Clinical Trials"
                  },
                  new JobCategoryType
                  {
                      JobCategoryTypeID = 6,
                      Description = "Business Development"
                  },
                  new JobCategoryType
                  {
                      JobCategoryTypeID = 7,
                      Description = "Software Development"
                  },
                  new JobCategoryType
                  {
                      JobCategoryTypeID = 8,
                      Description = "General"
                  },
                  new JobCategoryType
                  {
                      JobCategoryTypeID = 9,
                      Description = "Lab"
                  },
                  new JobCategoryType
                  {
                      JobCategoryTypeID = 10,
                      Description = "Bioinformatics"
                  }
              );
            modelBuilder.Entity<JobSubcategoryType>().HasData(
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 201,
                    JobCategoryTypeID = 2,
                    Description = "Senior Scientist"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 202,
                    JobCategoryTypeID = 2,
                    Description = "Research Associate"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 203,
                    JobCategoryTypeID = 2,
                    Description = "Lab Technician"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 204,
                    JobCategoryTypeID = 2,
                    Description = "Team Manager"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 205,
                    JobCategoryTypeID = 2,
                    Description = "Production Worker"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 206,
                    JobCategoryTypeID = 2,
                    Description = "Operation Executive"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 207,
                    JobCategoryTypeID = 2,
                    Description = "Business Development"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 208,
                    JobCategoryTypeID = 2,
                    Description = "Sales"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 209,
                    JobCategoryTypeID = 2,
                    Description = "Lab Manager"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 210,
                    JobCategoryTypeID = 2,
                    Description = "Bioinformatician"
                },
                                new JobSubcategoryType
                                {
                                    JobSubcategoryTypeID = 301,
                                    JobCategoryTypeID = 3,
                                    Description = "Senior Scientist"
                                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 302,
                    JobCategoryTypeID = 3,
                    Description = "Research Associate"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 303,
                    JobCategoryTypeID = 3,
                    Description = "Lab Technician"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 304,
                    JobCategoryTypeID = 3,
                    Description = "Team Manager"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 305,
                    JobCategoryTypeID = 3,
                    Description = "Production Worker"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 306,
                    JobCategoryTypeID = 3,
                    Description = "Operation Executive"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 307,
                    JobCategoryTypeID = 3,
                    Description = "Business Development"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 308,
                    JobCategoryTypeID = 3,
                    Description = "Sales"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 309,
                    JobCategoryTypeID = 3,
                    Description = "Lab Manager"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 310,
                    JobCategoryTypeID = 3,
                    Description = "Bioinformatician"
                },
                                new JobSubcategoryType
                                {
                                    JobSubcategoryTypeID = 401,
                                    JobCategoryTypeID = 4,
                                    Description = "Senior Scientist"
                                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 402,
                    JobCategoryTypeID = 4,
                    Description = "Research Associate"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 403,
                    JobCategoryTypeID = 4,
                    Description = "Lab Technician"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 404,
                    JobCategoryTypeID = 4,
                    Description = "Team Manager"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 405,
                    JobCategoryTypeID = 4,
                    Description = "Production Worker"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 406,
                    JobCategoryTypeID = 4,
                    Description = "Operation Executive"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 407,
                    JobCategoryTypeID = 4,
                    Description = "Business Development"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 408,
                    JobCategoryTypeID = 4,
                    Description = "Sales"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 409,
                    JobCategoryTypeID = 4,
                    Description = "Lab Manager"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 410,
                    JobCategoryTypeID = 4,
                    Description = "Bioinformatician"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 101,
                    JobCategoryTypeID = 1,
                    Description = "CEO"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 102,
                    JobCategoryTypeID = 1,
                    Description = "CTO"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 103,
                    JobCategoryTypeID = 1,
                    Description = "COO"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 104,
                    JobCategoryTypeID = 1,
                    Description = "President"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 105,
                    JobCategoryTypeID = 1,
                    Description = "Director"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 106,
                    JobCategoryTypeID = 1,
                    Description = "CSO"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 107,
                    JobCategoryTypeID = 1,
                    Description = "CMO"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 108,
                    JobCategoryTypeID = 1,
                    Description = "CFO"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 109,
                    JobCategoryTypeID = 1,
                    Description = "CBO"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 601,
                    JobCategoryTypeID = 6,
                    Description = "Sales"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 701,
                    JobCategoryTypeID = 7,
                    Description = "Elixir"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 702,
                    JobCategoryTypeID = 7,
                    Description = "Automation Developer"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 703,
                    JobCategoryTypeID = 7,
                    Description = "Other"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 801,
                    JobCategoryTypeID = 8,
                    Description = "Cooking"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 802,
                    JobCategoryTypeID = 8,
                    Description = "Cleaning"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 803,
                    JobCategoryTypeID = 8,
                    Description = "IT"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 804,
                    JobCategoryTypeID = 8,
                    Description = "Administration"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 805,
                    JobCategoryTypeID = 8,
                    Description = "Operations Manager"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 806,
                    JobCategoryTypeID = 8,
                    Description = "Branch Manager"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 901,
                    JobCategoryTypeID = 9,
                    Description = "Lab Manager"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 902,
                    JobCategoryTypeID = 9,
                    Description = "Automations Manager"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 903,
                    JobCategoryTypeID = 9,
                    Description = "Automations Implementer"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 1001,
                    JobCategoryTypeID = 10,
                    Description = "Senior Bioinformatician"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 1002,
                    JobCategoryTypeID = 10,
                    Description = "Bioinformatician Executive"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 1003,
                    JobCategoryTypeID = 10,
                    Description = "Bioinformatician Team Manager"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 1004,
                    JobCategoryTypeID = 10,
                    Description = "Bioinformatics Technician"
                },
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 1005,
                    JobCategoryTypeID = 10,
                    Description = "Bioinformatics Researcher"
                },
              
                new JobSubcategoryType
                {
                    JobSubcategoryTypeID = 501,
                    JobCategoryTypeID = 5,
                    Description = "Clinical Trials"
                }
            );
            modelBuilder.Entity<Degree>().HasData(
                new Degree
                {
                    DegreeID = 1,
                    Description = "B.Sc"
                },
                  new Degree
                  {
                      DegreeID = 2,
                      Description = "M.Sc"
                  },
                new Degree
                {
                    DegreeID = 3,
                    Description = "P.hd"
                },
                new Degree
                {
                    DegreeID = 4,
                    Description = "Post P.hd"
                },
                new Degree
                {
                    DegreeID = 5,
                    Description = "No Degree"
                },
                new Degree
                {
                    DegreeID = 6,
                    Description = "Certificate"
                }
            );
            modelBuilder.Entity<Citizenship>().HasData(
                  new Citizenship
                  {
                      CitizenshipID = 1,
                      Description = "Israel"
                  },
                 new Citizenship
                 {
                     CitizenshipID = 2,
                     Description = "USA"
                 }
            );
            modelBuilder.Entity<MaritalStatus>().HasData(
                new MaritalStatus
                {
                    MaritalStatusID = 1,
                    Description = "Married"
                },
                new MaritalStatus
                {
                    MaritalStatusID = 2,
                    Description = "Single"
                },
                new MaritalStatus
                {
                    MaritalStatusID = 3,
                    Description = "Divorced"
                }
            );
            modelBuilder.Entity<CalibrationType>().HasData(
             new CalibrationType
             {
                 CalibrationTypeID = 1,
                 Description = "Repair",
                 Icon = "icon-build-24px"
             }
             ,
             new CalibrationType
             {
                 CalibrationTypeID = 2,
                 Description = "External Calibration",
                 Icon = "icon-miscellaneous_services-24px-1"
             },
             new CalibrationType
             {
                 CalibrationTypeID = 3,
                 Description = "In House Maintainance",
                 Icon = "icon-inhouse-maintainance-24px"
             }
         );

            modelBuilder.Entity<CompanyDayOffType>().HasData(
             new CompanyDayOffType
             {
                 CompanyDayOffTypeID = 1,
                 Name = "Purim 1"
             },
              new CompanyDayOffType
              {
                  CompanyDayOffTypeID = 2,
                  Name = "Purim 2"
              },
               new CompanyDayOffType
               {
                   CompanyDayOffTypeID = 3,
                   Name = "Erev Pesach"
               },
               new CompanyDayOffType
               {
                   CompanyDayOffTypeID = 4,
                   Name = "Pesach"
               },
               new CompanyDayOffType
               {
                   CompanyDayOffTypeID = 5,
                   Name = "Erev Shviei Pesach"
               },
               new CompanyDayOffType
               {
                   CompanyDayOffTypeID = 6,
                   Name = "Shviei Pesach"
               },
                new CompanyDayOffType
                {
                    CompanyDayOffTypeID = 7,
                    Name = "Yom Hazmaut"
                },
               new CompanyDayOffType
               {
                   CompanyDayOffTypeID = 8,
                   Name = "Erev Shavuous"
               },
               new CompanyDayOffType
               {
                   CompanyDayOffTypeID = 9,
                   Name = "Shavuous"
               },

                new CompanyDayOffType
                {
                    CompanyDayOffTypeID = 10,
                    Name = "Erev Rosh Hashana"
                },
               new CompanyDayOffType
               {
                   CompanyDayOffTypeID = 11,
                   Name = "Rosh Hashana 1"
               },
               new CompanyDayOffType
               {
                   CompanyDayOffTypeID = 12,
                   Name = "Rosh Hashana 2"
               },
               new CompanyDayOffType
               {
                   CompanyDayOffTypeID = 13,
                   Name = "Erev Yom Kippur"
               },
               new CompanyDayOffType
               {
                   CompanyDayOffTypeID = 14,
                   Name = "Yom Kippur"
               },
               new CompanyDayOffType
               {
                   CompanyDayOffTypeID = 15,
                   Name = "Erev Sukkot"
               },
               new CompanyDayOffType
               {
                   CompanyDayOffTypeID = 16,
                   Name = "Sukkot"
               },
               new CompanyDayOffType
               {
                   CompanyDayOffTypeID = 17,
                   Name = "Erev Simchat Torah"
               },
                new CompanyDayOffType
                {
                    CompanyDayOffTypeID = 18,
                    Name = "Simchat Torah"
                }
         );

        }
    }
}
