using Belcukerkka.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Belcukerkka.Repositories
{
    internal class ModelConfigurations
    {
        public static void BoxConfigure(EntityTypeBuilder<Box> builder)
        {
            builder.ToTable("Boxes").HasKey(b => b.Id);
            builder.Property(b => b.Price).IsRequired();

            builder.HasOne(b => b.BoxParent)
                .WithMany(bp => bp.Boxes)
                .HasForeignKey(b => b.BoxParentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Composition)
                .WithMany(c => c.Boxes)
                .HasForeignKey(b => b.CompositionId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public static void BoxParentConfigure(EntityTypeBuilder<BoxParent> builder)
        {
            builder.ToTable("BoxParents").HasKey(bp => bp.Id);
            builder.Property(bp => bp.Name).IsRequired();

            builder.HasOne(bp => bp.BoxPackage)
                .WithMany(b => b.BoxParents)
                .HasForeignKey(bp => bp.BoxPackageId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public static void CandyConfigure(EntityTypeBuilder<Candy> builder)
        {
            builder.ToTable("Candies").HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired();
        }

        public static void CompositionConfigure(EntityTypeBuilder<Composition> builder)
        {
            builder.ToTable("Compositions").HasKey(c => c.Id);
            builder.Property(c => c.Weight).IsRequired();

            builder.HasIndex(c => new { c.WeightTypeId, c.Weight }).IsUnique();

            builder.HasOne(c => c.WeightType)
                .WithMany(wt => wt.Compositions)
                .HasForeignKey(c => c.WeightTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(composition => composition.Candies)
                .WithMany(candies => candies.Compositions)
                .UsingEntity<CandyInComposition>(
                    x => x.HasOne(cic => cic.Candy)
                            .WithMany(c => c.CandyInCompositions)
                            .HasForeignKey(cic => cic.CandyId),
                    x => x.HasOne(cic => cic.Composition)
                            .WithMany(c => c.CandiesInComposition)
                            .HasForeignKey(cic => cic.CompositionId),
                    x =>
                    {
                        x.Property(cic => cic.Amount).IsRequired();
                        x.HasKey(cic => new { cic.CandyId, cic.CompositionId });
                        x.ToTable("CandiesInCompositions");
                    }
                );
        }

        public static void OrderItemConfigure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems").HasKey(oi => oi.Id);

            builder.HasOne(oi => oi.Box)
                .WithMany(b => b.OrderItems)
                .HasForeignKey(oi => oi.BoxId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public static void OrderConfigure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders").HasKey(o => o.Id);
            builder.Property(o => o.Date).HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);
        }

        public static void CustomerConfigure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers").HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.Phone).IsRequired();
        }

        public static void CatalogItemConfigure(EntityTypeBuilder<CatalogItem> builder)
        {
            builder.ToView("bel_vw_CatalogItems")
                .HasIndex(ci => new { ci.Id, ci.Weight })
                .IsUnique();
        }

        public static void UserConfigure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users").HasKey(u => u.Id);
            builder.Property(u => u.UserName).IsRequired();
            builder.Property(u => u.Password).IsRequired();
            builder.HasIndex(u => u.UserName).IsUnique();
        }
    }
}
