using ChronicleKeeper.Core.Entities.Characters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class CharacterConfiguration : LoreEntityConfiguration<Character>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Character> builder)
        {
            // Obavezna polja
            builder.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            // Opcionalna polja sa maksimalnim dužinama
            builder.Property(c => c.LastName)
                .HasMaxLength(50);

            builder.Property(c => c.Nickname)
                .HasMaxLength(50);

            builder.Property(c => c.Title)
                .HasMaxLength(100);

            // Fictional in-world dates stored as free strings (stopgap until custom calendars)
            builder.Property(c => c.BirthDate)
                .HasMaxLength(100);

            builder.Property(c => c.DeathDate)
                .HasMaxLength(100);

            builder.Property(c => c.HairColor)
                .HasMaxLength(50);

            builder.Property(c => c.EyeColor)
                .HasMaxLength(50);

            builder.Property(c => c.SpecialPhysicalFeatures)
                .HasMaxLength(500);

            // Indeksi za brže pretraživanje (bazna klasa dodaje (WorldId, Name))
            builder.HasIndex(c => c.FirstName);
            builder.HasIndex(c => c.LastName);

            // Self-referencing family relationships — Restrict: SQL Server ne dopušta
            // cascade na self-ref; CharacterRepository.DeleteAsync prvo nulira FatherId/MotherId
            builder.HasOne(c => c.Father)
                .WithMany()
                .HasForeignKey(c => c.FatherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Mother)
                .WithMany()
                .HasForeignKey(c => c.MotherId)
                .OnDelete(DeleteBehavior.Restrict);

            // Species/Race — Restrict: brisanje vrste/rase u upotrebi je friendly app greška
            builder.HasOne(c => c.SapientSpecies)
                .WithMany()
                .HasForeignKey(c => c.SapientSpeciesId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Race)
                .WithMany()
                .HasForeignKey(c => c.RaceId)
                .OnDelete(DeleteBehavior.Restrict);

            // SocialClass — Restrict: brisanje društvenog sloja u upotrebi je friendly app greška
            builder.HasOne(c => c.SocialClass)
                .WithMany(sc => sc.Members)
                .HasForeignKey(c => c.SocialClassId)
                .OnDelete(DeleteBehavior.Restrict);

            // Nation — Restrict: brisanje nacije u upotrebi je friendly app greška
            builder.HasOne(c => c.Nation)
                .WithMany(n => n.Characters)
                .HasForeignKey(c => c.NationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Religion — Restrict: brisanje religije u upotrebi je friendly app greška
            builder.HasOne(c => c.Religion)
                .WithMany(r => r.Followers)
                .HasForeignKey(c => c.ReligionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Profession — Restrict: brisanje zanimanja u upotrebi je friendly app greška
            builder.HasOne(c => c.Profession)
                .WithMany(p => p.Characters)
                .HasForeignKey(c => c.ProfessionId)
                .OnDelete(DeleteBehavior.Restrict);

            // History — pointer-only (poput Faction.LeaderId), SetNull: brisanje History profila
            // samo odveže lika, ne blokira
            builder.HasOne(c => c.History)
                .WithMany()
                .HasForeignKey(c => c.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Owned value types — stored inline as columns on the Characters table
            // (Background_*, Personality_*). Required so existing rows get non-null defaults
            // and reads always reconstruct the object (never null).
            builder.OwnsOne(c => c.Background, b =>
            {
                b.Property(x => x.FamilyStatus).HasMaxLength(200);
                b.Property(x => x.Childhood).HasMaxLength(4000);
                b.Property(x => x.Upbringing).HasMaxLength(4000);
                b.Property(x => x.MigrationHistory).HasMaxLength(4000);
            });
            builder.Navigation(c => c.Background).IsRequired();

            builder.OwnsOne(c => c.Personality, b =>
            {
                b.Property(x => x.PersonalityTraits).HasMaxLength(2000);
                b.Property(x => x.Motivations).HasMaxLength(2000);
                b.Property(x => x.Virtues).HasMaxLength(2000);
                b.Property(x => x.Flaws).HasMaxLength(2000);
                b.Property(x => x.PsychologicalProfile).HasMaxLength(4000);
                b.Property(x => x.Fears).HasMaxLength(2000);
                b.Property(x => x.Ambitions).HasMaxLength(2000);
            });
            builder.Navigation(c => c.Personality).IsRequired();

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Characters_Father_NotSelf", "[FatherId] <> [Id]");
                t.HasCheckConstraint("CK_Characters_Mother_NotSelf", "[MotherId] <> [Id]");
            });
        }
    }
}
