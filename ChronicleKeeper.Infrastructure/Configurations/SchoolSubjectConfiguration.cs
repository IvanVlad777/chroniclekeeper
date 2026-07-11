using ChronicleKeeper.Core.Entities.Social.Education;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class SchoolSubjectConfiguration : LoreEntityConfiguration<SchoolSubject>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SchoolSubject> builder)
        {
            builder.Property(s => s.SubjectName)
                .HasMaxLength(200);

            builder.HasOne(s => s.School)
                .WithMany(sc => sc.Subjects)
                .HasForeignKey(s => s.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
