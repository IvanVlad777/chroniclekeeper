using ChronicleKeeper.Core.Entities.Geography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    // Country/City ↔ X many-to-many join configs. Composite PK, Cascade both FKs, index on the
    // target FK. Country/City is the owner (WithMany(nav)); target side is asymmetric (WithMany()).
    // All-Cascade is safe: join tables share no cascading ancestor (WorldId is always Restrict), and
    // each pair targets a distinct table so there is no multiple-cascade-path convergence.

    // ---- Country-owned ----

    public class CountryIndustryConfiguration : IEntityTypeConfiguration<CountryIndustry>
    {
        public void Configure(EntityTypeBuilder<CountryIndustry> builder)
        {
            builder.HasKey(x => new { x.CountryId, x.IndustryId });
            builder.HasOne(x => x.Country).WithMany(c => c.Industries).HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Industry).WithMany().HasForeignKey(x => x.IndustryId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.IndustryId);
        }
    }

    public class CountryCorporationConfiguration : IEntityTypeConfiguration<CountryCorporation>
    {
        public void Configure(EntityTypeBuilder<CountryCorporation> builder)
        {
            builder.HasKey(x => new { x.CountryId, x.CorporationId });
            builder.HasOne(x => x.Country).WithMany(c => c.Corporations).HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Corporation).WithMany().HasForeignKey(x => x.CorporationId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.CorporationId);
        }
    }

    public class CountryGuildConfiguration : IEntityTypeConfiguration<CountryGuild>
    {
        public void Configure(EntityTypeBuilder<CountryGuild> builder)
        {
            builder.HasKey(x => new { x.CountryId, x.GuildId });
            builder.HasOne(x => x.Country).WithMany(c => c.Guilds).HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Guild).WithMany().HasForeignKey(x => x.GuildId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.GuildId);
        }
    }

    public class CountryPoliticalPartyConfiguration : IEntityTypeConfiguration<CountryPoliticalParty>
    {
        public void Configure(EntityTypeBuilder<CountryPoliticalParty> builder)
        {
            builder.HasKey(x => new { x.CountryId, x.PoliticalPartyId });
            builder.HasOne(x => x.Country).WithMany(c => c.PoliticalParties).HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.PoliticalParty).WithMany().HasForeignKey(x => x.PoliticalPartyId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.PoliticalPartyId);
        }
    }

    public class CountryNationConfiguration : IEntityTypeConfiguration<CountryNation>
    {
        public void Configure(EntityTypeBuilder<CountryNation> builder)
        {
            builder.HasKey(x => new { x.CountryId, x.NationId });
            builder.HasOne(x => x.Country).WithMany(c => c.Nations).HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Nation).WithMany().HasForeignKey(x => x.NationId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.NationId);
        }
    }

    public class CountryFactionConfiguration : IEntityTypeConfiguration<CountryFaction>
    {
        public void Configure(EntityTypeBuilder<CountryFaction> builder)
        {
            builder.HasKey(x => new { x.CountryId, x.FactionId });
            builder.HasOne(x => x.Country).WithMany(c => c.Factions).HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Faction).WithMany().HasForeignKey(x => x.FactionId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.FactionId);
        }
    }

    public class CountryCultureConfiguration : IEntityTypeConfiguration<CountryCulture>
    {
        public void Configure(EntityTypeBuilder<CountryCulture> builder)
        {
            builder.HasKey(x => new { x.CountryId, x.CultureId });
            builder.HasOne(x => x.Country).WithMany(c => c.PredominantCultures).HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Culture).WithMany().HasForeignKey(x => x.CultureId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.CultureId);
        }
    }

    public class CountryReligionConfiguration : IEntityTypeConfiguration<CountryReligion>
    {
        public void Configure(EntityTypeBuilder<CountryReligion> builder)
        {
            builder.HasKey(x => new { x.CountryId, x.ReligionId });
            builder.HasOne(x => x.Country).WithMany(c => c.Religions).HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Religion).WithMany().HasForeignKey(x => x.ReligionId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.ReligionId);
        }
    }

    // ---- City-owned ----

    public class CityIndustryConfiguration : IEntityTypeConfiguration<CityIndustry>
    {
        public void Configure(EntityTypeBuilder<CityIndustry> builder)
        {
            builder.HasKey(x => new { x.CityId, x.IndustryId });
            builder.HasOne(x => x.City).WithMany(c => c.Industries).HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Industry).WithMany().HasForeignKey(x => x.IndustryId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.IndustryId);
        }
    }

    public class CityCorporationConfiguration : IEntityTypeConfiguration<CityCorporation>
    {
        public void Configure(EntityTypeBuilder<CityCorporation> builder)
        {
            builder.HasKey(x => new { x.CityId, x.CorporationId });
            builder.HasOne(x => x.City).WithMany(c => c.Corporations).HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Corporation).WithMany().HasForeignKey(x => x.CorporationId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.CorporationId);
        }
    }

    public class CityGuildConfiguration : IEntityTypeConfiguration<CityGuild>
    {
        public void Configure(EntityTypeBuilder<CityGuild> builder)
        {
            builder.HasKey(x => new { x.CityId, x.GuildId });
            builder.HasOne(x => x.City).WithMany(c => c.Guilds).HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Guild).WithMany().HasForeignKey(x => x.GuildId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.GuildId);
        }
    }

    public class CityCulturalInstitutionConfiguration : IEntityTypeConfiguration<CityCulturalInstitution>
    {
        public void Configure(EntityTypeBuilder<CityCulturalInstitution> builder)
        {
            builder.HasKey(x => new { x.CityId, x.CulturalInstitutionId });
            builder.HasOne(x => x.City).WithMany(c => c.CulturalInstitutions).HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.CulturalInstitution).WithMany().HasForeignKey(x => x.CulturalInstitutionId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.CulturalInstitutionId);
        }
    }

    public class CityPoliticalPartyConfiguration : IEntityTypeConfiguration<CityPoliticalParty>
    {
        public void Configure(EntityTypeBuilder<CityPoliticalParty> builder)
        {
            builder.HasKey(x => new { x.CityId, x.PoliticalPartyId });
            builder.HasOne(x => x.City).WithMany(c => c.PoliticalParties).HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.PoliticalParty).WithMany().HasForeignKey(x => x.PoliticalPartyId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.PoliticalPartyId);
        }
    }

    public class CityCultureConfiguration : IEntityTypeConfiguration<CityCulture>
    {
        public void Configure(EntityTypeBuilder<CityCulture> builder)
        {
            builder.HasKey(x => new { x.CityId, x.CultureId });
            builder.HasOne(x => x.City).WithMany(c => c.PredominantCultures).HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Culture).WithMany().HasForeignKey(x => x.CultureId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.CultureId);
        }
    }

    public class CityNationConfiguration : IEntityTypeConfiguration<CityNation>
    {
        public void Configure(EntityTypeBuilder<CityNation> builder)
        {
            builder.HasKey(x => new { x.CityId, x.NationId });
            builder.HasOne(x => x.City).WithMany(c => c.Nations).HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Nation).WithMany().HasForeignKey(x => x.NationId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.NationId);
        }
    }

    public class CityReligionConfiguration : IEntityTypeConfiguration<CityReligion>
    {
        public void Configure(EntityTypeBuilder<CityReligion> builder)
        {
            builder.HasKey(x => new { x.CityId, x.ReligionId });
            builder.HasOne(x => x.City).WithMany(c => c.Religions).HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Religion).WithMany().HasForeignKey(x => x.ReligionId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.ReligionId);
        }
    }
}
