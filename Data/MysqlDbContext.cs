using Eventual.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventual.Data;

public class MysqlDbContext : DbContext
{
    public MysqlDbContext(DbContextOptions<MysqlDbContext> options) :base(options){}
    public DbSet<Events> events { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Events>(entity =>
        {
            entity.ToTable("events");
            entity.HasKey(e => e.id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Description).IsRequired().HasColumnType("TEXT");
            entity.Property(e => e.Location).IsRequired().HasMaxLength(300);
            entity.Property(e => e.Poster).HasMaxLength(500).IsRequired(false);
            entity.Property(e => e.EventDate).IsRequired().HasColumnType("date");
        });
    }
}