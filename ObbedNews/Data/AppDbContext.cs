using Microsoft.EntityFrameworkCore;
using ObbedNews.Models;
using ObbedNews.Models.Middle;
using ObbedNews.Models.Payment;

namespace ObbedNews.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    // accounts
    public DbSet<Account> Accounts { get; set; } = null!;

    // users
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserNewsFavorite> UserNewsFavorites { get; set; } = null!;

    // news
    public DbSet<ParentCategory> ParentCategories { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<News> News { get; set; } = null!;
    public DbSet<NewsVideoSection> NewsVideoSections { get; set; } = null!;
    public DbSet<NewsVideo> NewsVideos { get; set; } = null!;

    // guilties
    public DbSet<UserNewsLike> UserNewsLikes { get; set; } = null!;

    // news comments
    public DbSet<Comment> Comments { get; set; } = null!;

    // news tags
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