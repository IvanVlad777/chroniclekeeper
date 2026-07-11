using ChronicleKeeper.Core.Entities.Social.Education;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class UniversityMajorConfiguration : LoreEntityConfiguration<UniversityMajor>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<UniversityMajor> builder)
        {
            builder.Property(m => m.MajorName)
                .HasMaxLength(200);

            builder.Property(m => m.DegreeLevel)
                .HasMaxLength(100);

            builder.HasOne(m => m.University)
                .WithMany(u => u.Majors)
                .HasForeignKey(m => m.UniversityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
