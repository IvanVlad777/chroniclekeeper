using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class SapientSpeciesConfiguration : LoreEntityConfiguration<SapientSpecies>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SapientSpecies> builder)
        {
            builder.Property(s => s.CommonName)
                .HasMaxLength(100);

            builder.Property(s => s.ScientificName)
                .HasMaxLength(100);

            builder.Property(s => s.Lifespan)
                .HasMaxLength(100);
        }
    }

    public class RaceConfiguration : LoreEntityConfiguration<Race>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Race> builder)
        {
            builder.Property(r => r.AppearanceTraits)
                .HasMaxLength(500);

            builder.Property(r => r.GeneticFeatures)
                .HasMaxLength(500);

            builder.Property(r => r.Adaptations)
                .HasMaxLength(500);

            // Jedini DDL put do Race; Character.RaceId (Restrict) blokira brisanje rase u upotrebi
            builder.HasOne(r => r.SapientSpecies)
                .WithMany(s => s.Races)
                .HasForeignKey(r => r.SapientSpeciesId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
