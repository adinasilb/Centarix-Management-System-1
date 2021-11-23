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
            modelBuilder.Entity<PaymentStatus>().HasData
                (
                SeededData.PaymentStatusData.Get()
            );

            modelBuilder.Entity<PaymentType>().HasData
                (
                SeededData.PaymentTypeData.Get()
           );

            modelBuilder.Entity<CompanyAccount>().HasData
                (
               SeededData.CompanyAccountData.Get()
            );

            modelBuilder.Entity<CreditCard>().HasData
                (
                SeededData.CreditCardData.Get()
           );
            modelBuilder.Entity<CategoryType>().HasData
                (
                SeededData.CategoryTypeData.Get()
             );
            //if any data is changed in parent categories - must edit parentRequestController monthly view models - had to hard code categories so if add any more have to adjust
            modelBuilder.Entity<ParentCategory>().HasData
            (
               SeededData.ParentCategoryData.Get()
            );

            modelBuilder.Entity<UnitTypeParentCategory>().HasData
              (
                 SeededData.UnitTypeParentCategoryData.Get()

              );
            modelBuilder.Entity<ProductSubcategory>().HasData
              (
                SeededData.ProductSubcategoryData.Get()
              ) ;

            modelBuilder.Entity<RequestStatus>().HasData
          (
              SeededData.RequestStatusData.Get()
             );
            modelBuilder.Entity<UnitParentType>().HasData
            (
               SeededData.UnitParentTypeData.Get()
            );

            modelBuilder.Entity<UnitType>().HasData
         (
            SeededData.UnitTypeData.Get()
            );

            modelBuilder.Entity<Project>().HasData
            (
                SeededData.ProjectData.Get()
            );

            modelBuilder.Entity<SubProject>().HasData
            (
              SeededData.SubProjectData.Get()
            );

            modelBuilder.Entity<QuoteStatus>().HasData(
              SeededData.QuoteStatusData.Get()
            );
            modelBuilder.Entity<RequestNotificationStatus>().HasData(
              SeededData.RequestNotificationStatusData.Get()
          );
            modelBuilder.Entity<TimekeeperNotificationStatus>().HasData(
              SeededData.TimeKeeperNotificationStatusData.Get()
          );

            modelBuilder.Entity<EmployeeInfoNotificationStatus>().HasData(
              SeededData.EmployeeInfoNotificationStatusData.Get()
          );

            modelBuilder.Entity<LocationType>().HasData(
              SeededData.LocationTypeData.Get()
            );
            modelBuilder.Entity<LocationRoomType>().HasData(
                SeededData.LocationRoomTypeData.Get()
            );
            modelBuilder.Entity<LocationRoomInstance>().HasData(
                     SeededData.LocationRoomInstanceData.Get()
                        );
            modelBuilder.Entity<TemporaryLocationInstance>().HasData(
                SeededData.TemporaryLocationInstanceData.Get()
             );
            modelBuilder.Entity<LabPart>().HasData(
                   SeededData.LabPartData.Get()
               );
            modelBuilder.Entity<OffDayType>().HasData(
                      SeededData.OffDayTypeData.Get()
                  );
            modelBuilder.Entity<EmployeeHoursStatus>().HasData(
               SeededData.EmployeeHoursStatusData.Get()
            );
            modelBuilder.Entity<EmployeeStatus>().HasData(
            SeededData.EmployeeStatusData.Get()
       );
            modelBuilder.Entity<JobCategoryType>().HasData(
                 SeededData.JobCategoryTypeData.Get()
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
                SeededData.DegreeData.Get()
            );
            modelBuilder.Entity<Citizenship>().HasData(
                SeededData.CitizenshipData.Get()
            );
            modelBuilder.Entity<MaritalStatus>().HasData(
                SeededData.MaritalStatusData.Get()
            );
            modelBuilder.Entity<CalibrationType>().HasData(
                SeededData.CalibrationTypeData.Get()
            );
            modelBuilder.Entity<CompanyDayOffType>().HasData(
                SeededData.CompanyDayOffTypeData.Get()
            );
            modelBuilder.Entity<ProtocolType>().HasData(
                SeededData.ProtocolTypeData.Get()
            );
            modelBuilder.Entity<ProtocolCategory>().HasData(
                SeededData.ProtocolCategoryData.Get()
            );
            modelBuilder.Entity<ProtocolSubCategory>().HasData(
                SeededData.ProtocolSubCategoryData.Get()
            );
            modelBuilder.Entity<ResourceCategory>().HasData(
                SeededData.ResourceCategoryData.Get()
            );
            modelBuilder.Entity<ReportType>().HasData(
                SeededData.ReportTypeData.Get()
            );
            modelBuilder.Entity<MaterialCategory>().HasData(
                SeededData.MaterialCategoryData.Get()
            );
            modelBuilder.Entity<LineType>().HasData(
                SeededData.LineTypeData.Get()
            );
            modelBuilder.Entity<FunctionType>().HasData(
                SeededData.FunctionTypeData.Get()
            );
            modelBuilder.Entity<Gender>().HasData(
                SeededData.GenderData.Get()
            );
            modelBuilder.Entity<ParticipantStatus>().HasData(
                SeededData.ParticipantStatusData.Get()
            );
            modelBuilder.Entity<Country>().HasData(
                SeededData.CountryData.Get()
            );
            modelBuilder.Entity<Currency>().HasData(
           SeededData.CurrencyData.Get()
           );
        }
    }
}
