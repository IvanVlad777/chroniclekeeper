using ChronicleKeeper.Core.Entities.Professions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    // Join tablice smiju kaskadirati s obje strane: entiteti nemaju
    // zajedničkog kaskadirajućeg pretka (WorldId je Restrict).

    public class ProfessionSapientSpeciesConfiguration : IEntityTypeConfiguration<ProfessionSapientSpecies>
    {
        public void Configure(EntityTypeBuilder<ProfessionSapientSpecies> builder)
        {
            builder.HasKey(ps => new { ps.ProfessionId, ps.SapientSpeciesId });

            builder.HasOne(ps => ps.Profession)
                .WithMany(p => p.PracticedBySpecies)
                .HasForeignKey(ps => ps.ProfessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ps => ps.SapientSpecies)
                .WithMany(s => s.FrequentOccupations)
                .HasForeignKey(ps => ps.SapientSpeciesId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(ps => ps.SapientSpeciesId);
        }
    }

    public class ProfessionSocialClassConfiguration : IEntityTypeConfiguration<ProfessionSocialClass>
    {
        public void Configure(EntityTypeBuilder<ProfessionSocialClass> builder)
        {
            builder.HasKey(ps => new { ps.ProfessionId, ps.SocialClassId });

            builder.HasOne(ps => ps.Profession)
                .WithMany(p => p.SocialClasses)
                .HasForeignKey(ps => ps.ProfessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ps => ps.SocialClass)
                .WithMany()
                .HasForeignKey(ps => ps.SocialClassId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(ps => ps.SocialClassId);
        }
    }

    public class ProfessionTradeSchoolConfiguration : IEntityTypeConfiguration<ProfessionTradeSchool>
    {
        public void Configure(EntityTypeBuilder<ProfessionTradeSchool> builder)
        {
            builder.HasKey(pt => new { pt.ProfessionId, pt.TradeSchoolId });

            builder.HasOne(pt => pt.Profession)
                .WithMany(p => p.TradeSchools)
                .HasForeignKey(pt => pt.ProfessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pt => pt.TradeSchool)
                .WithMany(t => t.TrainedProfessions)
                .HasForeignKey(pt => pt.TradeSchoolId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(pt => pt.TradeSchoolId);
        }
    }
}
