using ChronicleKeeper.Core.Entities.Social.Cultures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class CultureConfiguration : LoreEntityConfiguration<Culture>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Culture> builder)
        {
            builder.Property(c => c.CommonValues)
                .HasMaxLength(500);

            builder.Property(c => c.SocialStructure)
                .HasMaxLength(500);

            builder.Property(c => c.ConflictResolution)
                .HasMaxLength(500);

            builder.Property(c => c.XenophobiaLevel)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(c => c.TechnologicalLevel)
                .HasConversion<string>()
                .HasMaxLength(30);

            // Language — Restrict: brisanje jezika u upotrebi je friendly app greška
            builder.HasOne(c => c.Language)
                .WithMany(l => l.Cultures)
                .HasForeignKey(c => c.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);

            // Religion — Restrict: brisanje religije u upotrebi je friendly app greška
            // (nema back-collection na Religion — Religion.Followers su samo likovi)
            builder.HasOne(c => c.Religion)
                .WithMany()
                .HasForeignKey(c => c.ReligionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
