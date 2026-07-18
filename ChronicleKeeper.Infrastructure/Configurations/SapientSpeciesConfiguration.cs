using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    // SapientSpecies is a Creature TPH subtype (discriminator "Sapient"), NOT its own table —
    // the LoreEntity/table setup comes from CreatureConfiguration. Here we only configure the
    // subtype-specific columns, renaming the two that would otherwise collide inside the shared
    // Creatures table (Lifespan: string vs Tree.Lifespan int; ScientificName: shared with Fungus).
    public class SapientSpeciesConfiguration : IEntityTypeConfiguration<SapientSpecies>
    {
        public void Configure(EntityTypeBuilder<SapientSpecies> builder)
        {
            builder.Property(s => s.CommonName)
                .HasMaxLength(100);

            builder.Property(s => s.ScientificName)
                .HasColumnName("Sapient_ScientificName")
                .HasMaxLength(100);

            builder.Property(s => s.Lifespan)
                .HasColumnName("Sapient_Lifespan")
                .HasMaxLength(100);

            builder.Property(s => s.SapientType)
                .HasConversion<string>()
                .HasMaxLength(30);
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
