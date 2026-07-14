using ChronicleKeeper.Core.Entities.Geography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    // Extra FKs owned by individual Location TPH subtypes — mirrors how ArticleConfiguration/
    // MovieConfiguration sit beside ContentConfiguration for the Content TPH root.

    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasOne(c => c.GovernmentSystem)
                .WithMany()
                .HasForeignKey(c => c.GovernmentSystemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.LegalSystem)
                .WithMany()
                .HasForeignKey(c => c.LegalSystemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.EducationSystem)
                .WithMany()
                .HasForeignKey(c => c.EducationSystemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.EconomicSystem)
                .WithMany()
                .HasForeignKey(c => c.EconomicSystemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasOne(c => c.GovernmentSystem)
                .WithMany()
                .HasForeignKey(c => c.GovernmentSystemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.LegalSystem)
                .WithMany()
                .HasForeignKey(c => c.LegalSystemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.EducationSystem)
                .WithMany()
                .HasForeignKey(c => c.EducationSystemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.EconomicSystem)
                .WithMany()
                .HasForeignKey(c => c.EconomicSystemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class RegionSapientSpeciesConfiguration : IEntityTypeConfiguration<RegionSapientSpecies>
    {
        public void Configure(EntityTypeBuilder<RegionSapientSpecies> builder)
        {
            builder.HasKey(rs => new { rs.RegionId, rs.SapientSpeciesId });

            builder.HasOne(rs => rs.Region)
                .WithMany(r => r.OriginOfSapientSpecies)
                .HasForeignKey(rs => rs.RegionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rs => rs.SapientSpecies)
                .WithMany(s => s.NativeRegions)
                .HasForeignKey(rs => rs.SapientSpeciesId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(rs => rs.SapientSpeciesId);
        }
    }
}
