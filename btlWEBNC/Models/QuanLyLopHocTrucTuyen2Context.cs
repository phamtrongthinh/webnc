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

    public virtual DbSet<TblCourse> TblCourses { get; set; }

    public virtual DbSet<TblEnrollment> TblEnrollments { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=QuanLyLopHocTrucTuyen2;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblCourse>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__tblCours__C92D7187B629BF9B");

            entity.ToTable("tblCourses", tb => tb.HasTrigger("trg_CheckTeacherRole"));

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Teacher).WithMany(p => p.TblCourses)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__tblCourse__Teach__5165187F");
        });

        modelBuilder.Entity<TblEnrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PK__tblEnrol__7F6877FB36C962AE");

            entity.ToTable("tblEnrollments", tb => tb.HasTrigger("trg_CheckStudentRole"));

            entity.Property(e => e.EnrollmentId).HasColumnName("EnrollmentID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.EnrollmentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Course).WithMany(p => p.TblEnrollments)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__tblEnroll__Cours__571DF1D5");

            entity.HasOne(d => d.Student).WithMany(p => p.TblEnrollments)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__tblEnroll__Stude__5629CD9C");
        });

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
