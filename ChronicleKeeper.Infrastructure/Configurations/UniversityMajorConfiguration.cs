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

    public class UniversityMajorProfessorConfiguration : IEntityTypeConfiguration<UniversityMajorProfessor>
    {
        public void Configure(EntityTypeBuilder<UniversityMajorProfessor> builder)
        {
            builder.HasKey(x => new { x.UniversityMajorId, x.CharacterId });
            builder.HasOne(x => x.UniversityMajor).WithMany(m => m.Professors).HasForeignKey(x => x.UniversityMajorId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Character).WithMany().HasForeignKey(x => x.CharacterId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.CharacterId);
        }
    }

    public class UniversityMajorStudentConfiguration : IEntityTypeConfiguration<UniversityMajorStudent>
    {
        public void Configure(EntityTypeBuilder<UniversityMajorStudent> builder)
        {
            builder.HasKey(x => new { x.UniversityMajorId, x.CharacterId });
            builder.HasOne(x => x.UniversityMajor).WithMany(m => m.Students).HasForeignKey(x => x.UniversityMajorId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Character).WithMany().HasForeignKey(x => x.CharacterId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.CharacterId);
        }
    }
}
