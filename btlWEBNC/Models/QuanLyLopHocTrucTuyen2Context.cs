using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace btlWEBNC.Models;

public partial class QuanLyLopHocTrucTuyen2Context : DbContext
{
    public QuanLyLopHocTrucTuyen2Context()
    {
    }

    public QuanLyLopHocTrucTuyen2Context(DbContextOptions<QuanLyLopHocTrucTuyen2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__tblUsers__1788CCACE14F9582");

            entity.ToTable("tblUsers");

            entity.HasIndex(e => e.Username, "UQ__tblUsers__536C85E4C8727215").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__tblUsers__A9D105347D419190").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(10);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
