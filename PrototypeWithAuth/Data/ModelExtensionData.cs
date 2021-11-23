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
                SeededData.JobSubcategoryTypeData.Get()
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
