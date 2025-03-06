using Microsoft.EntityFrameworkCore;
using StudyCards.Domain.Entities;

namespace StudyCards.Infrastructure.Database.Context;

public class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions options) : base(options) { }

    public DbSet<Deck> Deck { get; set; }
    public DbSet<Card> Cards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultContainer("Cards");

        modelBuilder.Entity<Card>()
            .HasNoDiscriminator()
            .HasPartitionKey(x => x.DeckId)
            .HasKey(x => x.CardId);

        modelBuilder.Entity<Deck>()
            .ToContainer("Deck")
            .HasNoDiscriminator()
            .HasPartitionKey(x => x.UserEmail)
            .HasKey(x => x.DeckId);
    }
}
