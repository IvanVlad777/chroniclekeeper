using ChronicleKeeper.Core.Entities.Professions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class ProfessionConfiguration : LoreEntityConfiguration<Profession>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Profession> builder)
        {
            builder.Property(p => p.RequiredSkills)
                .HasMaxLength(500);

            builder.Property(p => p.WorkEnvironment)
                .HasMaxLength(500);
        }
    }

    public class JobRankConfiguration : LoreEntityConfiguration<JobRank>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<JobRank> builder)
        {
            builder.Property(j => j.RankTitle)
                .HasMaxLength(100);

            builder.Property(j => j.Responsibilities)
                .HasMaxLength(1000);

            // Compositional child of Profession, required FK — same shape as TimelineEvent→Timeline
            builder.HasOne(j => j.Profession)
                .WithMany(p => p.JobRanks)
                .HasForeignKey(j => j.ProfessionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class SpecialisationConfiguration : LoreEntityConfiguration<Specialisation>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Specialisation> builder)
        {
            builder.Property(s => s.Field)
                .HasMaxLength(200);

            // Optional C# type, but Profession is the sole conceptual owner — Cascade like JobRank
            builder.HasOne(s => s.Profession)
                .WithMany(p => p.Specialisations)
                .HasForeignKey(s => s.ProfessionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ApprenticeshipConfiguration : LoreEntityConfiguration<Apprenticeship>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Apprenticeship> builder)
        {
            builder.Property(a => a.SkillsTaught)
                .HasMaxLength(1000);

            builder.HasOne(a => a.Profession)
                .WithMany(p => p.Apprenticeships)
                .HasForeignKey(a => a.ProfessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // TradeSchool is a shared reference (many apprenticeships can point at the same
            // school) — Restrict + delete-guard (CountApprenticeshipsUsingTradeSchoolAsync)
            builder.HasOne(a => a.TradeSchool)
                .WithMany(t => t.Apprenticeships)
                .HasForeignKey(a => a.TradeSchoolId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
