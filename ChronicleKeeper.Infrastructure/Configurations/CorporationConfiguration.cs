using ChronicleKeeper.Core.Entities.Social.Economy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class CorporationConfiguration : LoreEntityConfiguration<Corporation>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Corporation> builder)
        {
            builder.Property(c => c.IndustrySector).HasMaxLength(100);

            // System FKs — Restrict: deleting a system in use is a friendly app error
            builder.HasOne(c => c.Industry)
                .WithMany()
                .HasForeignKey(c => c.IndustryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.TaxationSystem)
                .WithMany(t => t.TaxedCorporations)
                .HasForeignKey(c => c.TaxationSystemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.BankingSystem)
                .WithMany(b => b.FinancialInstitutions)
                .HasForeignKey(c => c.BankingSystemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Self-ref subsidiary hierarchy — Restrict: deleting a corporation with subsidiaries is a friendly app error
            builder.HasOne(c => c.ParentCorporation)
                .WithMany(c => c.Subsidiaries)
                .HasForeignKey(c => c.ParentCorporationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => c.ParentCorporationId);

            builder.HasOne(c => c.History)
                .WithMany()
                .HasForeignKey(c => c.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Compositional children — die with the corporation
            builder.HasMany(c => c.Leadership)
                .WithOne(l => l.Corporation)
                .HasForeignKey(l => l.CorporationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Pointer children — an apprenticeship survives its corporation's deletion
            builder.HasMany(c => c.Apprenticeships)
                .WithOne(a => a.Corporation)
                .HasForeignKey(a => a.CorporationId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.ToTable(t =>
                t.HasCheckConstraint("CK_Corporations_Parent_NotSelf", "[ParentCorporationId] <> [Id]"));
        }
    }

    public class CorporateLeadershipConfiguration : LoreEntityConfiguration<CorporateLeadership>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<CorporateLeadership> builder)
        {
            builder.Property(l => l.Position).HasMaxLength(100);

            // Pointer-only references — SetNull
            builder.HasOne(l => l.Profession)
                .WithMany()
                .HasForeignKey(l => l.ProfessionId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(l => l.Character)
                .WithMany()
                .HasForeignKey(l => l.CharacterId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(l => l.History)
                .WithMany()
                .HasForeignKey(l => l.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
