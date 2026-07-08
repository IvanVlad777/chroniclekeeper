using ChronicleKeeper.Core.Entities.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class TagConfiguration : LoreEntityConfiguration<Tag>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(t => t.Name)
                .HasMaxLength(50);

            // Ime taga je jedinstveno unutar svijeta (pooštrava bazni ne-unique index)
            builder.HasIndex(t => new { t.WorldId, t.Name })
                .IsUnique();
        }
    }
}
