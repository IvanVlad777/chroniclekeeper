using ChronicleKeeper.Core.Entities.Social.Cultures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    // ---------------- Custom (optional Culture) ----------------
    public class CustomConfiguration : LoreEntityConfiguration<Custom>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Custom> builder)
        {
            builder.HasOne(c => c.Culture)
                .WithMany(cu => cu.Customs)
                .HasForeignKey(c => c.CultureId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(c => c.History)
                .WithMany()
                .HasForeignKey(c => c.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    // ---------------- ArtForm ----------------
    public class ArtFormConfiguration : LoreEntityConfiguration<ArtForm>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ArtForm> builder)
        {
            builder.Property(a => a.Type).HasMaxLength(100);
            builder.Property(a => a.NotableArtists).HasMaxLength(500);
            builder.Property(a => a.HistoricalInfluences).HasMaxLength(500);

            builder.HasOne(a => a.Culture)
                .WithMany(cu => cu.ArtForms)
                .HasForeignKey(a => a.CultureId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.History)
                .WithMany()
                .HasForeignKey(a => a.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    // ---------------- Cuisine ----------------
    public class CuisineConfiguration : LoreEntityConfiguration<Cuisine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Cuisine> builder)
        {
            builder.Property(c => c.MainIngredients).HasMaxLength(500);
            builder.Property(c => c.CookingMethods).HasMaxLength(500);
            builder.Property(c => c.TypicalDishes).HasMaxLength(500);

            builder.HasOne(c => c.Culture)
                .WithMany(cu => cu.Cuisines)
                .HasForeignKey(c => c.CultureId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.History)
                .WithMany()
                .HasForeignKey(c => c.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    // ---------------- Tradition (Culture required + optional Religion) ----------------
    public class TraditionConfiguration : LoreEntityConfiguration<Tradition>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Tradition> builder)
        {
            builder.Property(t => t.Practice).HasMaxLength(500);

            builder.HasOne(t => t.Culture)
                .WithMany(cu => cu.Traditions)
                .HasForeignKey(t => t.CultureId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.Religion)
                .WithMany(r => r.Traditions)
                .HasForeignKey(t => t.ReligionId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(t => t.History)
                .WithMany()
                .HasForeignKey(t => t.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    // ---------------- CulturalFestival (Culture required + optional Location) ----------------
    public class CulturalFestivalConfiguration : LoreEntityConfiguration<CulturalFestival>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<CulturalFestival> builder)
        {
            builder.Property(f => f.StartDate).HasMaxLength(100);
            builder.Property(f => f.Activities).HasMaxLength(500);

            builder.HasOne(f => f.Culture)
                .WithMany(cu => cu.Festivals)
                .HasForeignKey(f => f.CultureId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional location — SetNull, no reverse nav (Location plays the target role, asymmetric).
            builder.HasOne(f => f.Location)
                .WithMany()
                .HasForeignKey(f => f.LocationId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(f => f.History)
                .WithMany()
                .HasForeignKey(f => f.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    // ---------------- Myth (Culture required + optional Religion; Deity deferred) ----------------
    public class MythConfiguration : LoreEntityConfiguration<Myth>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Myth> builder)
        {
            builder.Property(m => m.CreationStory).HasMaxLength(2000);
            builder.Property(m => m.Symbolism).HasMaxLength(1000);

            builder.HasOne(m => m.Culture)
                .WithMany(cu => cu.Myths)
                .HasForeignKey(m => m.CultureId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Religion)
                .WithMany(r => r.Myths)
                .HasForeignKey(m => m.ReligionId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(m => m.History)
                .WithMany()
                .HasForeignKey(m => m.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    // ---------------- Clothing ----------------
    public class ClothingConfiguration : LoreEntityConfiguration<Clothing>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Clothing> builder)
        {
            builder.Property(c => c.ClothingType).HasMaxLength(100);
            builder.Property(c => c.Materials).HasMaxLength(500);
            builder.Property(c => c.DesignFeatures).HasMaxLength(500);
            builder.Property(c => c.SpecialProperties).HasMaxLength(500);

            builder.HasOne(c => c.Culture)
                .WithMany(cu => cu.Clothing)
                .HasForeignKey(c => c.CultureId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.History)
                .WithMany()
                .HasForeignKey(c => c.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    // ---------------- ArchitectureStyle (+ M:N Location) ----------------
    public class ArchitectureStyleConfiguration : LoreEntityConfiguration<ArchitectureStyle>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ArchitectureStyle> builder)
        {
            builder.Property(a => a.MaterialsUsed).HasMaxLength(500);
            builder.Property(a => a.DesignFeatures).HasMaxLength(500);

            builder.HasOne(a => a.Culture)
                .WithMany(cu => cu.ArchitectureStyles)
                .HasForeignKey(a => a.CultureId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.History)
                .WithMany()
                .HasForeignKey(a => a.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    // ---------------- Folklore (+ M:N TimelineEvent, M:N SapientSpecies) ----------------
    public class FolkloreConfiguration : LoreEntityConfiguration<Folklore>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Folklore> builder)
        {
            builder.Property(f => f.Story).HasMaxLength(2000);
            builder.Property(f => f.Moral).HasMaxLength(500);

            builder.HasOne(f => f.Culture)
                .WithMany(cu => cu.Folktales)
                .HasForeignKey(f => f.CultureId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.History)
                .WithMany()
                .HasForeignKey(f => f.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    // ---------------- CulturalInstitution (optional Culture + optional City) ----------------
    public class CulturalInstitutionConfiguration : LoreEntityConfiguration<CulturalInstitution>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<CulturalInstitution> builder)
        {
            builder.Property(i => i.InstitutionType).HasMaxLength(100);

            builder.HasOne(i => i.Culture)
                .WithMany(cu => cu.CulturalInstitutions)
                .HasForeignKey(i => i.CultureId)
                .OnDelete(DeleteBehavior.SetNull);

            // Optional city — SetNull, no reverse nav (City plays the target role, asymmetric).
            builder.HasOne(i => i.City)
                .WithMany()
                .HasForeignKey(i => i.CityId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(i => i.History)
                .WithMany()
                .HasForeignKey(i => i.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    // ---- Join tables (composite PK, Cascade both sides; target side asymmetric — no reverse nav) ----

    public class ArchitectureStyleLocationConfiguration : IEntityTypeConfiguration<ArchitectureStyleLocation>
    {
        public void Configure(EntityTypeBuilder<ArchitectureStyleLocation> builder)
        {
            builder.HasKey(al => new { al.ArchitectureStyleId, al.LocationId });

            builder.HasOne(al => al.ArchitectureStyle)
                .WithMany(a => a.TypicalLocations)
                .HasForeignKey(al => al.ArchitectureStyleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(al => al.Location)
                .WithMany()
                .HasForeignKey(al => al.LocationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class FolkloreTimelineEventConfiguration : IEntityTypeConfiguration<FolkloreTimelineEvent>
    {
        public void Configure(EntityTypeBuilder<FolkloreTimelineEvent> builder)
        {
            builder.HasKey(fe => new { fe.FolkloreId, fe.TimelineEventId });

            builder.HasOne(fe => fe.Folklore)
                .WithMany(f => f.RelatedEvents)
                .HasForeignKey(fe => fe.FolkloreId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(fe => fe.TimelineEvent)
                .WithMany()
                .HasForeignKey(fe => fe.TimelineEventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class FolkloreSapientSpeciesConfiguration : IEntityTypeConfiguration<FolkloreSapientSpecies>
    {
        public void Configure(EntityTypeBuilder<FolkloreSapientSpecies> builder)
        {
            builder.HasKey(fs => new { fs.FolkloreId, fs.SapientSpeciesId });

            builder.HasOne(fs => fs.Folklore)
                .WithMany(f => f.OriginatedFromSpecies)
                .HasForeignKey(fs => fs.FolkloreId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(fs => fs.SapientSpecies)
                .WithMany(s => s.Folklore)
                .HasForeignKey(fs => fs.SapientSpeciesId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
