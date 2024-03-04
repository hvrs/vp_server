using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace vp_server.Models;

public partial class VapeshopContext : DbContext
{
    public VapeshopContext()
    {
    }

    public VapeshopContext(DbContextOptions<VapeshopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<NicotineType> NicotineTypes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCount> ProductCounts { get; set; }

    public virtual DbSet<Strenght> Strenghts { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionsAndProduct> TransactionsAndProducts { get; set; }

    public virtual DbSet<View> Views { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(local);Initial Catalog=vapeshop;Integrated Security=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CategoryName).HasMaxLength(50);
            entity.Property(e => e.ParentCategoryId).HasColumnName("ParentCategoryID");

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .HasConstraintName("FK_Category_Category");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.ToTable("Manufacturer");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<NicotineType>(entity =>
        {
            entity.ToTable("NicotineType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerID");
            entity.Property(e => e.Material).HasMaxLength(50);
            entity.Property(e => e.NicotineTypeId).HasColumnName("NicotineTypeID");
            entity.Property(e => e.StrengthId).HasColumnName("StrengthID");
            entity.Property(e => e.Taste).HasMaxLength(50);
            entity.Property(e => e.Title).HasColumnType("text");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Product_Category");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Products)
                .HasForeignKey(d => d.ManufacturerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Product_Manufacturer");

            entity.HasOne(d => d.NicotineType).WithMany(p => p.Products)
                .HasForeignKey(d => d.NicotineTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Product_NicotineType");

            entity.HasOne(d => d.Strength).WithMany(p => p.Products)
                .HasForeignKey(d => d.StrengthId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Product_Strenght");
        });

        modelBuilder.Entity<ProductCount>(entity =>
        {
            entity.ToTable("ProductCount");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductCounts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductCount_Product");
        });

        modelBuilder.Entity<Strenght>(entity =>
        {
            entity.ToTable("Strenght");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.IsViewed).HasColumnName("isViewed");
        });

        modelBuilder.Entity<TransactionsAndProduct>(entity =>
        {
            entity.ToTable("TransactionsAndProduct");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

            entity.HasOne(d => d.Product).WithMany(p => p.TransactionsAndProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_TransactionsAndProduct_Product");

            entity.HasOne(d => d.Transaction).WithMany(p => p.TransactionsAndProducts)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("FK_TransactionsAndProduct_Transactions");
        });

        modelBuilder.Entity<View>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.Views)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Views_Product");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
