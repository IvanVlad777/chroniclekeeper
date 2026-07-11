using ChronicleKeeper.Core.Entities.Geography.Climate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class ClimateZoneConfiguration : LoreEntityConfiguration<ClimateZone>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ClimateZone> builder)
        {
            builder.Property(z => z.ZoneType)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.HasOne(z => z.History)
                .WithMany()
                .HasForeignKey(z => z.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(z => z.TypicalWeatherPatterns)
                .WithOne(w => w.ClimateZone)
                .HasForeignKey(w => w.ClimateZoneId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
