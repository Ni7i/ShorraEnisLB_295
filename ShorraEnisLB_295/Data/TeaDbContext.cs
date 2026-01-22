using Microsoft.EntityFrameworkCore;
using ShorraEnisLB_295.Models;

namespace ShorraEnisLB_295.Data;

public class TeaDbContext : DbContext
{
    public TeaDbContext(DbContextOptions<TeaDbContext> options) : base(options)
    {
    }

    public DbSet<Collection> Collections { get; set; }
    public DbSet<TeaRecipe> TeaRecipes { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Collection Konfiguration
        modelBuilder.Entity<Collection>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
        });

        // TeaRecipe Konfiguration
        modelBuilder.Entity<TeaRecipe>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.TeaName).IsRequired().HasMaxLength(100);
            
            entity.HasOne(t => t.Collection)
                .WithMany(c => c.TeaRecipes)
                .HasForeignKey(t => t.CollectionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // User Konfiguration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
            entity.Property(u => u.Password).IsRequired().HasMaxLength(100);
            entity.HasIndex(u => u.Username).IsUnique();
        });

        // Seed Test-User (admin/admin123)
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            Username = "admin",
            Password = "admin123"
        });
    }
}