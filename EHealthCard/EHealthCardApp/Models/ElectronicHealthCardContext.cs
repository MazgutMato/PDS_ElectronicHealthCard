using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ElectronicHealthCardApp.Models;

public partial class ElectronicHealthCardContext : DbContext
{
    public ElectronicHealthCardContext()
    {
    }

    public ElectronicHealthCardContext(DbContextOptions<ElectronicHealthCardContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Diagnosis> Diagnoses { get; set; }

    public virtual DbSet<DiagnosisType> DiagnosisTypes { get; set; }

    public virtual DbSet<Hospital> Hospitals { get; set; }

    public virtual DbSet<Hospitalization> Hospitalizations { get; set; }

    public virtual DbSet<Insurance> Insurances { get; set; }

    public virtual DbSet<InsuranceComp> InsuranceComps { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Person> People { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBEHeathCard;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Zip);

            entity.ToTable("city");

            entity.Property(e => e.Zip)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ZIP");
            entity.Property(e => e.CityName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("city_name");
        });

        modelBuilder.Entity<Diagnosis>(entity =>
        {
            entity.HasKey(e => new { e.DateStart, e.HospitalName, e.PersonId, e.DiagnosisId });

            entity.ToTable("diagnoses");

            entity.Property(e => e.DateStart)
                .HasColumnType("date")
                .HasColumnName("date_start");
            entity.Property(e => e.HospitalName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("hospital_name");
            entity.Property(e => e.PersonId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("person_id");
            entity.Property(e => e.DiagnosisId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("diagnosis_id");
            entity.Property(e => e.Document)
                .HasColumnType("image")
                .HasColumnName("document");

            entity.HasOne(d => d.DiagnosisNavigation).WithMany(p => p.Diagnoses)
                .HasForeignKey(d => d.DiagnosisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Relationship25");

            entity.HasOne(d => d.Hospitalization).WithMany(p => p.Diagnoses)
                .HasForeignKey(d => new { d.DateStart, d.HospitalName, d.PersonId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Relationship24");
        });

        modelBuilder.Entity<DiagnosisType>(entity =>
        {
            entity.HasKey(e => e.DiagnosisId);

            entity.ToTable("diagnosis_type");

            entity.Property(e => e.DiagnosisId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("diagnosis_id");
            entity.Property(e => e.DailyCosts)
                .HasColumnType("money")
                .HasColumnName("daily_costs");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("description");
        });

        modelBuilder.Entity<Hospital>(entity =>
        {
            entity.HasKey(e => e.HospitalName);

            entity.ToTable("hospital");

            entity.HasIndex(e => e.Zip, "IX_Relationship9");

            entity.Property(e => e.HospitalName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("hospital_name");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.Zip)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ZIP");

            entity.HasOne(d => d.ZipNavigation).WithMany(p => p.Hospitals)
                .HasForeignKey(d => d.Zip)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Relationship9");
        });

        modelBuilder.Entity<Hospitalization>(entity =>
        {
            entity.HasKey(e => new { e.DateStart, e.HospitalName, e.PersonId });

            entity.ToTable("hospitalization");

            entity.Property(e => e.DateStart)
                .HasColumnType("date")
                .HasColumnName("date_start");
            entity.Property(e => e.HospitalName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("hospital_name");
            entity.Property(e => e.PersonId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("person_id");
            entity.Property(e => e.DateEnd)
                .HasColumnType("date")
                .HasColumnName("date_end");

            entity.HasOne(d => d.HospitalNameNavigation).WithMany(p => p.Hospitalizations)
                .HasForeignKey(d => d.HospitalName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Relationship22");

            entity.HasOne(d => d.Person).WithMany(p => p.Hospitalizations)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Relationship23");
        });

        modelBuilder.Entity<Insurance>(entity =>
        {
            entity.HasKey(e => new { e.PersonId, e.CompId, e.DateStart });

            entity.ToTable("insurance");

            entity.Property(e => e.PersonId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("person_id");
            entity.Property(e => e.CompId)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("comp_id");
            entity.Property(e => e.DateStart)
                .HasColumnType("date")
                .HasColumnName("date_start");
            entity.Property(e => e.DateEnd)
                .HasColumnType("date")
                .HasColumnName("date_end");

            entity.HasOne(d => d.Comp).WithMany(p => p.Insurances)
                .HasForeignKey(d => d.CompId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Relationship3");

            entity.HasOne(d => d.Person).WithMany(p => p.Insurances)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Relationship2");
        });

        modelBuilder.Entity<InsuranceComp>(entity =>
        {
            entity.HasKey(e => e.CompId);

            entity.ToTable("insurance_comp");

            entity.Property(e => e.CompId)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("comp_id");
            entity.Property(e => e.CompName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("comp_name");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => new { e.HospitalName, e.CompId });

            entity.ToTable("payment");

            entity.Property(e => e.HospitalName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("hospital_name");
            entity.Property(e => e.CompId)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("comp_id");
            entity.Property(e => e.Details)
                .HasColumnType("xml")
                .HasColumnName("details");
            entity.Property(e => e.PaymentDate)
                .HasColumnType("date")
                .HasColumnName("payment_date");
            entity.Property(e => e.PaymentPeriod)
                .HasColumnType("date")
                .HasColumnName("payment_period");

            entity.HasOne(d => d.Comp).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CompId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Relationship28");

            entity.HasOne(d => d.HospitalNameNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.HospitalName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Relationship26");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("person");

            entity.HasIndex(e => e.Zip, "IX_Relationship4");

            entity.Property(e => e.PersonId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("person_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Zip)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ZIP");

            entity.HasOne(d => d.ZipNavigation).WithMany(p => p.People)
                .HasForeignKey(d => d.Zip)
                .HasConstraintName("Relationship4");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
