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

            // History — pointer-only, SetNull: brisanje History profila samo odveže lokaciju
            builder.HasOne(l => l.History)
                .WithMany()
                .HasForeignKey(l => l.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.ToTable(t =>
                t.HasCheckConstraint("CK_Locations_Parent_NotSelf", "[ParentLocationId] <> [Id]"));

            // TPH: shadow string discriminator, same pattern as SchoolType/ContentType (School/Content
            // TPH roots elsewhere in this codebase). One value per C# subtype, not per LocationType —
            // all 6 "plain" LocationType values (Town/Village/Building/Landmark/Wilderness/Other) share
            // the base Location class. A prior attempt reused the Type column itself as the discriminator
            // (typed via LocationType with its own HasConversion<string>) but EF's discriminator-value
            // lookup doesn't consistently apply that converter when materializing mixed-subtype query
            // results — writes succeeded but reads threw "No discriminators matched the discriminator
            // value" for every non-first plain type. The shadow column avoids that entirely.
            builder.HasDiscriminator<string>("LocationSubtype")
                .HasValue<Location>("Location")
                .HasValue<Continent>("Continent")
                .HasValue<Region>("Region")
                .HasValue<Country>("Country")
                .HasValue<City>("City")
                .HasValue<District>("District");
        }
    }
}
