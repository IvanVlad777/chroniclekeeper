using ChronicleKeeper.Core.Entities.Social.Politics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class PoliticalIdeologyConfiguration : LoreEntityConfiguration<PoliticalIdeology>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PoliticalIdeology> builder)
        {
        }
    }
}
