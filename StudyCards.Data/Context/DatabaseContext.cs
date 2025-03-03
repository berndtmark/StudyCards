using Microsoft.EntityFrameworkCore;
using StudyCards.Domain.Entities;

namespace StudyCards.Infrastructure.Database.Context;

public class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions options) : base(options) { }

    public DbSet<Card> Cards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAutoscaleThroughput(1000);
        modelBuilder.HasDefaultContainer("Cards");

        modelBuilder.Entity<Card>()
            .HasNoDiscriminator()
            .HasPartitionKey(x => x.UserEmail)
            .HasKey(x => x.CardId);
    }
}
