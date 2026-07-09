using ChronicleKeeper.Core.Entities.Social.Nationality;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class NationConfiguration : LoreEntityConfiguration<Nation>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Nation> builder)
        {
        }
    }
}
