using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Context;

public partial class TripsContext : DbContext
{
    private readonly IConfiguration configuration;
    
    public TripsContext(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public TripsContext(DbContextOptions<DbContext> options, IConfiguration configuration)
        : base(options)
    {
        this.configuration = configuration;
    }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSqlServer(this.configuration["ConnectionStrings:DefaultConnection"])
            .LogTo(Console.WriteLine, LogLevel.Information);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.IdCountry).HasName("PK__countrie__7E8CD055C49B922B");

            entity.ToTable("Country");

            entity.Property(e => e.Name)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("Name");
            
            entity.HasMany(d => d.Trip)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__departmen__locat__48CFD27E");

        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("PK__departme__C2232422E039C091");

            entity.ToTable("Client");

            entity.Property(e => e.FirstName)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("FirstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("LastName");
            entity.Property(e => e.Email)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("Email");
            entity.Property(e => e.Telephone)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("Telephone");
            entity.Property(e => e.Pesel)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("Pesel");
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity
                .HasKey(e => e.IdTrip)
                .HasName("PK__dependen__F25E28CE0C74D7FC");

            entity.ToTable("Trip");

            entity.Property(e => e.Name)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("Name");
            entity.Property(e => e.Description)
                .HasMaxLength(220)
                .IsUnicode(false)
                .HasColumnName("Description");
            entity.Property(e => e.DateFrom)
                .HasColumnType("datetime")
                .HasColumnName("DateFrom");
            entity.Property(e => e.DateTo)
                .HasColumnType("datetime")
                .HasColumnName("DateTo");
            entity.Property(e => e.MaxPeople)
                .HasColumnType("int")
                .HasColumnName("MaxPeople");

            entity.HasOne(d => d.Employee).WithMany(p => p.Dependents)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__dependent__emplo__5441852A");
        });



        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
