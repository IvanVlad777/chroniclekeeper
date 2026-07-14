using ChronicleKeeper.Core.Entities.Social.Economy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    // Resources/trade cluster: Industry, ExtractionMethod <- NaturalResource <-> TradeRoute.

    public class IndustryConfiguration : LoreEntityConfiguration<Industry>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Industry> builder)
        {
            builder.Property(i => i.Sector).HasMaxLength(100);

            builder.HasOne(i => i.History)
                .WithMany()
                .HasForeignKey(i => i.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class ExtractionMethodConfiguration : LoreEntityConfiguration<ExtractionMethod>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ExtractionMethod> builder)
        {
            builder.Property(e => e.MethodType).HasMaxLength(100);

            builder.HasOne(e => e.History)
                .WithMany()
                .HasForeignKey(e => e.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class NaturalResourceConfiguration : LoreEntityConfiguration<NaturalResource>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<NaturalResource> builder)
        {
            builder.Property(n => n.ResourceType).HasMaxLength(100);

            // ExtractionMethod — Restrict: deleting a method in use is a friendly app error
            builder.HasOne(n => n.ExtractionMethod)
                .WithMany(e => e.ResourcesExtracted)
                .HasForeignKey(n => n.ExtractionMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(n => n.History)
                .WithMany()
                .HasForeignKey(n => n.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class TradeRouteConfiguration : LoreEntityConfiguration<TradeRoute>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<TradeRoute> builder)
        {
            builder.Property(t => t.RouteType).HasMaxLength(50);
            builder.Property(t => t.MainGoods).HasMaxLength(200);

            builder.HasOne(t => t.History)
                .WithMany()
                .HasForeignKey(t => t.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
