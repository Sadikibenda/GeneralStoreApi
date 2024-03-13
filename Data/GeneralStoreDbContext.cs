using System;
using System.Collections.Generic;
using GeneralStoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GeneralStoreApi.Data;

public partial class GeneralStoreDbContext : DbContext
{
    public GeneralStoreDbContext()
    {
    }

    public GeneralStoreDbContext(DbContextOptions<GeneralStoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:GeneralStoreDb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC0736475B26");

            entity.ToTable("Customers", "dev");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC076CE8FDCD");

            entity.ToTable("Products", "dev");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC071DAE7D97");

            entity.ToTable("Transactions", "dev");

            entity.Property(e => e.DateOfTransaction).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Custo__3F466844");

            entity.HasOne(d => d.Product).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Produ__3E52440B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
