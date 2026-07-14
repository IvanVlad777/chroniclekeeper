using ChronicleKeeper.Core.Entities.Social.Economy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    // Systems cluster: Currency <- BankingSystem <- EconomicSystem -> TaxationSystem.
    // All chain FKs are optional (a writer can sketch a system without prerequisites)
    // and Restrict — deleting a system in use is a friendly app error via delete guards.

    public class CurrencyConfiguration : LoreEntityConfiguration<Currency>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Currency> builder)
        {
            builder.Property(c => c.Symbol).HasMaxLength(10);
            builder.Property(c => c.BackingType).HasMaxLength(100);

            builder.HasOne(c => c.History)
                .WithMany()
                .HasForeignKey(c => c.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class BankingSystemConfiguration : LoreEntityConfiguration<BankingSystem>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<BankingSystem> builder)
        {
            builder.Property(b => b.SystemType).HasMaxLength(100);

            builder.HasOne(b => b.Currency)
                .WithMany()
                .HasForeignKey(b => b.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.History)
                .WithMany()
                .HasForeignKey(b => b.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class TaxationSystemConfiguration : LoreEntityConfiguration<TaxationSystem>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<TaxationSystem> builder)
        {
            builder.HasOne(t => t.History)
                .WithMany()
                .HasForeignKey(t => t.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class EconomicSystemConfiguration : LoreEntityConfiguration<EconomicSystem>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<EconomicSystem> builder)
        {
            builder.HasOne(e => e.TaxationSystem)
                .WithMany(t => t.UsedInEconomicSystems)
                .HasForeignKey(e => e.TaxationSystemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.BankingSystem)
                .WithMany(b => b.UsedInEconomicSystems)
                .HasForeignKey(e => e.BankingSystemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.History)
                .WithMany()
                .HasForeignKey(e => e.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
