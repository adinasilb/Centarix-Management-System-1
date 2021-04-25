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
       
        public DbSet<FavoriteRequest> FavoriteRequests { get; set; }
        public DbSet<ShareRequest> ShareRequests { get; set; }
        public DbSet<ProtocolInstanceResult> ProtocolInstanceResults { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorProtocol> AuthorProtocols { get; set; }
        public DbSet<ProtocolType> ProtocolTypes { get; set; }
        public DbSet<TagArticle> TagArticles { get; set; }
        public DbSet<TagProtocol> TagProtocols { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ResourceType> ResourceTypes { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<MaterialCategory> MaterialCategories { get; set; }
        public DbSet<ProtocolCategory> ProtocolCategories { get; set; }
        public DbSet<ProtocolSubCategory> ProtocolSubCategories { get; set; }
        public DbSet<LineType> LineTypes { get; set; }
        public DbSet<FunctionLine> FunctionLines { get; set; }
        public DbSet<MaterialProtocol> MaterialProtocols { get; set; }
        public DbSet<ProtocolComment> ProtocolComments { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<ProtocolInstance> ProtocolInstances { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<FunctionType> FunctionTypes { get; set; }
        public DbSet<Protocol> Protocols { get; set; }
        public DbSet<LocationRoomType> LocationRoomTypes { get; set; }
        public DbSet<LabPart> LabParts { get; set; }
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
        public DbSet<IpRange> IpRanges { get; set; }
        public DbSet<PhysicalAddress> PhysicalAddresses { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VendorCategoryType>()
                .HasKey(v => new { v.VendorID, v.CategoryTypeID });

            modelBuilder.Entity<UnitTypeParentCategory>()
                .HasKey(u => new { u.UnitTypeID, u.ParentCategoryID });

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

            modelBuilder.Entity<ShareRequest>()
                .HasOne(sr => sr.FromApplicationUser)
                .WithMany(au => au.ShareRequestsCreated)
                .HasForeignKey(sr => sr.FromApplicationUserID);

            modelBuilder.Entity<ShareRequest>()
                .HasOne(sr => sr.ToApplicationUser)
                .WithMany(au => au.ShareRequestsReceived)
                .HasForeignKey(sr => sr.ToApplicationUserID);


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

            modelBuilder.Entity<EmployeeHours>()
             .HasOne<OffDayType>(eh => eh.PartialOffDayType)
             .WithMany(odt => odt.EmployeeHoursPartial)
             .HasForeignKey(eh => eh.PartialOffDayTypeID);

            modelBuilder.Entity<EmployeeHours>()
              .HasOne<OffDayType>(eh => eh.OffDayType)
              .WithMany(odt => odt.EmployeeHours)
              .HasForeignKey(eh => eh.OffDayTypeID);

            modelBuilder.Entity<EmployeeHoursAwaitingApproval>()
              .HasOne<OffDayType>(eh => eh.PartialOffDayType)
              .WithMany(odt => odt.EmployeeHoursAwaitingApprovalsPartial)
              .HasForeignKey(eh => eh.PartialOffDayTypeID);

            modelBuilder.Entity<EmployeeHoursAwaitingApproval>()
              .HasOne<OffDayType>(eh => eh.OffDayType)
              .WithMany(odt => odt.EmployeeHoursAwaitingApprovals)
              .HasForeignKey(eh => eh.OffDayTypeID);

            //modelBuilder.Entity<Vendor>()
            //.HasOne<ParentCategory>(v => v.ParentCategory)
            //.WithMany(pc => pc.Vendors)
            //.HasForeignKey(v => v.ParentCategoryID)
            //.OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ParentQuote>()
                .HasQueryFilter(item => !item.IsDeleted);

            modelBuilder.Entity<Calibration>()
                .HasQueryFilter(item => !item.IsDeleted);

            modelBuilder.Entity<Payment>()
                .HasQueryFilter(item => !item.IsDeleted);

            modelBuilder.Entity<Comment>()
                .HasQueryFilter(item => !item.IsDeleted);

            modelBuilder.Entity<SalariedEmployee>().Ignore(e => e.WorkScope);
            modelBuilder.Entity<Employee>().Ignore(e => e.NetSalary);
            modelBuilder.Entity<Employee>().Ignore(e => e.TotalCost);
            modelBuilder.Entity<Employee>().Ignore(e => e.SickDays);
            modelBuilder.Entity<Employee>().Ignore(e => e.SickDaysPerMonth);
            modelBuilder.Entity<Employee>().Ignore(e => e.VacationDaysPerMonth);
            modelBuilder.Entity<Request>().Ignore(e => e.VAT);
            modelBuilder.Entity<Request>().Ignore(e => e.PricePerUnit);
            modelBuilder.Entity<Request>().Ignore(e => e.PricePerSubUnit);
            modelBuilder.Entity<Request>().Ignore(e => e.PricePerSubSubUnit);
            modelBuilder.Entity<Request>().Ignore(e => e.TotalWithVat);
            modelBuilder.Entity<Request>().Ignore(e => e.Ignore);
            modelBuilder.Entity<Request>().Ignore(e => e.IsReceived);
            modelBuilder.Entity<ParentQuote>().Ignore(e => e.QuoteDate_submit);
            modelBuilder.Entity<ParentRequest>().Ignore(e => e.OrderDate_submit);
            modelBuilder.Entity<Invoice>().Ignore(e => e.InvoiceDate_submit);
            modelBuilder.Entity<Payment>().Ignore(e => e.PaymentReferenceDate_submit);
            // modelBuilder.Entity<ParentRequest>().Ignore(e => e.InvoiceDate_submit);
            modelBuilder.Entity<Request>().Ignore(e => e.ArrivalDate_submit);
            modelBuilder.Entity<Request>().Ignore(e => e.BatchExpiration_submit);
            modelBuilder.Entity<EmployeeHours>().Ignore(e => e.Date_submit);
            modelBuilder.Entity<Employee>().Ignore(e => e.StartedWorking_submit);
            modelBuilder.Entity<Employee>().Ignore(e => e.DOB_submit);
            modelBuilder.Entity<ParentCategory>().Ignore(e => e.ParentCategoryDescriptionEnum);
            modelBuilder.Entity<EmployeeHoursAwaitingApproval>().Property(e => e.IsDenied).HasDefaultValue(false);
            modelBuilder.Entity<Request>().Property(r => r.IncludeVAT).HasDefaultValue(true);
            modelBuilder.Entity<ApplicationUser>().HasIndex(a => a.UserNum).IsUnique();
            modelBuilder.Entity<ExchangeRate>().Property(e => e.LatestExchangeRate).HasColumnType("decimal(18,3)");
            modelBuilder.Entity<Request>().Property(r => r.ExchangeRate).HasColumnType("decimal(18,3)");
            modelBuilder.Entity<Product>().Property(r => r.ProductCreationDate).HasDefaultValueSql("getdate()");

            /*PROTOCOLS*/
            ///set up composite keys

            modelBuilder.Entity<TagProtocol>()
              .HasKey(t => new { t.TagID, t.ProtocolID });

            modelBuilder.Entity<TagArticle>()
              .HasKey(t => new { t.ArticleID, t.TagID });

            modelBuilder.Entity<MaterialProtocol>()
              .HasKey(m => new { m.MaterialID, m.ProtocolID });

            modelBuilder.Entity<FunctionLine>()
              .HasKey(f => new { f.FunctionTypeID, f.LineID });

            modelBuilder.Entity<AuthorProtocol>()
                .HasKey(a => new { a.AuthorID, a.ProtocolID });

            //set up many to many relationshipw
            modelBuilder.Entity<LineType>()
                .HasOne(l=>l.LineTypeParent)
                .WithMany()
                .HasForeignKey(ltp => ltp.LineTypeParentID);

            modelBuilder.Entity<LineType>()
               .HasOne(lt => lt.LineTypeChild)
               .WithMany()
               .HasForeignKey(ltc => ltc.LineTypeChildID);


            modelBuilder.Entity<Report>().Property(r => r.ReportDescription).HasColumnType("ntext");
            modelBuilder.Entity<ProtocolInstanceResult>().Property(r => r.ResultDesciption).HasColumnType("ntext");

            modelBuilder.Seed();

            //foreach loop ensures that deletion is resticted - no cascade delete
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }
    }
}

