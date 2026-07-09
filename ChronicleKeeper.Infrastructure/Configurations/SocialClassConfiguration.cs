using ChronicleKeeper.Core.Entities.Social.Structure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class SocialClassConfiguration : LoreEntityConfiguration<SocialClass>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SocialClass> builder)
        {
        }
    }
}
