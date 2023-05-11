using Microsoft.EntityFrameworkCore;
using Obbed.Models;
using Obbed.Models.Middle;
using Obbed.Models.Payment;
using Obbed.Models.Words;

namespace Obbed.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    // accounts
    public DbSet<Account> Accounts { get; set; } = null!;

    // users
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserWordFavorite> UserWordsFavorites { get; set; } = null!;

    // words
    public DbSet<ParentCategory> ParentCategories { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Word> Words { get; set; } = null!;
    public DbSet<WordVideoSection> WordsVideoSections { get; set; } = null!;
    public DbSet<WordVideo> WordsVideos { get; set; } = null!;

    // guilties
    public DbSet<UserWordLike> UserWordsLikes { get; set; } = null!;

    // words comments
    public DbSet<WordComment> Comments { get; set; } = null!;

    // words tags
    public DbSet<Tag> Tags { get; set; } = null!;

    // price
    public DbSet<Currency> Currencies { get; set; } = null!;
    public DbSet<Price> Prices { get; set; } = null!;

    // orders
    public DbSet<PayPalOrder> PayPalOrders { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Account>()
            .HasOne<User>(a => a.User)
            .WithOne(u => u.Account)
            .HasForeignKey<User>(u => u.AccountId);
    }
}