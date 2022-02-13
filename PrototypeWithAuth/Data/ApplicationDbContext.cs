﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.Data;
using System.Linq;
using Abp.Domain.Entities;
using System.Threading.Tasks;
using System.Transactions;
using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics.CodeAnalysis;

namespace PrototypeWithAuth.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        internal readonly object AspNetUsers;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<RecurrenceEndStatus> RecurringOrderEndStatuses { get; set;}
        public DbSet<TimePeriod> TimePeriods { get; set; }
        public DbSet<RecurringOrder> RecurringOrders { get; set; }
        public DbSet<StandingOrder> StandingOrders { get; set; }
        public DbSet<SingleOrder> SingleOrders { get; set; }
        public DbSet<OrderMethod> OrderMethods { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<CommentType> CommentTypes { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<OldVendorCountry> OldVendorCountries { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<ExperimentEntry> ExperimentEntries { get; set; }
        public DbSet<TestValue> TestValues { get; set; }
        public DbSet<TestHeader> TestHeaders { get; set; }
        public DbSet<TestOuterGroup> TestOuterGroups { get; set; }
        public DbSet<TestGroup> TestGroups { get; set; }
        public DbSet<FavoriteReport> FavoriteReports { get; set; }
        public DbSet<TempResultsJson> TempResultsJsons { get; set; }
        public DbSet<TempReportJson> TempReportJsons { get; set; }
        public DbSet<ProtocolVersion> ProtocolVersions { get; set; }
        public DbSet<FunctionResult> FunctionResults { get; set; }
        public DbSet<TempLineID> TempLineIDs { get; set; }
        public DbSet<FunctionLineID> FunctionLineIDs { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestCategory> TestCategories { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<ParticipantStatus> ParticipantStatuses { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Timepoint> Timepoints { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Experiment> Experiments { get; set; }
        public DbSet<TempLinesJson> TempLinesJsons { get; set; }
        public DbSet<TempRequestJson> TempRequestJsons { get; set; }
        public DbSet<LineChange> LineChanges { get; set; }
        public DbSet<ShareProtocol> ShareProtocols { get; set; }
        public DbSet<ShareResource> ShareResources { get; set; }
        public DbSet<FavoriteResource> FavoriteResources { get; set; }
        public DbSet<FavoriteProtocol> FavoriteProtocols { get; set; }
        public DbSet<ResourceNote> ResourceNotes { get; set; }
        public DbSet<ResourceCategory> ResourceCategories { get; set; }
        public DbSet<FavoriteRequest> FavoriteRequests { get; set; }
        public DbSet<ShareRequest> ShareRequests { get; set; }
        public DbSet<RequestList> RequestLists { get; set; }
        public DbSet<ShareRequestList> ShareRequestLists { get; set; }
        public DbSet<RequestListRequest> RequestListRequests { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorProtocol> AuthorProtocols { get; set; }
        public DbSet<ProtocolType> ProtocolTypes { get; set; }
        //public DbSet<TagArticle> TagArticles { get; set; }
        public DbSet<TagProtocol> TagProtocols { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportType> ReportTypes { get; set; }
        public DbSet<FunctionReport> FunctionReports { get; set; }
        public DbSet<ResourceType> ResourceTypes { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<MaterialCategory> MaterialCategories { get; set; }
        public DbSet<ProtocolCategory> ProtocolCategories { get; set; }
        public DbSet<ProtocolSubCategory> ProtocolSubCategories { get; set; }
        public DbSet<LineType> LineTypes { get; set; }
        public DbSet<FunctionLine> FunctionLines { get; set; }
        public DbSet<ProtocolComment> ProtocolComments { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<ProtocolInstance> ProtocolInstances { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<FunctionType> FunctionTypes { get; set; }
        public DbSet<Protocol> Protocols { get; set; }
        public DbSet<LocationRoomType> LocationRoomTypes { get; set; }
        public DbSet<LabPart> LabParts { get; set; }
        public DbSet<CentarixID> CentarixIDs { get; set; }
        public DbSet<GlobalInfo> GlobalInfos { get; set; }
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
        public DbSet<EmployeeInfoNotificationStatus> EmployeeInfoNotificationStatuses { get; set; }
        public DbSet<NotificationStatus> NotificationStatuses { get; set; }
        public DbSet<RequestNotification> RequestNotifications { get; set; }
        public DbSet<TimekeeperNotification> TimekeeperNotifications { get; set; }
        //public DbSet<Notification<NotificationStatus>> Notifications { get; set; }
        public DbSet<EmployeeInfoNotification> EmployeeInfoNotifications { get; set; }
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
        public DbSet<RequestComment> RequestComments { get; set; }
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
        public DbSet<LocationRoomInstance> LocationRoomInstances { get; set; }
        public DbSet<RequestLocationInstance> RequestLocationInstances { get; set; }
        public DbSet<TemporaryLocationInstance> TemporaryLocationInstances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RequestListRequest>()
                .HasKey(v => new { v.ListID, v.RequestID });

            modelBuilder.Entity<VendorCategoryType>()
                .HasKey(v => new { v.VendorID, v.CategoryTypeID });

            modelBuilder.Entity<UnitTypeParentCategory>()
                .HasKey(u => new { u.UnitTypeID, u.ParentCategoryID });

            modelBuilder.Entity<ResourceResourceCategory>()
                .HasKey(rrc => new { rrc.ResourceID, rrc.ResourceCategoryID });

            modelBuilder.Entity<ResourceResourceCategory>()
                .HasOne(rrc => rrc.ResourceCategory)
                .WithMany(rc => rc.ResourceResourceCategories)
                .HasForeignKey(rrc => rrc.ResourceCategoryID);

            modelBuilder.Entity<ResourceResourceCategory>()
                .HasOne(rrc => rrc.Resource)
                .WithMany(r => r.ResourceResourceCategories)
                .HasForeignKey(rrc => rrc.ResourceID);

            modelBuilder.Entity<ExperimentTest>()
                .HasKey(et => new { et.ExperimentID, et.TestID });

            modelBuilder.Entity<ExperimentTest>()
                .HasOne(et => et.Experiment)
                .WithMany(e => e.ExperimentTests)
                .HasForeignKey(et => et.ExperimentID);

            modelBuilder.Entity<ExperimentTest>()
                .HasOne(et => et.Test)
                .WithMany(t => t.ExperimentTests)
                .HasForeignKey(et => et.TestID);

            modelBuilder.Entity<RequestLocationInstance>()
                .HasKey(rl => new { rl.RequestID, rl.LocationInstanceID });

            modelBuilder.Entity<RequestLocationInstance>()
                .HasOne(rl => rl.Request)
                .WithMany(r => r.RequestLocationInstances)
                .HasForeignKey(rl => rl.RequestID);

            modelBuilder.Entity<RequestLocationInstance>()
                .HasOne(rl => rl.LocationInstance)
                .WithMany(l => l.RequestLocationInstances)
                .HasForeignKey(rl => rl.LocationInstanceID);

            modelBuilder.Entity<RequestLocationInstance>()
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


            //modelBuilder.Entity<Test>()
            //    .HasOne(t => t.TestFieldHeader)
            //    .WithOne(tfh => tfh.Test)
            //    .HasForeignKey<Test>(t => t.TestFieldHeaderID);

            modelBuilder.Entity<Line>()
                  .Property(e => e.LineID)
                  .ValueGeneratedNever();

            modelBuilder.Entity<FunctionLine>()
                .Property(e => e.ID)
                .ValueGeneratedNever();

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

            modelBuilder.Entity<ShareResource>()
                .HasOne(sr => sr.FromApplicationUser)
                .WithMany(au => au.ShareResourcesCreated)
                .HasForeignKey(sr => sr.FromApplicationUserID);

            modelBuilder.Entity<ShareResource>()
                .HasOne(sr => sr.ToApplicationUser)
                .WithMany(au => au.ShareResourcesReceived)
                .HasForeignKey(sr => sr.ToApplicationUserID);

            //naming the constraints so they won't be droppped and readded

            // configures one-to-many relationship between Inventory and InventorySubcategories
            modelBuilder.Entity<ProductSubcategory>()
            .HasOne<ParentCategory>(ps => ps.ParentCategory)
            .WithMany(pc => pc.ProductSubcategories)
            .HasForeignKey(ps => ps.ParentCategoryID)
            .HasConstraintName("FK_ProductSubcategories_ParentCategory");

            modelBuilder.Entity<Product>()
                .HasOne<ProductSubcategory>(p => p.ProductSubcategory)
                .WithMany(ps => ps.Products)
                .HasForeignKey(p => p.ProductSubcategoryID)
                .HasConstraintName("FK_Products_ProductSubcategory");

            modelBuilder.Entity<ResourceResourceCategory>()
                .HasOne<ResourceCategory>(rrc => rrc.ResourceCategory)
                .WithMany(rc => rc.ResourceResourceCategories)
                .HasForeignKey(rrc => rrc.ResourceCategoryID)
                .HasConstraintName("FK_ResourceResourceCategory_ResourceCategories_ResourceCategoryID");

            modelBuilder.Entity<Report>()
                .HasOne<ResourceCategory>(r => r.ReportCategory)
                .WithMany(rc => rc.Reports)
                .HasForeignKey(r => r.ReportCategoryID)
                .HasConstraintName("FK_Reports_ResourceCategories_ReportCategoryID");

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

            modelBuilder.Entity<Product>()
            .HasQueryFilter(item => !item.IsDeleted);

            modelBuilder.Entity<ParentQuote>()
                .HasQueryFilter(item => !item.IsDeleted);

            modelBuilder.Entity<Calibration>()
                .HasQueryFilter(item => !item.IsDeleted);

            modelBuilder.Entity<Payment>()
                .HasQueryFilter(item => !item.IsDeleted);

            modelBuilder.Entity<RequestComment>()
                .HasQueryFilter(item => !item.IsDeleted);

            modelBuilder.Entity<ProductComment>()
                .HasQueryFilter(item => !item.IsDeleted);

            modelBuilder.Entity<VendorComment>()
                .HasQueryFilter(item => !item.IsDeleted);
            //modelBuilder.Entity<LocationInstance>()
            //    .HasQueryFilter(item => !(item is TemporaryLocationInstance));

            modelBuilder.Entity<Material>()
                .HasQueryFilter(item => !item.IsDeleted);

            modelBuilder.Entity<ProductSubcategory>()
           .HasQueryFilter(item => !item.IsOldSubCategory);

            modelBuilder.Entity<LocationType>()
           .HasQueryFilter(item => item.LocationTypeID != 600);

            modelBuilder.Entity<LocationInstance>()
        .HasQueryFilter(item => item.LocationTypeID != 600);

            modelBuilder.Entity<SalariedEmployee>().Ignore(e => e.WorkScope);
            modelBuilder.Entity<Employee>().Ignore(e => e.NetSalary);
            modelBuilder.Entity<Employee>().Ignore(e => e.TotalCost);
            modelBuilder.Entity<Employee>().Ignore(e => e.SickDays);
            modelBuilder.Entity<Employee>().Ignore(e => e.SickDaysPerMonth);
            modelBuilder.Entity<Employee>().Ignore(e => e.VacationDaysPerMonth);
            modelBuilder.Entity<Request>().Ignore(e => e.VAT);
            modelBuilder.Entity<Request>().Ignore(e => e.PricePerUnit);
            modelBuilder.Entity<Request>().Ignore(e => e.TotalWithVat);
            modelBuilder.Entity<Request>().Ignore(e => e.Ignore);
            modelBuilder.Entity<Request>().Ignore(e => e.IsReceived);
            modelBuilder.Entity<Request>().Ignore(e => e.SingleOrder);
            modelBuilder.Entity<Request>().Ignore(e => e.RecurringOrder);
            modelBuilder.Entity<Request>().Ignore(e => e.StandingOrder);
            modelBuilder.Entity<ParentQuote>().Ignore(e => e.ExpirationDate_submit);
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
            modelBuilder.Entity<Request>().Ignore(e => e.SerialNumberString);
            modelBuilder.Entity<EmployeeHoursAwaitingApproval>().Property(e => e.IsDenied).HasDefaultValue(false);
            modelBuilder.Entity<ApplicationUser>().HasIndex(a => a.UserNum).IsUnique();
            modelBuilder.Entity<Request>().Property(r => r.ExchangeRate).HasColumnType("decimal(18,3)");
            modelBuilder.Entity<Product>().Property(r => r.ProductCreationDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<ParentQuote>().Property(r => r.QuoteDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<TempLineID>().Property(r => r.DateCreated).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<FunctionLineID>().Property(r => r.DateCreated).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<ParentRequest>().HasIndex(p => p.OrderNumber).IsUnique();
            modelBuilder.Entity<ParentRequest>().HasIndex(p => p.QuartzyOrderNumber).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(p => p.SerialNumber).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(p => new { p.SerialNumber, p.VendorID }).IsUnique();
            modelBuilder.Entity<ShareRequest>().Property(sb => sb.TimeStamp).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("getdate()");
            modelBuilder.Entity<ShareRequestList>().Property(sb => sb.TimeStamp).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("getdate()");
            modelBuilder.Entity<ShareProtocol>().Property(sb => sb.TimeStamp).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("getdate()");
            modelBuilder.Entity<ShareResource>().Property(sb => sb.TimeStamp).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("getdate()");
            modelBuilder.Entity<TestHeader>().HasIndex(th => new { th.SequencePosition, th.TestGroupID }).IsUnique();
            modelBuilder.Entity<ExperimentEntry>().HasIndex(ee => new { ee.ParticipantID, ee.VisitNumber }).IsUnique();
            modelBuilder.Entity<Request>().HasIndex(r => r.SerialNumber).IsUnique();
            modelBuilder.Entity<Participant>().Property(r => r.DateCreated).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<ExperimentEntry>().Property(r => r.DateCreated).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<TimekeeperNotification>().Property(tn => tn.TimeStamp).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("getdate()");
            modelBuilder.Entity<RequestNotification>().Property(rn => rn.TimeStamp).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("getdate()");
            modelBuilder.Entity<EmployeeInfoNotification>().Property(en => en.TimeStamp).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Vendor>().HasIndex(v => new { v.CountryID, v.VendorBuisnessID }).IsUnique();
            modelBuilder.Entity<Participant>().HasIndex(p => new { p.ParticipantID, p.CentarixID }).IsUnique();
            modelBuilder.HasSequence<int>("SerialNumberHelper", schema: "dbo").StartsAt(1).IncrementsBy(1);
            modelBuilder.Entity<Request>().Property(r => r.SerialNumber).HasDefaultValueSql("NEXT VALUE FOR dbo.SerialNumberHelper");

            /*PROTOCOLS*/
            ///set up composite keys

            modelBuilder.Entity<TagProtocol>()
              .HasKey(t => new { t.TagID, t.ProtocolID });

            modelBuilder.Entity<AuthorProtocol>()
                .HasKey(a => new { a.AuthorID, a.ProtocolID });

            //set up many to many relationshipw
            modelBuilder.Entity<LineType>()
                .HasOne(l => l.LineTypeParent)
                .WithMany()
                .HasForeignKey(ltp => ltp.LineTypeParentID);

            modelBuilder.Entity<LineType>()
               .HasOne(lt => lt.LineTypeChild)
               .WithMany()
               .HasForeignKey(ltc => ltc.LineTypeChildID);

            modelBuilder.Entity<LineChange>()
           .HasKey(lc => new { lc.LineID, lc.ProtocolInstanceID });



            modelBuilder.Entity<Report>().Property(r => r.ReportText).HasColumnType("ntext");
            modelBuilder.Entity<Resource>().Property(r => r.Summary).HasColumnType("ntext");
            modelBuilder.Entity<ResourceNote>().Property(r => r.Note).HasColumnType("ntext");
            modelBuilder.Entity<ProtocolInstance>().Property(r => r.ResultDescription).HasColumnType("ntext");
            modelBuilder.Entity<TempRequestJson>().Property(t => t.Json).HasColumnType("ntext");
            modelBuilder.Entity<TempLinesJson>().Property(t => t.Json).HasColumnType("ntext");
            modelBuilder.Entity<Protocol>().HasIndex(p => new { p.UniqueCode }).IsUnique();
            modelBuilder.Entity<ProtocolVersion>().HasIndex(p => new { p.ProtocolID, p.VersionNumber }).IsUnique();
            modelBuilder.Entity<ProtocolVersion>().Ignore(p => p.Name);
            modelBuilder.Seed();

            //foreach loop ensures that deletion is resticted - no cascade delete
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }




        }

    }
}

