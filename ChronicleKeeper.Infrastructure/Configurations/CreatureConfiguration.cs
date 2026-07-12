using ChronicleKeeper.Core.Entities.Geography.Creatures;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Animals;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Fungi;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Plants;
using ChronicleKeeper.Core.Entities.Geography.Ecosystems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class CreatureConfiguration : LoreEntityConfiguration<Creature>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Creature> builder)
        {
            builder.Property(c => c.Type)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(c => c.ArtificialOrigin)
                .HasConversion<string>()
                .HasMaxLength(30);

            // Self-ref taksonomska hijerarhija — Restrict: brisanje vrste s podvrstama je friendly app greška
            builder.HasOne(c => c.ParentCreature)
                .WithMany(c => c.Subspecies)
                .HasForeignKey(c => c.ParentCreatureId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => c.ParentCreatureId);

            // History — pointer-only, SetNull: brisanje History profila samo odveže stvorenje
            builder.HasOne(c => c.History)
                .WithMany()
                .HasForeignKey(c => c.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.ToTable(t =>
                t.HasCheckConstraint("CK_Creatures_Parent_NotSelf", "[ParentCreatureId] <> [Id]"));

            // TPH: shadow string discriminator, same pattern as LocationSubtype/ContentType/SchoolType.
            builder.HasDiscriminator<string>("CreatureSubtype")
                .HasValue<Animal>("Animal")
                .HasValue<Plant>("Plant")
                .HasValue<Tree>("Tree")
                .HasValue<Crop>("Crop")
                .HasValue<Fungus>("Fungus");
        }
    }

    public class CreatureCityConfiguration : IEntityTypeConfiguration<CreatureCity>
    {
        public void Configure(EntityTypeBuilder<CreatureCity> builder)
        {
            builder.HasKey(cc => new { cc.CreatureId, cc.CityId });

            builder.HasOne(cc => cc.Creature)
                .WithMany(c => c.CitiesItInhabits)
                .HasForeignKey(cc => cc.CreatureId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cc => cc.City)
                .WithMany()
                .HasForeignKey(cc => cc.CityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(cc => cc.CityId);
        }
    }

    public class CreatureEcosystemConfiguration : IEntityTypeConfiguration<CreatureEcosystem>
    {
        public void Configure(EntityTypeBuilder<CreatureEcosystem> builder)
        {
            builder.HasKey(ce => new { ce.CreatureId, ce.EcosystemId });

            builder.HasOne(ce => ce.Creature)
                .WithMany(c => c.Habitants)
                .HasForeignKey(ce => ce.CreatureId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ce => ce.Ecosystem)
                .WithMany()
                .HasForeignKey(ce => ce.EcosystemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(ce => ce.EcosystemId);
        }
    }

    public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
    {
        public void Configure(EntityTypeBuilder<Animal> builder)
        {
            builder.Property(a => a.Diet)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(a => a.Intelligence).HasMaxLength(500);
            builder.Property(a => a.SpecialAbilities).HasMaxLength(500);
        }
    }

    public class PlantConfiguration : IEntityTypeConfiguration<Plant>
    {
        public void Configure(EntityTypeBuilder<Plant> builder)
        {
            builder.Property(p => p.PlantType)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(p => p.Sunlight)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(p => p.PreferredSoil)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(p => p.TemperatureRange)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(p => p.Rarity)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(p => p.ScientificName).HasMaxLength(100);
            builder.Property(p => p.SpecialProperties).HasMaxLength(500);
            builder.Property(p => p.MythologicalSignificance).HasMaxLength(500);
        }
    }

    public class TreeConfiguration : IEntityTypeConfiguration<Tree>
    {
        public void Configure(EntityTypeBuilder<Tree> builder)
        {
            builder.Property(t => t.LeafType)
                .HasConversion<string>()
                .HasMaxLength(30);
        }
    }

    public class CropConfiguration : IEntityTypeConfiguration<Crop>
    {
        public void Configure(EntityTypeBuilder<Crop> builder)
        {
            builder.Property(c => c.CropType)
                .HasConversion<string>()
                .HasMaxLength(30);
        }
    }

    public class FungusConfiguration : IEntityTypeConfiguration<Fungus>
    {
        public void Configure(EntityTypeBuilder<Fungus> builder)
        {
            builder.Property(f => f.ScientificName).HasMaxLength(100);
            builder.Property(f => f.SpecialProperties).HasMaxLength(500);
            builder.Property(f => f.MythologicalSignificance).HasMaxLength(500);
        }
    }
}
