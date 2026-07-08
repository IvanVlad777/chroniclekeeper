using ChronicleKeeper.Core.Entities.Geography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class LocationConfiguration : LoreEntityConfiguration<Location>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Location> builder)
        {
            builder.Property(l => l.Type)
                .HasConversion<string>()
                .HasMaxLength(30);

            // Self-ref hijerarhija — Restrict: brisanje roditelja s djecom je friendly app greška
            builder.HasOne(l => l.ParentLocation)
                .WithMany(l => l.SubLocations)
                .HasForeignKey(l => l.ParentLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(l => l.ParentLocationId);

            builder.ToTable(t =>
                t.HasCheckConstraint("CK_Locations_Parent_NotSelf", "[ParentLocationId] <> [Id]"));
        }
    }
}
