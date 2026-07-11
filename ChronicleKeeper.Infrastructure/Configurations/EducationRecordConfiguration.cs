using ChronicleKeeper.Core.Entities.Social.Education;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class EducationRecordConfiguration : LoreEntityConfiguration<EducationRecord>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<EducationRecord> builder)
        {
            builder.Property(e => e.Degree)
                .HasMaxLength(200);

            // Single-owner: an education record belongs to exactly one character.
            // Nullable only to allow an institutional record with no attributed person.
            builder.HasOne(e => e.Character)
                .WithMany(c => c.Educations)
                .HasForeignKey(e => e.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            // School/University are the shared/reusable side — Restrict + delete-guard
            builder.HasOne(e => e.School)
                .WithMany(s => s.Alumni)
                .HasForeignKey(e => e.SchoolId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.University)
                .WithMany(u => u.Alumni)
                .HasForeignKey(e => e.UniversityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
