using ChronicleKeeper.Core.Entities.Social.Cultures;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class LanguageConfiguration : LoreEntityConfiguration<Language>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Language> builder)
        {
            builder.Property(l => l.WritingSystem)
                .HasMaxLength(100);

            builder.Property(l => l.Dialects)
                .HasMaxLength(500);
        }
    }
}
