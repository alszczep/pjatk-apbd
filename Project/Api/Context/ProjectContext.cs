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

        this.OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
