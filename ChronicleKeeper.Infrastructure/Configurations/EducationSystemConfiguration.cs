using ChronicleKeeper.Core.Entities.Social.Education;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class EducationSystemConfiguration : LoreEntityConfiguration<EducationSystem>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<EducationSystem> builder)
        {
        }
    }
}
