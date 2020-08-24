﻿using System;
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

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        //public DbSet<RequestLocationInstance> RequestLocationInstances { get; set; } // do we not need to include this set in the db context???
        public DbSet<RequestNotification> RequestNotifications { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PaymentStatus> PaymentStatuses { get; set; }
        public DbSet<Reorder> Reorder { get; set; }
        public DbSet<ParentQuote> ParentQuotes { get; set; }
        public DbSet<QuoteStatus> QuoteStatuses { get; set; }
        public DbSet<CategoryType> CategoryTypes { get; set; }
        public DbSet<VendorComment> VendorComments { get; set; }
        public DbSet<VendorContact> VendorContacts { get; set; }
        public DbSet<Menu> Menus { get; set; }
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
        public DbSet<Product> Products  { get; set; }
        public DbSet <ProductSubcategory> ProductSubcategories { get; set; }
        public DbSet <ParentCategory> ParentCategories { get; set; }
        public DbSet<UnitType> UnitTypes { get; set; }
        public DbSet<UnitParentType> UnitParentTypes {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            //modelBuilder.Entity<Vendor>()
            //.HasOne<ParentCategory>(v => v.ParentCategory)
            //.WithMany(pc => pc.Vendors)
            //.HasForeignKey(v => v.ParentCategoryID)
            //.OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ParentQuote>()
           .HasQueryFilter(item => !item.IsDeleted);






            modelBuilder.Seed();

            //foreach loop ensures that deletion is resticted - no cascade delete
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }
    }
}

