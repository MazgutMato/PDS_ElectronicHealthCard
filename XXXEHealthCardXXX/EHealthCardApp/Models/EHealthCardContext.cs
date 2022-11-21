using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EHealthCardApp.Models
{
    public partial class EHealthCardContext : DbContext
    {
        public EHealthCardContext()
        {
        }

        public EHealthCardContext(DbContextOptions<EHealthCardContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<DiagnosesType> DiagnosesTypes { get; set; } = null!;
        public virtual DbSet<Diagnosis> Diagnoses { get; set; } = null!;
        public virtual DbSet<Hospital> Hospitals { get; set; } = null!;
        public virtual DbSet<Hospitalization> Hospitalizations { get; set; } = null!;
        public virtual DbSet<Insurance> Insurances { get; set; } = null!;
        public virtual DbSet<InsuranceComp> InsuranceComps { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Person> People { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseOracle("User Id=C##LOCAL;Password=oracle;Data Source=25.48.253.17:1521/xe;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("C##LOCAL")
                .UseCollation("USING_NLS_COMP");

            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.Zip);

                entity.ToTable("CITY");

                entity.Property(e => e.Zip)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ZIP")
                    .IsFixedLength();

                entity.Property(e => e.CityName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CITY_NAME");
            });

            modelBuilder.Entity<DiagnosesType>(entity =>
            {
                entity.HasKey(e => e.DiagnosisId);

                entity.ToTable("DIAGNOSES_TYPE");

                entity.Property(e => e.DiagnosisId)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("DIAGNOSIS_ID")
                    .IsFixedLength();

                entity.Property(e => e.DailyCosts)
                    .HasColumnType("NUMBER(15,2)")
                    .HasColumnName("DAILY_COSTS");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");
            });

            modelBuilder.Entity<Diagnosis>(entity =>
            {
                entity.HasKey(e => new { e.DateStart, e.HospitalName, e.PersonId, e.DiagnosisId });

                entity.ToTable("DIAGNOSES");

                entity.Property(e => e.DateStart)
                    .HasColumnType("DATE")
                    .HasColumnName("DATE_START");

                entity.Property(e => e.HospitalName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("HOSPITAL_NAME");

                entity.Property(e => e.PersonId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PERSON_ID")
                    .IsFixedLength();

                entity.Property(e => e.DiagnosisId)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("DIAGNOSIS_ID")
                    .IsFixedLength();

                entity.Property(e => e.Document)
                    .HasColumnType("LONG RAW")
                    .HasColumnName("DOCUMENT");

                entity.HasOne(d => d.DiagnosisNavigation)
                    .WithMany(p => p.Diagnoses)
                    .HasForeignKey(d => d.DiagnosisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RELATIONSHIPDIAGNOSESTYPE");

                entity.HasOne(d => d.Hospitalization)
                    .WithMany(p => p.Diagnoses)
                    .HasForeignKey(d => new { d.DateStart, d.HospitalName, d.PersonId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RELATIONSHIPDIAGNOSESHOSPITALIZATION");
            });

            modelBuilder.Entity<Hospital>(entity =>
            {
                entity.HasKey(e => e.HospitalName);

                entity.ToTable("HOSPITAL");

                entity.HasIndex(e => e.Zip, "IX_RELATIONSHIP1");

                entity.Property(e => e.HospitalName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("HOSPITAL_NAME");

                entity.Property(e => e.Capacity)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("CAPACITY");

                entity.Property(e => e.Zip)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ZIP")
                    .IsFixedLength();

                entity.HasOne(d => d.ZipNavigation)
                    .WithMany(p => p.Hospitals)
                    .HasForeignKey(d => d.Zip)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RELATIONSHIPCITYHOSPITAL");
            });

            modelBuilder.Entity<Hospitalization>(entity =>
            {
                entity.HasKey(e => new { e.DateStart, e.HospitalName, e.PersonId });

                entity.ToTable("HOSPITALIZATION");

                entity.Property(e => e.DateStart)
                    .HasColumnType("DATE")
                    .HasColumnName("DATE_START");

                entity.Property(e => e.HospitalName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("HOSPITAL_NAME");

                entity.Property(e => e.PersonId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PERSON_ID")
                    .IsFixedLength();

                entity.Property(e => e.DateEnd)
                    .HasColumnType("DATE")
                    .HasColumnName("DATE_END");

                entity.HasOne(d => d.HospitalNameNavigation)
                    .WithMany(p => p.Hospitalizations)
                    .HasForeignKey(d => d.HospitalName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RELATIONSHIPHOSPITALHOPSITALIZATION");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Hospitalizations)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RELATIONSHIPPERSONHOSPITALIZATION");
            });

            modelBuilder.Entity<Insurance>(entity =>
            {
                entity.HasKey(e => new { e.PersonId, e.CompId, e.DateStart });

                entity.ToTable("INSURANCE");

                entity.Property(e => e.PersonId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PERSON_ID")
                    .IsFixedLength();

                entity.Property(e => e.CompId)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("COMP_ID")
                    .IsFixedLength();

                entity.Property(e => e.DateStart)
                    .HasColumnType("DATE")
                    .HasColumnName("DATE_START");

                entity.Property(e => e.DateEnd)
                    .HasColumnType("DATE")
                    .HasColumnName("DATE_END");

                entity.HasOne(d => d.Comp)
                    .WithMany(p => p.Insurances)
                    .HasForeignKey(d => d.CompId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RELATIONSHIPINSURANCECOMP");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Insurances)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RELATIONSHIPPERSONINSURANCE");
            });

            modelBuilder.Entity<InsuranceComp>(entity =>
            {
                entity.HasKey(e => e.CompId);

                entity.ToTable("INSURANCE_COMP");

                entity.Property(e => e.CompId)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("COMP_ID")
                    .IsFixedLength();

                entity.Property(e => e.CompName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("COMP_NAME");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => new { e.HospitalName, e.CompId, e.PaymentId });

                entity.ToTable("PAYMENT");

                entity.Property(e => e.HospitalName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("HOSPITAL_NAME");

                entity.Property(e => e.CompId)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("COMP_ID")
                    .IsFixedLength();

                entity.Property(e => e.PaymentId)
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PAYMENT_ID");

                entity.Property(e => e.Details)
                    .HasColumnType("XMLTYPE")
                    .HasColumnName("DETAILS");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("DATE")
                    .HasColumnName("PAYMENT_DATE");

                entity.Property(e => e.PaymentPeriod)
                    .HasColumnType("DATE")
                    .HasColumnName("PAYMENT_PERIOD");

                entity.HasOne(d => d.Comp)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.CompId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RELATIONSHIPPAYMENTINSURANCE");

                entity.HasOne(d => d.HospitalNameNavigation)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.HospitalName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RELATIONSHIPPAYMENTHOSPITAL");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("PERSON");

                entity.HasIndex(e => e.Zip, "IX_RELATIONSHIP2");

                entity.Property(e => e.PersonId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PERSON_ID")
                    .IsFixedLength();

                entity.Property(e => e.Zip)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ZIP")
                    .IsFixedLength();

                entity.HasOne(d => d.ZipNavigation)
                    .WithMany(p => p.People)
                    .HasForeignKey(d => d.Zip)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RELATIONSHIPCITYPERSON");
            });

            modelBuilder.HasSequence("SEQ_PAYMENT_ID");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
