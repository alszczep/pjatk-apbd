using api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Context;

public partial class ProjectContext : DbContext
{
    private readonly IConfiguration configuration;

    public ProjectContext(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public ProjectContext(DbContextOptions<ProjectContext> options, IConfiguration configuration)
        : base(options)
    {
        this.configuration = configuration;
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<ClientIndividual> ClientIndividuals { get; set; } = null!;
    public DbSet<ClientCompany> ClientCompanies { get; set; } = null!;
    public DbSet<Discount> Discounts { get; set; } = null!;
    public DbSet<SoftwareProduct> SoftwareProducts { get; set; } = null!;
    public DbSet<Contract> Contracts { get; set; } = null!;
    public DbSet<ContractPayment> ContractPayments { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer(this.configuration["ConnectionStrings:DefaultConnection"])
            .LogTo(Console.WriteLine, LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("apbdproject");

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Username);

            entity.ToTable("User");

            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).HasConversion<int>();
            entity.Property(e => e.RefreshToken).IsRequired();
            entity.Property(e => e.RefreshTokenExpiration).IsRequired();
            entity.Property(e => e.Salt).IsRequired();
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Client");

            entity.HasDiscriminator<ClientType>("Type")
                .HasValue<ClientIndividual>(ClientType.Individual)
                .HasValue<ClientCompany>(ClientType.Company);
        });

        modelBuilder.Entity<ClientIndividual>(entity =>
        {
            entity.HasBaseType<Client>();

            entity.Property(e => e.Pesel).IsRequired();
            entity.Property(e => e.FirstName).IsRequired();
            entity.Property(e => e.LastName).IsRequired();
            entity.Property(e => e.IsDeleted).IsRequired();

            entity.HasIndex(e => e.Pesel).IsUnique();
        });

        modelBuilder.Entity<ClientCompany>(entity =>
        {
            entity.HasBaseType<Client>();

            entity.Property(e => e.Krs).IsRequired();
            entity.Property(e => e.CompanyName).IsRequired();

            entity.HasIndex(e => e.Krs).IsUnique();
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Discount");

            entity.Property(e => e.DiscountPercentage).IsRequired();
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.StartDate).IsRequired();
            entity.Property(e => e.EndDate).IsRequired();

            entity.HasOne(e => e.SoftwareProduct)
                .WithMany(e => e.Discounts)
                .IsRequired();
        });

        modelBuilder.Entity<SoftwareProduct>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("SoftwareProduct");

            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Version).IsRequired();
            entity.Property(e => e.Category).IsRequired();
            entity.Property(e => e.UpfrontYearlyPriceInPln).IsRequired().HasColumnType("money");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Contract");

            entity.Property(e => e.StartDate).IsRequired();
            entity.Property(e => e.EndDate).IsRequired();
            entity.Property(e => e.PriceInPlnAfterDiscounts).IsRequired().HasColumnType("money");
            entity.Property(e => e.YearsOfExtendedSupport).IsRequired();
            entity.Property(e => e.IsSigned).IsRequired();
            entity.Property(e => e.SignedDate).IsRequired(false);

            entity.HasOne(e => e.Client)
                .WithMany(e => e.Contracts)
                .IsRequired();

            entity.HasOne(e => e.SoftwareProduct)
                .WithMany(e => e.Contracts)
                .IsRequired();
        });

        modelBuilder.Entity<ContractPayment>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("ContractPayment");

            entity.Property(e => e.PaymentAmountInPln).IsRequired().HasColumnType("money");

            entity.HasOne(e => e.Contract)
                .WithMany(e => e.Payments)
                .IsRequired();
        });

        this.OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
