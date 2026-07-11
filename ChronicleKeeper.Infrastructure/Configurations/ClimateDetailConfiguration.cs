using ChronicleKeeper.Core.Entities.Geography.Climate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class ClimateDetailConfiguration : LoreEntityConfiguration<ClimateDetail>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ClimateDetail> builder)
        {
            builder.Property(d => d.WindDirection)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(d => d.NotableWeatherPhenomena)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.HasOne(d => d.History)
                .WithMany()
                .HasForeignKey(d => d.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
