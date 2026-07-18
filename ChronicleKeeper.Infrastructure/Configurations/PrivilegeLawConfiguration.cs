using ChronicleKeeper.Core.Entities.Social.Structure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class PrivilegeLawConfiguration : LoreEntityConfiguration<PrivilegeLaw>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PrivilegeLaw> builder)
        {
            // Compositional child of a SocialClass — required FK, Cascade.
            builder.HasOne(p => p.SocialClass)
                .WithMany(sc => sc.PrivilegeLaws)
                .HasForeignKey(p => p.SocialClassId)
                .OnDelete(DeleteBehavior.Cascade);

            // History — pointer-only, SetNull. (SET NULL from a different origin than the
            // SocialClass Cascade — no converging cascade path.)
            builder.HasOne(p => p.History)
                .WithMany()
                .HasForeignKey(p => p.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
