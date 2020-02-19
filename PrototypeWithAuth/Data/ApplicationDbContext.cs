using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.Data;

namespace PrototypeWithAuth.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {




        }
        public DbSet<RequestStatus> RequestStatuses { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Product> Products  { get; set; }
        public DbSet <ProductSubcategory> ProductSubcategories { get; set; }
        public DbSet <ParentCategory> ParentCategories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // configures one-to-many relationship between Inventory and InventorySubcategories
          
            modelBuilder.Entity<ProductSubcategory>()
            .HasOne<ParentCategory>(ps => ps.ParentCategory)
            .WithMany(pc => pc.ProductSubcategories)
            .HasForeignKey(ps => ps.ParentCategoryID);
            
            modelBuilder.Entity<Request>()
            .HasOne<ApplicationUser>(r => r.ApplicationUser)
            .WithMany(au => au.Requests)
            .HasForeignKey(r => r.ApplicationUserID);

            modelBuilder.Entity<Request>()
            .HasOne<RequestStatus>(r => r.RequestStatus)
            .WithMany(rs => rs.Requests)
            .HasForeignKey(r => r.RequestStatusID);

            modelBuilder.Entity<Vendor>()
            .HasOne<ParentCategory>(v => v.ParentCategory)
            .WithMany(pc => pc.Vendors)
            .HasForeignKey(v => v.ParentCategoryID)
            .OnDelete(DeleteBehavior.Restrict);
            

            modelBuilder.Seed();
        }
    }
}

