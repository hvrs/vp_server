using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

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

    public virtual DbSet<ExcelDocument> ExcelDocuments { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<NicotineType> NicotineTypes { get; set; }

    public virtual DbSet<PaymentDetail> PaymentDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductBasket> ProductBaskets { get; set; }

    public virtual DbSet<ProductCount> ProductCounts { get; set; }

    public virtual DbSet<ReplenishmentProduct> ReplenishmentProducts { get; set; }

    public virtual DbSet<Strenght> Strenghts { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionsAndProduct> TransactionsAndProducts { get; set; }

    public virtual DbSet<TransactionStatus> TransactionStatuses { get; set; }

    public virtual DbSet<View> Views { get; set; }

    public virtual DbSet<Statusmain> Statusmains { get; set; }
    public virtual DbSet<RemoveTime> RemoveTimes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=Passw0rd;database=vapeshop", ServerVersion.Parse("8.2.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("category");

            entity.HasIndex(e => e.ParentCategoryId, "ParentCategoryID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CategoryName).HasMaxLength(50);
            entity.Property(e => e.isProduct).HasColumnName("isProduct");
            entity.Property(e => e.ParentCategoryId).HasColumnName("ParentCategoryID");

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("category_ibfk_1");
        });

        modelBuilder.Entity<ExcelDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("exceldocument");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DocExcel).HasColumnType("mediumblob");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("manufacturer");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<NicotineType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("nicotinetype");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<PaymentDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("paymentdetails");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BankInn)
                .HasMaxLength(50)
                .HasColumnName("BankINN");
            entity.Property(e => e.BankKpp)
                .HasMaxLength(50)
                .HasColumnName("BankKPP");
            entity.Property(e => e.BankKs)
                .HasMaxLength(50)
                .HasColumnName("BankKS");
            entity.Property(e => e.BankName).HasMaxLength(150);
            entity.Property(e => e.Bik)
                .HasMaxLength(50)
                .HasColumnName("BIK");
            entity.Property(e => e.FirmName).HasMaxLength(150);
            entity.Property(e => e.PersonalRs)
                .HasMaxLength(50)
                .HasColumnName("PersonalRS");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("product");

            entity.HasIndex(e => e.CategoryId, "CategoryID");

            entity.HasIndex(e => e.ManufacturerId, "ManufacturerID");

            entity.HasIndex(e => e.NicotineTypeId, "NicotineTypeID");

            entity.HasIndex(e => e.StrengthId, "StrengthID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Image).HasColumnType("mediumblob");
            entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerID");
            entity.Property(e => e.Material).HasMaxLength(50);
            entity.Property(e => e.NicotineTypeId).HasColumnName("NicotineTypeID");
            entity.Property(e => e.StrengthId).HasColumnName("StrengthID");
            entity.Property(e => e.Taste).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("product_ibfk_1");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Products)
                .HasForeignKey(d => d.ManufacturerId)
                .HasConstraintName("product_ibfk_4");

            entity.HasOne(d => d.NicotineType).WithMany(p => p.Products)
                .HasForeignKey(d => d.NicotineTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("product_ibfk_2");

            entity.HasOne(d => d.Strength).WithMany(p => p.Products)
                .HasForeignKey(d => d.StrengthId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("product_ibfk_3");
        });

        modelBuilder.Entity<ProductBasket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("productbasket");

            entity.HasIndex(e => e.ProductId, "ProductID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductBaskets)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("productbasket_ibfk_1");
        });

        modelBuilder.Entity<ProductCount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("productcount");

            entity.HasIndex(e => e.ProductId, "ProductID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductCounts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("productcount_ibfk_1");
        });

        modelBuilder.Entity<RemoveTime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("removetime");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DaysToRemove).HasColumnName("DaysToRemove");
        });

        modelBuilder.Entity<ReplenishmentProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("replenishmentproduct");

            entity.HasIndex(e => e.ProductId, "ProductID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.ReplenishmentProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("replenishmentproduct_ibfk_1");
        });

        modelBuilder.Entity<Strenght>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("strenght");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("transactions");

            entity.HasIndex(e => e.TransactionStatusId, "TransactionStatusID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IsViewed).HasColumnName("isViewed");
            entity.Property(e => e.Time).HasColumnType("time");
            entity.Property(e => e.TransactionStatusId).HasColumnName("TransactionStatusID");

            entity.HasOne(d => d.TransactionStatus).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.TransactionStatusId)
                .HasConstraintName("transactions_ibfk_1");
        });

        modelBuilder.Entity<TransactionsAndProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("transactionsandproduct");

            entity.HasIndex(e => e.ProductId, "ProductID");

            entity.HasIndex(e => e.TransactionId, "TransactionID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

            entity.HasOne(d => d.Product).WithMany(p => p.TransactionsAndProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("transactionsandproduct_ibfk_2");

            entity.HasOne(d => d.Transaction).WithMany(p => p.TransactionsAndProducts)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("transactionsandproduct_ibfk_1");
        });

        modelBuilder.Entity<TransactionStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("transactionstatus");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<View>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("views");

            entity.HasIndex(e => e.ProductId, "ProductID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Time).HasColumnType("time");

            entity.HasOne(d => d.Product).WithMany(p => p.Views)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("views_ibfk_1");
        });

        modelBuilder.Entity<Statusmain>(entity =>
        {
            entity.HasKey(e => e.IdStatusMain).HasName("PRIMARY");

            entity.ToTable("statusmain");

            entity.Property(e => e.IdStatusMain).HasColumnName("idStatusMain");
            entity.Property(e => e.IsProductType).HasColumnName("isProductType");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
