using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.Data;
using System.Linq;
using PrototypeWithAuth.Models.LocationsTierInstantiation;


namespace PrototypeWithAuth.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {




        }
        public DbSet<LocationsTier1Instance> LocationsTier1Instance { get; set; }
        public DbSet<LocationsTier2Instance> LocationsTier2Instance { get; set; }
        public DbSet<LocationsTier3Instance> LocationsTier3Instance { get; set; }
        public DbSet<LocationsTier4Instance> LocationsTier4Instance { get; set; }
        public DbSet<LocationsTier5Instance> LocationsTier5Instance { get; set; }
        public DbSet<LocationsTier6Instance> LocationsTier6Instance { get; set; }
        public DbSet<LocationsTier1Model> LocationsTier1Models { get; set; }
        public DbSet<LocationsTier2Model> LocationsTier2Models { get; set; }
        public DbSet<LocationsTier3Model> LocationsTier3Models { get; set; }
        public DbSet<LocationsTier4Model> LocationsTier4Models { get; set; }
        public DbSet<LocationsTier5Model> LocationsTier5Models { get; set; }
        public DbSet<LocationsTier6Model> LocationsTier6Models { get; set; }
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
            // configures one-to-many relationship between Inventory and InventorySubcategories
            
           
            modelBuilder.Entity<ProductSubcategory>()
            .HasOne<ParentCategory>(ps => ps.ParentCategory)
            .WithMany(pc => pc.ProductSubcategories)
            .HasForeignKey(ps => ps.ParentCategoryID);

            modelBuilder.Entity<UnitType>()
           .HasOne<UnitParentType>(ut => ut.UnitParentType)
           .WithMany(upt => upt.UnitTypes)
           .HasForeignKey(ut => ut.UnitParentTypeID);

            modelBuilder.Entity<ParentRequest>()
            .HasOne<ApplicationUser>(pr => pr.ApplicationUser)
            .WithMany(au => au.ParentRequests)
            .HasForeignKey(r => r.ApplicationUserID);

            //modelBuilder.Entity<Request>()
            //.HasOne<Product>(r => r.Product)
            //.WithMany(p => p.Requests)
            //.HasForeignKey(r => r.ProductID)
            //.OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Request>()
            .HasOne<RequestStatus>(r => r.RequestStatus)
            .WithMany(rs => rs.Requests)
            .HasForeignKey(r => r.RequestStatusID);

            //modelBuilder.Entity<Vendor>()
            //.HasOne<ParentCategory>(v => v.ParentCategory)
            //.WithMany(pc => pc.Vendors)
            //.HasForeignKey(v => v.ParentCategoryID)
            //.OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Seed();

            //foreach loop ensures that deletion is resticted - no cascade delete
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }
    }
}

