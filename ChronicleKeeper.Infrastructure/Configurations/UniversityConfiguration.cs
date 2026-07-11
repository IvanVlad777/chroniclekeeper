using ChronicleKeeper.Core.Entities.Social.Education;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class UniversityConfiguration : LoreEntityConfiguration<University>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<University> builder)
        {
            builder.HasOne(u => u.EducationSystem)
                .WithMany(e => e.Universities)
                .HasForeignKey(u => u.EducationSystemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
