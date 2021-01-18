using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.Data;
using System.Linq;
using Abp.Domain.Entities;

namespace PrototypeWithAuth.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        internal readonly object AspNetUsers;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        //public DbSet<RequestLocationInstance> RequestLocationInstances { get; set; } // do we not need to include this set in the db context???
        public DbSet<PartialOffDayType> PartialOffDayTypes { get; set; }
        public DbSet<CentarixID> CentarixIDs { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<CompanyDayOff> CompanyDayOffs { get; set; }
        public DbSet<CompanyDayOffType> CompanyDayOffTypes { get; set; }
        public DbSet<CalibrationType> CalibrationTypes { get; set; }
        public DbSet<InternalCalibration> InternalCalibrations { get; set; }
        public DbSet<ExternalCalibration> ExternalCalibrations { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<Calibration> Calibrations { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<Citizenship> Citizenships { get; set; }
        public DbSet<MaritalStatus> MaritalStatuses { get; set; }
        public DbSet<JobSubcategoryType> JobSubcategoryTypes { get; set; }
        public DbSet<JobCategoryType> JobCategoryTypes { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Freelancer> Freelancers { get; set; }
        public DbSet<EmployeeStatus> EmployeeStatuses { get; set; }
        public DbSet<Advisor> Advisors { get; set; }
        public DbSet<SalariedEmployee> SalariedEmployees { get; set; }
        public DbSet<EmployeeHoursStatus> EmployeeHoursStatuses { get; set; }
        public DbSet<OffDayType> OffDayTypes { get; set; }
        public DbSet<EmployeeHoursAwaitingApproval> EmployeeHoursAwaitingApprovals { get; set; }
        public DbSet<EmployeeHours> EmployeeHours { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<TimekeeperNotificationStatus> TimekeeperNotificationStatuses { get; set; }
        public DbSet<RequestNotificationStatus> RequestNotificationStatuses { get; set; }
        public DbSet<NotificationStatus> NotificationStatuses { get; set; }
        public DbSet<RequestNotification> RequestNotifications { get; set; }
        public DbSet<TimekeeperNotification> TimekeeperNotifications { get; set; }
        //public DbSet<Notification<NotificationStatus>> Notifications { get; set; }
        public DbSet<PaymentStatus> PaymentStatuses { get; set; }
        public DbSet<Reorder> Reorder { get; set; }
        public DbSet<ParentQuote> ParentQuotes { get; set; }
        public DbSet<QuoteStatus> QuoteStatuses { get; set; }
        public DbSet<CategoryType> CategoryTypes { get; set; }
        public DbSet<VendorComment> VendorComments { get; set; }
        public DbSet<VendorContact> VendorContacts { get; set; }
        public DbSet<SubProject> SubProjects { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<LocationInstance> LocationInstances { get; set; }
        public DbSet<LocationType> LocationTypes { get; set; }
        public DbSet<CompanyAccount> CompanyAccounts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<RequestStatus> RequestStatuses { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<ParentRequest> ParentRequests { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSubcategory> ProductSubcategories { get; set; }
        public DbSet<ParentCategory> ParentCategories { get; set; }
        public DbSet<UnitType> UnitTypes { get; set; }
        public DbSet<UnitParentType> UnitParentTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VendorCategoryType>()
                .HasKey(v => new { v.VendorID, v.CategoryTypeID });
            modelBuilder.Entity<JobCategorySubCategory>()
            .HasKey(j => new { j.JobCategoryTypeID, j.JobSubcategoryTypeID });


            modelBuilder.Entity<RequestLocationInstance>()
                .HasQueryFilter(item => !item.IsDeleted)
                .HasKey(rl => new { rl.RequestID, rl.LocationInstanceID });

            modelBuilder.Entity<RequestLocationInstance>()
                .HasQueryFilter(item => !item.IsDeleted)
                .HasOne(rl => rl.Request)
                .WithMany(r => r.RequestLocationInstances)
                .HasForeignKey(rl => rl.RequestID);

            modelBuilder.Entity<RequestLocationInstance>()
                .HasQueryFilter(item => !item.IsDeleted)
                .HasOne(rl => rl.LocationInstance)
                .WithMany(l => l.RequestLocationInstances)
                .HasForeignKey(rl => rl.LocationInstanceID);

            modelBuilder.Entity<RequestLocationInstance>()
                .HasQueryFilter(item => !item.IsDeleted)
                .HasOne(rl => rl.ParentLocationInstance)
                .WithMany(l => l.AllRequestLocationInstances)
                .HasForeignKey(rl => rl.ParentLocationInstanceID);


            //set locationInstances to have self referncing fk
            modelBuilder.Entity<LocationInstance>()
                .HasOne(li => li.LocationInstanceParent)
                .WithMany()
                .HasForeignKey(lip => lip.LocationInstanceParentID);


            //set locationTypes to have self referncing fks
            modelBuilder.Entity<LocationType>()
                .HasOne(lt => lt.LocationTypeParent)
                .WithMany()
                .HasForeignKey(ltp => ltp.LocationTypeParentID);

            modelBuilder.Entity<LocationType>()
               .HasOne(lt => lt.LocationTypeChild)
               .WithMany()
               .HasForeignKey(ltc => ltc.LocationTypeChildID);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.ApplicationUserCreator)
                .WithMany(au => au.RequestsCreated)
                .HasForeignKey(r => r.ApplicationUserCreatorID);
            modelBuilder.Entity<Request>()
                .HasOne(r => r.ApplicationUserReceiver)
                .WithMany(au => au.RequestsReceived)
                .HasForeignKey(r => r.ApplicationUserReceiverID);



            // configures one-to-many relationship between Inventory and InventorySubcategories
            modelBuilder.Entity<ProductSubcategory>()
            .HasOne<ParentCategory>(ps => ps.ParentCategory)
            .WithMany(pc => pc.ProductSubcategories)
            .HasForeignKey(ps => ps.ParentCategoryID);

            modelBuilder.Entity<SubProject>()
            .HasOne<Project>(sp => sp.Project)
            .WithMany(p => p.SubProjects)
            .HasForeignKey(sp => sp.ProjectID);

            modelBuilder.Entity<UnitType>()
           .HasOne<UnitParentType>(ut => ut.UnitParentType)
           .WithMany(upt => upt.UnitTypes)
           .HasForeignKey(ut => ut.UnitParentTypeID);

            modelBuilder.Entity<ParentRequest>()
            .HasQueryFilter(item => !item.IsDeleted)
            .HasOne<ApplicationUser>(pr => pr.ApplicationUser)
            .WithMany(au => au.ParentRequests)
            .HasForeignKey(r => r.ApplicationUserID);

            //modelBuilder.Entity<Request>()
            //.HasOne<Product>(r => r.Product)
            //.WithMany(p => p.Requests)
            //.HasForeignKey(r => r.ProductID)
            //.OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Request>()
            .HasQueryFilter(item => !item.IsDeleted)
            .HasOne<RequestStatus>(r => r.RequestStatus)
            .WithMany(rs => rs.Requests)
            .HasForeignKey(r => r.RequestStatusID);


            modelBuilder.Entity<EmployeeHours>()
             .HasOne<EmployeeHoursStatus>(eh => eh.EmployeeHoursStatusEntry2)
             .WithMany(ehs => ehs.EmployeeHours)
             .HasForeignKey(eh => eh.EmployeeHoursStatusEntry2ID);

            modelBuilder.Entity<EmployeeHoursAwaitingApproval>()
              .HasOne<EmployeeHoursStatus>(eh => eh.EmployeeHoursStatusEntry2)
              .WithMany(ehs => ehs.EmployeeHoursAwaitingApprovals)
              .HasForeignKey(eh => eh.EmployeeHoursStatusEntry2ID);



            //modelBuilder.Entity<Vendor>()
            //.HasOne<ParentCategory>(v => v.ParentCategory)
            //.WithMany(pc => pc.Vendors)
            //.HasForeignKey(v => v.ParentCategoryID)
            //.OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ParentQuote>()
           .HasQueryFilter(item => !item.IsDeleted);

            modelBuilder.Entity<Calibration>()
                .HasQueryFilter(item => !item.IsDeleted);

            modelBuilder.Entity<SalariedEmployee>().Ignore(e => e.WorkScope);
            modelBuilder.Entity<Employee>().Ignore(e => e.NetSalary);
            modelBuilder.Entity<Employee>().Ignore(e => e.TotalCost);
            modelBuilder.Entity<Employee>().Ignore(e => e.SickDays);
            modelBuilder.Entity<Employee>().Ignore(e => e.SickDaysPerMonth);
            modelBuilder.Entity<Employee>().Ignore(e => e.VacationDaysPerMonth);
            modelBuilder.Entity<Request>().Ignore(e => e.VAT);
            modelBuilder.Entity<Request>().Ignore(e => e.PricePerUnit);
            modelBuilder.Entity<Request>().Ignore(e => e.TotalWithVat);

            modelBuilder.Entity<EmployeeHoursAwaitingApproval>().Property(e => e.IsDenied).HasDefaultValue(false);

            modelBuilder.Entity<ApplicationUser>().HasIndex(a => a.UserNum).IsUnique();
            modelBuilder.Seed();

            //foreach loop ensures that deletion is resticted - no cascade delete
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }
    }
}

