using ChronicleKeeper.Core.Entities.Social.Politics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class GovernmentSystemConfiguration : LoreEntityConfiguration<GovernmentSystem>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<GovernmentSystem> builder)
        {
            builder.Property(g => g.ElectionSystem)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(g => g.StabilityLevel)
                .HasConversion<string>()
                .HasMaxLength(30);

            // PoliticalIdeology — Restrict: deleting an ideology in use is a friendly app error
            builder.HasOne(g => g.PoliticalIdeology)
                .WithMany(i => i.AffiliatedGovernmentSystems)
                .HasForeignKey(g => g.PoliticalIdeologyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
