using ChronicleKeeper.Core.Entities.Geography.Ecosystems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    // Extra config owned by individual Ecosystem TPH subtypes — mirrors LocationSubtypeConfigurations.cs.

    public class EcosystemConfiguration : IEntityTypeConfiguration<Ecosystem>
    {
        public void Configure(EntityTypeBuilder<Ecosystem> builder)
        {
            builder.Property(e => e.UniqueFeatures).HasMaxLength(1000);
        }
    }

    public class RiverEcosystemConfiguration : IEntityTypeConfiguration<RiverEcosystem>
    {
        public void Configure(EntityTypeBuilder<RiverEcosystem> builder)
        {
            // Point references, not hierarchy — Restrict, not SetNull: two SetNull FKs from one
            // table to the same target table trigger SQL Server's multiple-cascade-path error
            // (same lesson as OwnershipHistory.PreviousOwnerId/NewOwnerId). Nulled explicitly in
            // WorldRepository.DeleteAsync before the Locations bulk delete; single-location delete
            // is blocked instead via ILocationRepository.IsReferencedAsRiverEndpointAsync.
            builder.HasOne(r => r.SourceLocation)
                .WithMany()
                .HasForeignKey(r => r.SourceLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.MouthLocation)
                .WithMany()
                .HasForeignKey(r => r.MouthLocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class DesertEcosystemConfiguration : IEntityTypeConfiguration<DesertEcosystem>
    {
        public void Configure(EntityTypeBuilder<DesertEcosystem> builder)
        {
            builder.Property(d => d.DesertKind)
                .HasConversion<string>()
                .HasMaxLength(30);
        }
    }

    public class ForestEcosystemConfiguration : IEntityTypeConfiguration<ForestEcosystem>
    {
        public void Configure(EntityTypeBuilder<ForestEcosystem> builder)
        {
            builder.Property(f => f.ForestKind)
                .HasConversion<string>()
                .HasMaxLength(30);
        }
    }

    public class CaveEcosystemConfiguration : IEntityTypeConfiguration<CaveEcosystem>
    {
        public void Configure(EntityTypeBuilder<CaveEcosystem> builder)
        {
            builder.Property(c => c.CaveKind)
                .HasConversion<string>()
                .HasMaxLength(30);
        }
    }

    public class GrasslandEcosystemConfiguration : IEntityTypeConfiguration<GrasslandEcosystem>
    {
        public void Configure(EntityTypeBuilder<GrasslandEcosystem> builder)
        {
            builder.Property(g => g.GrasslandKind)
                .HasConversion<string>()
                .HasMaxLength(30);
        }
    }
}
