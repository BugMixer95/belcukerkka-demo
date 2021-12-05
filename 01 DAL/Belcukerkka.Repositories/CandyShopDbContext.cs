using Belcukerkka.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Belcukerkka.Repositories
{
    public class CandyShopDbContext : DbContext
    {
        public CandyShopDbContext(DbContextOptions<CandyShopDbContext> options)
            : base(options)
        {
        }

        public DbSet<Box> Boxes { get; set; }
        public DbSet<BoxPackage> BoxPackages { get; set; }
        public DbSet<BoxParent> BoxParents { get; set; }
        public DbSet<Candy> Candies { get; set; }
        public DbSet<Composition> Compositions { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CandyInComposition> CandiesInCompositions { get; set; }
        public DbSet<CatalogItem> CatalogItems { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Box>(ModelConfigurations.BoxConfigure);
            modelBuilder.Entity<Candy>(ModelConfigurations.CandyConfigure);
            modelBuilder.Entity<Composition>(ModelConfigurations.CompositionConfigure);
            modelBuilder.Entity<Order>(ModelConfigurations.OrderConfigure);
            modelBuilder.Entity<OrderItem>(ModelConfigurations.OrderItemConfigure);
            modelBuilder.Entity<Customer>(ModelConfigurations.CustomerConfigure);
            modelBuilder.Entity<BoxParent>(ModelConfigurations.BoxParentConfigure);
            modelBuilder.Entity<User>(ModelConfigurations.UserConfigure);

            modelBuilder.Seed();
            modelBuilder.SeedUsers();

            modelBuilder.Entity<CatalogItem>(ModelConfigurations.CatalogItemConfigure);
        }
    }
}
