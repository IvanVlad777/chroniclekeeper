using ChronicleKeeper.Core.Entities.Social.Nationality;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class NationConfiguration : LoreEntityConfiguration<Nation>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Nation> builder)
        {
            // History — pointer-only, SetNull: brisanje History profila samo odveže naciju
            builder.HasOne(n => n.History)
                .WithMany()
                .HasForeignKey(n => n.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
