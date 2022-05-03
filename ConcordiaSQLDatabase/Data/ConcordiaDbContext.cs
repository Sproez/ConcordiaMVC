namespace ConcordiaSqlDatabase.Data;

using ConcordiaLib.Domain;
using Microsoft.EntityFrameworkCore;

public class ConcordiaDbContext : DbContext
{
    public DbSet<Person> People { get; set; } = null!;
    public DbSet<Card> Cards { get; set; } = null!;
    public DbSet<CardList> CardLists { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Assignment> Assignments { get; set; } = null!;

    public ConcordiaDbContext(DbContextOptions<ConcordiaDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Assignment>()
        .HasKey(a => new { a.CardId, a.PersonId });

    }
}