﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PrototypeWithAuth.Data;

namespace PrototypeWithAuth.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("PrototypeWithAuth.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("PrototypeWithAuth.Models.Inventory", b =>
                {
                    b.Property<int>("InventoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("InventorySubcategoryID")
                        .HasColumnType("int");

                    b.Property<string>("ItemDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ItemPaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ItemReceivedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("InventoryID");

                    b.HasIndex("InventorySubcategoryID");

                    b.ToTable("Inventories");
                });

            modelBuilder.Entity("PrototypeWithAuth.Models.InventorySubcategory", b =>
                {
                    b.Property<int>("InventorySubcategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("InventorySubcategoryDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("InventorySubcategoryID");

                    b.ToTable("InventorySubcategories");
                });

            modelBuilder.Entity("PrototypeWithAuth.Models.ParentCategory", b =>
                {
                    b.Property<int>("ParentCategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ParentCategoryDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ParentCategoryID");

                    b.ToTable("ParentCategories");

                    b.HasData(
                        new
                        {
                            ParentCategoryID = 1,
                            ParentCategoryDescription = "Plastics"
                        },
                        new
                        {
                            ParentCategoryID = 2,
                            ParentCategoryDescription = "Reagents"
                        },
                        new
                        {
                            ParentCategoryID = 3,
                            ParentCategoryDescription = "Proprietry"
                        },
                        new
                        {
                            ParentCategoryID = 4,
                            ParentCategoryDescription = "Reusable"
                        });
                });

            modelBuilder.Entity("PrototypeWithAuth.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("LocationID")
                        .HasColumnType("int");

                    b.Property<string>("ProductComment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductMedia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductSubcategoryID")
                        .HasColumnType("int");

                    b.Property<int>("QuantityPerUnit")
                        .HasColumnType("int");

                    b.Property<int>("ReorderLevel")
                        .HasColumnType("int");

                    b.Property<int>("UnitsInOrder")
                        .HasColumnType("int");

                    b.Property<int>("UnitsInStock")
                        .HasColumnType("int");

                    b.Property<int>("VendorID")
                        .HasColumnType("int");

                    b.HasKey("ProductID");

                    b.HasIndex("ProductSubcategoryID");

                    b.HasIndex("VendorID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("PrototypeWithAuth.Models.ProductSubcategory", b =>
                {
                    b.Property<int>("ProductSubcategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ParentCategoryID")
                        .HasColumnType("int");

                    b.Property<string>("ProductSubcategoryDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductSubcategoryID");

                    b.HasIndex("ParentCategoryID");

                    b.ToTable("ProductSubcategories");

                    b.HasData(
                        new
                        {
                            ProductSubcategoryID = 11,
                            ParentCategoryID = 1,
                            ProductSubcategoryDescription = "Tubes"
                        },
                        new
                        {
                            ProductSubcategoryID = 12,
                            ParentCategoryID = 1,
                            ProductSubcategoryDescription = "Pipets"
                        },
                        new
                        {
                            ProductSubcategoryID = 13,
                            ParentCategoryID = 1,
                            ProductSubcategoryDescription = "Tips"
                        },
                        new
                        {
                            ProductSubcategoryID = 14,
                            ParentCategoryID = 1,
                            ProductSubcategoryDescription = "Dishes"
                        },
                        new
                        {
                            ProductSubcategoryID = 15,
                            ParentCategoryID = 1,
                            ProductSubcategoryDescription = "Cell Culture Plates"
                        },
                        new
                        {
                            ProductSubcategoryID = 21,
                            ParentCategoryID = 2,
                            ProductSubcategoryDescription = "Chemical Powder"
                        },
                        new
                        {
                            ProductSubcategoryID = 22,
                            ParentCategoryID = 2,
                            ProductSubcategoryDescription = "DNA Enzyme"
                        },
                        new
                        {
                            ProductSubcategoryID = 23,
                            ParentCategoryID = 2,
                            ProductSubcategoryDescription = "Antibodies"
                        },
                        new
                        {
                            ProductSubcategoryID = 24,
                            ParentCategoryID = 2,
                            ProductSubcategoryDescription = "Cell Media"
                        },
                        new
                        {
                            ProductSubcategoryID = 31,
                            ParentCategoryID = 3,
                            ProductSubcategoryDescription = "Virus"
                        },
                        new
                        {
                            ProductSubcategoryID = 32,
                            ParentCategoryID = 3,
                            ProductSubcategoryDescription = "Plasmid"
                        },
                        new
                        {
                            ProductSubcategoryID = 33,
                            ParentCategoryID = 3,
                            ProductSubcategoryDescription = "Primers"
                        },
                        new
                        {
                            ProductSubcategoryID = 41,
                            ParentCategoryID = 4,
                            ProductSubcategoryDescription = "Beaker"
                        },
                        new
                        {
                            ProductSubcategoryID = 42,
                            ParentCategoryID = 4,
                            ProductSubcategoryDescription = "Buckets"
                        },
                        new
                        {
                            ProductSubcategoryID = 43,
                            ParentCategoryID = 4,
                            ProductSubcategoryDescription = "Measuring Instruments"
                        });
                });

            modelBuilder.Entity("PrototypeWithAuth.Models.Vendor", b =>
                {
                    b.Property<int>("VendorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContactEmail")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("ContactPerson")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("OrderEmail")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("VendorAccountNum")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VendorBIC")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VendorBank")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("VendorBankBranch")
                        .IsRequired()
                        .HasColumnType("nvarchar(4)")
                        .HasMaxLength(4);

                    b.Property<string>("VendorBuisnessID")
                        .IsRequired()
                        .HasColumnType("nvarchar(9)")
                        .HasMaxLength(9);

                    b.Property<string>("VendorCity")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("VendorContactPhone1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VendorContactPhone2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VendorEnName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("VendorFax")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VendorGoldAccount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VendorHeName")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("VendorStreet")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("VendorSwift")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VendorWebsite")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VendorZip")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VendorID");

                    b.ToTable("Vendors");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("PrototypeWithAuth.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("PrototypeWithAuth.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PrototypeWithAuth.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("PrototypeWithAuth.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PrototypeWithAuth.Models.Inventory", b =>
                {
                    b.HasOne("PrototypeWithAuth.Models.InventorySubcategory", "InventorySubcategory")
                        .WithMany("Inventories")
                        .HasForeignKey("InventorySubcategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PrototypeWithAuth.Models.Product", b =>
                {
                    b.HasOne("PrototypeWithAuth.Models.ProductSubcategory", "ProductSubcategory")
                        .WithMany()
                        .HasForeignKey("ProductSubcategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PrototypeWithAuth.Models.Vendor", "Vendor")
                        .WithMany()
                        .HasForeignKey("VendorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PrototypeWithAuth.Models.ProductSubcategory", b =>
                {
                    b.HasOne("PrototypeWithAuth.Models.ParentCategory", "ParentCategory")
                        .WithMany("ProductSubcategories")
                        .HasForeignKey("ParentCategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
