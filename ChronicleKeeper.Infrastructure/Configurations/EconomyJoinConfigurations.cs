using ChronicleKeeper.Core.Entities.Social.Economy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    // Join tablice smiju kaskadirati s obje strane: entiteti nemaju
    // zajedničkog kaskadirajućeg pretka (WorldId je Restrict).

    public class GuildFactionConfiguration : IEntityTypeConfiguration<GuildFaction>
    {
        public void Configure(EntityTypeBuilder<GuildFaction> builder)
        {
            builder.HasKey(gf => new { gf.GuildId, gf.FactionId });

            builder.HasOne(gf => gf.Guild)
                .WithMany(g => g.Factions)
                .HasForeignKey(gf => gf.GuildId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(gf => gf.Faction)
                .WithMany()
                .HasForeignKey(gf => gf.FactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(gf => gf.FactionId);
        }
    }

    public class GuildProfessionConfiguration : IEntityTypeConfiguration<GuildProfession>
    {
        public void Configure(EntityTypeBuilder<GuildProfession> builder)
        {
            builder.HasKey(gp => new { gp.GuildId, gp.ProfessionId });

            builder.HasOne(gp => gp.Guild)
                .WithMany(g => g.MemberProfessions)
                .HasForeignKey(gp => gp.GuildId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(gp => gp.Profession)
                .WithMany()
                .HasForeignKey(gp => gp.ProfessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(gp => gp.ProfessionId);
        }
    }

    public class GuildSocialClassConfiguration : IEntityTypeConfiguration<GuildSocialClass>
    {
        public void Configure(EntityTypeBuilder<GuildSocialClass> builder)
        {
            builder.HasKey(gs => new { gs.GuildId, gs.SocialClassId });

            builder.HasOne(gs => gs.Guild)
                .WithMany(g => g.SocialClasses)
                .HasForeignKey(gs => gs.GuildId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(gs => gs.SocialClass)
                .WithMany()
                .HasForeignKey(gs => gs.SocialClassId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(gs => gs.SocialClassId);
        }
    }

    public class CorporationFactionConfiguration : IEntityTypeConfiguration<CorporationFaction>
    {
        public void Configure(EntityTypeBuilder<CorporationFaction> builder)
        {
            builder.HasKey(cf => new { cf.CorporationId, cf.FactionId });

            builder.HasOne(cf => cf.Corporation)
                .WithMany(c => c.Factions)
                .HasForeignKey(cf => cf.CorporationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cf => cf.Faction)
                .WithMany()
                .HasForeignKey(cf => cf.FactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(cf => cf.FactionId);
        }
    }

    public class CorporationProfessionConfiguration : IEntityTypeConfiguration<CorporationProfession>
    {
        public void Configure(EntityTypeBuilder<CorporationProfession> builder)
        {
            builder.HasKey(cp => new { cp.CorporationId, cp.ProfessionId });

            builder.HasOne(cp => cp.Corporation)
                .WithMany(c => c.MemberProfessions)
                .HasForeignKey(cp => cp.CorporationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cp => cp.Profession)
                .WithMany()
                .HasForeignKey(cp => cp.ProfessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(cp => cp.ProfessionId);
        }
    }

    public class TradeRouteLocationConfiguration : IEntityTypeConfiguration<TradeRouteLocation>
    {
        public void Configure(EntityTypeBuilder<TradeRouteLocation> builder)
        {
            builder.HasKey(tl => new { tl.TradeRouteId, tl.LocationId });

            builder.HasOne(tl => tl.TradeRoute)
                .WithMany(t => t.Locations)
                .HasForeignKey(tl => tl.TradeRouteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(tl => tl.Location)
                .WithMany(l => l.TradeRouteLinks)
                .HasForeignKey(tl => tl.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(tl => tl.LocationId);
        }
    }

    public class TradeRouteResourceConfiguration : IEntityTypeConfiguration<TradeRouteResource>
    {
        public void Configure(EntityTypeBuilder<TradeRouteResource> builder)
        {
            builder.HasKey(tr => new { tr.TradeRouteId, tr.NaturalResourceId });

            builder.HasOne(tr => tr.TradeRoute)
                .WithMany(t => t.ResourcesTraded)
                .HasForeignKey(tr => tr.TradeRouteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Both sides expose a nav (same pattern as RegionSapientSpecies)
            builder.HasOne(tr => tr.NaturalResource)
                .WithMany(n => n.ExportRoutes)
                .HasForeignKey(tr => tr.NaturalResourceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(tr => tr.NaturalResourceId);
        }
    }

    public class NaturalResourceLocationConfiguration : IEntityTypeConfiguration<NaturalResourceLocation>
    {
        public void Configure(EntityTypeBuilder<NaturalResourceLocation> builder)
        {
            builder.HasKey(nl => new { nl.NaturalResourceId, nl.LocationId });

            builder.HasOne(nl => nl.NaturalResource)
                .WithMany(n => n.Locations)
                .HasForeignKey(nl => nl.NaturalResourceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(nl => nl.Location)
                .WithMany()
                .HasForeignKey(nl => nl.LocationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(nl => nl.LocationId);
        }
    }
}
