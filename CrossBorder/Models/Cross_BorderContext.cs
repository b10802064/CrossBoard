using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using cross_border.ViewModels;

#nullable disable

namespace CrossBorder.Models
{
    public partial class Cross_BorderContext : DbContext
    {
        public Cross_BorderContext()
        {
        }

        public Cross_BorderContext(DbContextOptions<Cross_BorderContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Classified> Classifieds { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<Shoppinglist> Shoppinglists { get; set; }
        public virtual DbSet<Type> Types { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Classified>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.TypeId });

                entity.ToTable("Classified");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.TypeId)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Classifieds)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Classified_Product");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Classifieds)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Classified_Type");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country");

                entity.Property(e => e.CountryId)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength(true);

                entity.Property(e => e.Prefix)
                    .HasMaxLength(50)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.CusdtomerName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength(true);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.Photo)
                    .HasMaxLength(100)
                    .IsFixedLength(true);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.CountryId });

                entity.ToTable("Sale");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.CountryId)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sale_Country");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sale_Product");
            });

            modelBuilder.Entity<Shoppinglist>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.ProductId });

                entity.ToTable("Shoppinglist");

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.ProductId)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Shoppinglists)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shoppinglist_Customer");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Shoppinglists)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shoppinglist_Product");
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.ToTable("Type");

                entity.Property(e => e.TypeId)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength(true);
            });

            var customer_data = new List<Customer>
            {
                new Customer
                {
                    CustomerId = "001",
                    CusdtomerName = "haah",
                    Email = "haah@gmail.com",
                    Password = "12345"
                },
                new Customer
                {
                    CustomerId = "002",
                    CusdtomerName = "haaj",
                    Email = "haaj@gmail.com",
                    Password = "123456"
                }
            };

            modelBuilder.Entity<Customer>().HasData(customer_data);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public DbSet<cross_border.ViewModels.RegisterViewModel> RegisterViewModel { get; set; }
    }
}
