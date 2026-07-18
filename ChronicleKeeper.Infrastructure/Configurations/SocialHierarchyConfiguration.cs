using ChronicleKeeper.Core.Entities.Social.Structure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class SocialHierarchyConfiguration : LoreEntityConfiguration<SocialHierarchy>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SocialHierarchy> builder)
        {
            // History — pointer-only, SetNull.
            builder.HasOne(h => h.History)
                .WithMany()
                .HasForeignKey(h => h.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // 1:N members — FK lives on the child side, optional/SetNull: deleting a hierarchy
            // just unlinks its classes/nations, never deletes them.
            builder.HasMany(h => h.Classes)
                .WithOne(sc => sc.SocialHierarchy)
                .HasForeignKey(sc => sc.SocialHierarchyId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(h => h.Nations)
                .WithOne(n => n.SocialHierarchy)
                .HasForeignKey(n => n.SocialHierarchyId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
