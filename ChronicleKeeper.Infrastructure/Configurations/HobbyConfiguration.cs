using ChronicleKeeper.Core.Entities.Characters.CharacterInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class HobbyConfiguration : LoreEntityConfiguration<Hobby>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Hobby> builder)
        {
            // History — pointer-only, SetNull.
            builder.HasOne(h => h.History)
                .WithMany()
                .HasForeignKey(h => h.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
