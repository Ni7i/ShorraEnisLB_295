namespace ShorraEnisLB_295.Data;

using Microsoft.EntityFrameworkCore;
using ShorraEnisLB_295.Models;

public class TeaDbContext : DbContext
{
    public TeaDbContext(DbContextOptions<TeaDbContext> options) : base(options) { }

    public DbSet<Collection> Collections { get; set; }
    public DbSet<TeaRecipe> TeaRecipes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Cascade Delete: Beim Löschen einer Collection werden alle TeaRecipes gelöscht
        modelBuilder.Entity<Collection>()
            .HasMany(c => c.TeaRecipes)
            .WithOne(r => r.Collection)
            .HasForeignKey(r => r.CollectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}