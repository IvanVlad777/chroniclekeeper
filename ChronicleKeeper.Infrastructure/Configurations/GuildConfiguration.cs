using ChronicleKeeper.Core.Entities.Social.Economy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class GuildConfiguration : LoreEntityConfiguration<Guild>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Guild> builder)
        {
            builder.Property(g => g.GuildType).HasMaxLength(100);
            builder.Property(g => g.PrimaryActivity).HasMaxLength(200);

            // System FKs — Restrict: deleting a system in use is a friendly app error
            builder.HasOne(g => g.TaxationSystem)
                .WithMany(t => t.TaxedGuilds)
                .HasForeignKey(g => g.TaxationSystemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(g => g.Industry)
                .WithMany()
                .HasForeignKey(g => g.IndustryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(g => g.LegalSystem)
                .WithMany(l => l.Guilds)
                .HasForeignKey(g => g.LegalSystemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(g => g.EducationSystem)
                .WithMany(e => e.GuildsProvidingEducation)
                .HasForeignKey(g => g.EducationSystemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(g => g.History)
                .WithMany()
                .HasForeignKey(g => g.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Compositional children — die with the guild
            builder.HasMany(g => g.GuildRanks)
                .WithOne(r => r.Guild)
                .HasForeignKey(r => r.GuildId)
                .OnDelete(DeleteBehavior.Cascade);

            // Pointer children — an apprenticeship/education record survives its guild's deletion
            builder.HasMany(g => g.Apprenticeships)
                .WithOne(a => a.Guild)
                .HasForeignKey(a => a.GuildId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(g => g.Alumni)
                .WithOne(e => e.Guild)
                .HasForeignKey(e => e.GuildId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class GuildRankConfiguration : LoreEntityConfiguration<GuildRank>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<GuildRank> builder)
        {
            builder.Property(r => r.RankTitle).HasMaxLength(100);

            builder.Property(r => r.RankLevel)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.HasOne(r => r.History)
                .WithMany()
                .HasForeignKey(r => r.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
