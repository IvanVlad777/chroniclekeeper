using ChronicleKeeper.Core.Entities.Social;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class FactionConfiguration : LoreEntityConfiguration<Faction>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Faction> builder)
        {
            builder.Property(f => f.Type)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(f => f.Motto)
                .HasMaxLength(200);

            // Brisanje lika/lokacije samo oslobađa vodstvo/sjedište
            builder.HasOne(f => f.Leader)
                .WithMany()
                .HasForeignKey(f => f.LeaderId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(f => f.Headquarters)
                .WithMany()
                .HasForeignKey(f => f.HeadquartersId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
