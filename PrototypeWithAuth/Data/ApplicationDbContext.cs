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
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Product> Products  { get; set; }
        public DbSet <ProductSubcategory> ProductSubcategories { get; set; }
        public DbSet <ParentCategory> ParentCategories { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventorySubcategory> InventorySubcategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // configures one-to-many relationship between Inventory and InventorySubcategories
            modelBuilder.Entity<Inventory>()
                .HasOne <InventorySubcategory>(i => i.InventorySubcategory)
                .WithMany(isc => isc.Inventories)
                .HasForeignKey(i => i.InventorySubcategoryID);

            modelBuilder.Seed();
        }
    }
}

