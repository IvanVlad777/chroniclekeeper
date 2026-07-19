using ChronicleKeeper.Core.Entities.Social.Cultures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    // Join tablice smiju kaskadirati s obje strane: entiteti nemaju
    // zajedničkog kaskadirajućeg pretka (WorldId je Restrict).

    public class CultureNationConfiguration : IEntityTypeConfiguration<CultureNation>
    {
        public void Configure(EntityTypeBuilder<CultureNation> builder)
        {
            builder.HasKey(cn => new { cn.CultureId, cn.NationId });

            builder.HasOne(cn => cn.Culture)
                .WithMany(c => c.Nations)
                .HasForeignKey(cn => cn.CultureId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cn => cn.Nation)
                .WithMany()
                .HasForeignKey(cn => cn.NationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(cn => cn.NationId);
        }
    }

    public class CultureSapientSpeciesConfiguration : IEntityTypeConfiguration<CultureSapientSpecies>
    {
        public void Configure(EntityTypeBuilder<CultureSapientSpecies> builder)
        {
            builder.HasKey(cs => new { cs.CultureId, cs.SapientSpeciesId });

            builder.HasOne(cs => cs.Culture)
                .WithMany(c => c.PracticedBySpecies)
                .HasForeignKey(cs => cs.CultureId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cs => cs.SapientSpecies)
                .WithMany(s => s.Cultures)
                .HasForeignKey(cs => cs.SapientSpeciesId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(cs => cs.SapientSpeciesId);
        }
    }

    public class CultureSocialClassConfiguration : IEntityTypeConfiguration<CultureSocialClass>
    {
        public void Configure(EntityTypeBuilder<CultureSocialClass> builder)
        {
            builder.HasKey(cs => new { cs.CultureId, cs.SocialClassId });

            builder.HasOne(cs => cs.Culture)
                .WithMany(c => c.InfluencedSocialClasses)
                .HasForeignKey(cs => cs.CultureId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cs => cs.SocialClass)
                .WithMany()
                .HasForeignKey(cs => cs.SocialClassId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(cs => cs.SocialClassId);
        }
    }
}
