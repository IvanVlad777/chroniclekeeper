using ChronicleKeeper.Core.Entities.Miscellaneous;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class MutationConfiguration : LoreEntityConfiguration<Mutation>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Mutation> builder)
        {
            builder.Property(m => m.Origin)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(m => m.Effect)
                .HasConversion<string>()
                .HasMaxLength(30);

            // History — pointer-only, SetNull.
            builder.HasOne(m => m.History)
                .WithMany()
                .HasForeignKey(m => m.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Optional affected creature — SetNull; reverse nav is Creature.Mutations.
            builder.HasOne(m => m.MutantCreature)
                .WithMany(c => c.Mutations)
                .HasForeignKey(m => m.MutantCreatureId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
