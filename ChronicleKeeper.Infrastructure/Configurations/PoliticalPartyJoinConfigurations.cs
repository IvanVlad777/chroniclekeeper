using ChronicleKeeper.Core.Entities.Social.Politics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class PoliticalPartyFactionConfiguration : IEntityTypeConfiguration<PoliticalPartyFaction>
    {
        public void Configure(EntityTypeBuilder<PoliticalPartyFaction> builder)
        {
            builder.HasKey(pf => new { pf.PoliticalPartyId, pf.FactionId });

            builder.HasOne(pf => pf.PoliticalParty)
                .WithMany(p => p.Factions)
                .HasForeignKey(pf => pf.PoliticalPartyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pf => pf.Faction)
                .WithMany()
                .HasForeignKey(pf => pf.FactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(pf => pf.FactionId);
        }
    }

    public class PoliticalPartyNationConfiguration : IEntityTypeConfiguration<PoliticalPartyNation>
    {
        public void Configure(EntityTypeBuilder<PoliticalPartyNation> builder)
        {
            builder.HasKey(pn => new { pn.PoliticalPartyId, pn.NationId });

            builder.HasOne(pn => pn.PoliticalParty)
                .WithMany(p => p.Nations)
                .HasForeignKey(pn => pn.PoliticalPartyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pn => pn.Nation)
                .WithMany()
                .HasForeignKey(pn => pn.NationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(pn => pn.NationId);
        }
    }
}
