using ChronicleKeeper.Core.Entities.Social.Politics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class PoliticalPartyConfiguration : LoreEntityConfiguration<PoliticalParty>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PoliticalParty> builder)
        {
            builder.Property(p => p.IdeologyDescription)
                .HasMaxLength(2000);

            builder.Property(p => p.InfluenceLevel)
                .HasConversion<string>()
                .HasMaxLength(30);

            // PoliticalIdeology — Restrict: deleting an ideology in use is a friendly app error
            builder.HasOne(p => p.PoliticalIdeology)
                .WithMany(i => i.AffiliatedPoliticalParties)
                .HasForeignKey(p => p.PoliticalIdeologyId)
                .OnDelete(DeleteBehavior.Restrict);

            // GovernmentSystem — Restrict: deleting a government system in use is a friendly app error
            builder.HasOne(p => p.GovernmentSystem)
                .WithMany(g => g.PoliticalParties)
                .HasForeignKey(p => p.GovernmentSystemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
