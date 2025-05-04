using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
 
    public DbSet<PersonEntity> Persons { get; set; }
    public DbSet<SectorEntity> Sectors { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PersonEntity>()
            .ToTable("Persons")
            .HasQueryFilter(p => !p.Deleted)
            .HasOne(p => p.Sector)
            .WithMany()
            .HasForeignKey(p => p.SectorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<SectorEntity>()
            .ToTable("Sectors")
            .HasQueryFilter(s => !s.Deleted)
            .HasOne(s => s.Parent)
            .WithMany(s => s.Children)
            .HasForeignKey(s => s.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}