using Microsoft.EntityFrameworkCore;
using StudyCards.Domain.Entities;

namespace StudyCards.Infrastructure.Database.Context;

public class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions options) : base(options) { }

    public DbSet<Deck> Deck { get; set; }
    public DbSet<Card> Card { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultContainer("Card");

        modelBuilder.Entity<Card>()
            .HasNoDiscriminator()
            .HasPartitionKey(x => x.DeckId)
            .HasKey(x => x.Id);

        modelBuilder.Entity<Deck>()
            .ToContainer("Deck")
            .HasNoDiscriminator()
            .HasPartitionKey(x => x.UserEmail)
            .HasKey(x => x.Id);
    }
}
