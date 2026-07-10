using ChronicleKeeper.Core.Entities.Social.Politics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class LegalSystemConfiguration : LoreEntityConfiguration<LegalSystem>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<LegalSystem> builder)
        {
            builder.Property(l => l.Laws)
                .HasMaxLength(4000);

            builder.Property(l => l.JudicialIndependence)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(l => l.PunishmentMethods)
                .HasConversion<string>()
                .HasMaxLength(30);
        }
    }
}
