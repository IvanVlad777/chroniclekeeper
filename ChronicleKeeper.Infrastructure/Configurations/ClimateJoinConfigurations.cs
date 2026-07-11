using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.Geography.Climate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    // Join tablice smiju kaskadirati s obje strane: entiteti nemaju
    // zajedničkog kaskadirajućeg pretka (WorldId je Restrict).

    public class ClimateZoneDetailConfiguration : IEntityTypeConfiguration<ClimateZoneDetail>
    {
        public void Configure(EntityTypeBuilder<ClimateZoneDetail> builder)
        {
            builder.HasKey(zd => new { zd.ClimateZoneId, zd.ClimateDetailId });

            builder.HasOne(zd => zd.ClimateZone)
                .WithMany(z => z.Climates)
                .HasForeignKey(zd => zd.ClimateZoneId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(zd => zd.ClimateDetail)
                .WithMany(d => d.ClimateZones)
                .HasForeignKey(zd => zd.ClimateDetailId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(zd => zd.ClimateDetailId);
        }
    }

    public class ClimateZoneSeasonConfiguration : IEntityTypeConfiguration<ClimateZoneSeason>
    {
        public void Configure(EntityTypeBuilder<ClimateZoneSeason> builder)
        {
            builder.HasKey(zs => new { zs.ClimateZoneId, zs.SeasonId });

            builder.HasOne(zs => zs.ClimateZone)
                .WithMany(z => z.Seasons)
                .HasForeignKey(zs => zs.ClimateZoneId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(zs => zs.Season)
                .WithMany(s => s.ClimateZones)
                .HasForeignKey(zs => zs.SeasonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(zs => zs.SeasonId);
        }
    }

    public class LocationClimateZoneConfiguration : IEntityTypeConfiguration<LocationClimateZone>
    {
        public void Configure(EntityTypeBuilder<LocationClimateZone> builder)
        {
            builder.HasKey(lz => new { lz.LocationId, lz.ClimateZoneId });

            // Location nema reverznu kolekciju (isti obrazac kao Nation u CultureNationConfiguration
            // — target strana M:N veze ne dobiva nav-property).
            builder.HasOne(lz => lz.Location)
                .WithMany()
                .HasForeignKey(lz => lz.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(lz => lz.ClimateZone)
                .WithMany(z => z.Locations)
                .HasForeignKey(lz => lz.ClimateZoneId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(lz => lz.ClimateZoneId);
        }
    }
}
