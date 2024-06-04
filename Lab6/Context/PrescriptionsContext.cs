using System;
using System.Collections.Generic;
using Lab6.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab6.Context;

public partial class PrescriptionsContext : DbContext
{
    private readonly IConfiguration configuration;

    public PrescriptionsContext(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public PrescriptionsContext(DbContextOptions<PrescriptionsContext> options, IConfiguration configuration)
        : base(options)
    {
        this.configuration = configuration;
    }

    public virtual DbSet<Medicament> Medicaments { get; set; } = null!;
    public virtual DbSet<Doctor> Doctors { get; set; } = null!;
    public virtual DbSet<Patient> Patients { get; set; } = null!;
    public virtual DbSet<Prescription> Prescriptions { get; set; } = null!;
    public virtual DbSet<Prescription_Medicament> Prescription_Medicaments { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder
           .UseSqlServer(this.configuration["ConnectionStrings:DefaultConnection"])
           .LogTo(Console.WriteLine, LogLevel.Information);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("apbdlab6");

        modelBuilder.Entity<Medicament>(entity =>
        {
            entity.HasKey(e => e.IdMedicament).HasName("Medicament_pk");
            entity.Property(e => e.IdMedicament).ValueGeneratedNever();

            entity.ToTable("Medicament");

            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Type).HasMaxLength(100).IsRequired();

            entity.HasMany(e => e.Prescription_Medicaments)
                .WithOne(e => e.Medicament)
                .HasForeignKey(e => e.IdMedicament);

            entity.HasData(
                new Medicament { IdMedicament = 1, Name = "Med1", Description = "Desc1", Type = "Type1" },
                new Medicament { IdMedicament = 2, Name = "Med2", Description = "Desc2", Type = "Type2" }
            );
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.IdDoctor).HasName("Doctor_pk");
            entity.Property(e => e.IdDoctor).ValueGeneratedNever();

            entity.ToTable("Doctor");

            entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(100).IsRequired();

            entity.HasData(
                new Doctor { IdDoctor = 1, FirstName = "Doc1", LastName = "Doc1", Email = "Doc1" },
                new Doctor { IdDoctor = 2, FirstName = "Doc2", LastName = "Doc2", Email = "Doc2" }
            );
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.IdPatient).HasName("Patient_pk");
            entity.Property(e => e.IdPatient).ValueGeneratedNever();

            entity.ToTable("Patient");

            entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Birthdate).HasColumnType("date").IsRequired();

            entity.HasData(
                new Patient { IdPatient = 1, FirstName = "Pat1", LastName = "Pat1", Birthdate = new DateOnly(2002, 10, 23) },
                new Patient { IdPatient = 2, FirstName = "Pat2", LastName = "Pat2", Birthdate = new DateOnly(2001, 10, 23) }
            );
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.IdPrescription).HasName("Prescription_pk");

            entity.ToTable("Prescription");

            entity.Property(e => e.Date).HasColumnType("date").IsRequired();
            entity.Property(e => e.DateDue).HasColumnType("date").IsRequired();

            entity.HasOne(e => e.Doctor)
                .WithMany(e => e.Prescriptions)
                .HasForeignKey(e => e.IdDoctor)
                .HasConstraintName("Doctor_Of_Prescription");

            entity.HasOne(e => e.Patient)
                .WithMany(e => e.Prescriptions)
                .HasForeignKey(e => e.IdPatient)
                .HasConstraintName("Patient_Of_Prescription");

            entity.HasMany(e => e.Prescription_Medicaments)
                .WithOne(e => e.Prescription)
                .HasForeignKey(e => e.IdPrescription);
        });

        modelBuilder.Entity<Prescription_Medicament>(entity =>
        {
            entity.HasKey(e => new { e.IdPrescription, e.IdMedicament }).HasName("Prescription_Medicament_pk");
            entity.Property(e => e.IdPrescription).ValueGeneratedNever();
            entity.Property(e => e.IdMedicament).ValueGeneratedNever();

            entity.ToTable("Prescription_Medicament");

            entity.Property(e => e.Dose);
            entity.Property(e => e.Details).HasMaxLength(100).IsRequired();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
