using ChronicleKeeper.Core.Entities.Geography.Climate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class WeatherPatternConfiguration : LoreEntityConfiguration<WeatherPattern>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<WeatherPattern> builder)
        {
            builder.Property(w => w.PatternType)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(w => w.Frequency)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(w => w.Effects)
                .HasConversion<string>()
                .HasMaxLength(30);

            // ClimateZoneId (Cascade) je konfiguriran s ClimateZone strane u ClimateZoneConfiguration.

            builder.HasOne(w => w.History)
                .WithMany()
                .HasForeignKey(w => w.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
