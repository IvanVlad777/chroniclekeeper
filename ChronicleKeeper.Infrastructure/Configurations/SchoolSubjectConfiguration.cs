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

    public class SchoolSubjectTeacherConfiguration : IEntityTypeConfiguration<SchoolSubjectTeacher>
    {
        public void Configure(EntityTypeBuilder<SchoolSubjectTeacher> builder)
        {
            builder.HasKey(x => new { x.SchoolSubjectId, x.CharacterId });

            builder.HasOne(x => x.SchoolSubject)
                .WithMany(s => s.Teachers)
                .HasForeignKey(x => x.SchoolSubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // The parents (SchoolSubject, Character) share no cascading ancestor (WorldId is always
            // Restrict), so both sides may cascade.
            builder.HasOne(x => x.Character)
                .WithMany()
                .HasForeignKey(x => x.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.CharacterId);
        }
    }
}
