using ChronicleKeeper.Core.Entities.Social.Religions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class ReligionConfiguration : LoreEntityConfiguration<Religion>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Religion> builder)
        {
            builder.Property(r => r.CoreBeliefs)
                .HasMaxLength(4000);

            builder.Property(r => r.Practices)
                .HasMaxLength(4000);
        }
    }
}
