using ChronicleKeeper.Core.Entities.Characters.CharacterInfo;
using ChronicleKeeper.Core.Entities.Professions;
using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Entities.Social.Education;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    // Deferred Character↔X many-to-many join tables. Every join Cascades from BOTH parents:
    // the two parents share no cascading ancestor (WorldId is always Restrict), and two joins
    // between the same pair (School↔Character student/teacher) target distinct tables, so no
    // cascade diamond. Same convention as CharacterAbility / CultureNation.

    public class CharacterHobbyConfiguration : IEntityTypeConfiguration<CharacterHobby>
    {
        public void Configure(EntityTypeBuilder<CharacterHobby> builder)
        {
            builder.HasKey(x => new { x.CharacterId, x.HobbyId });
            builder.HasOne(x => x.Character).WithMany(c => c.Hobbies)
                .HasForeignKey(x => x.CharacterId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Hobby).WithMany(h => h.Practitioners)
                .HasForeignKey(x => x.HobbyId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.HobbyId);
        }
    }

    public class CharacterSpecialisationConfiguration : IEntityTypeConfiguration<CharacterSpecialisation>
    {
        public void Configure(EntityTypeBuilder<CharacterSpecialisation> builder)
        {
            builder.HasKey(x => new { x.CharacterId, x.SpecialisationId });
            builder.HasOne(x => x.Character).WithMany(c => c.Specialisations)
                .HasForeignKey(x => x.CharacterId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Specialisation).WithMany(sp => sp.Experts)
                .HasForeignKey(x => x.SpecialisationId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.SpecialisationId);
        }
    }

    public class CharacterClothingConfiguration : IEntityTypeConfiguration<CharacterClothing>
    {
        public void Configure(EntityTypeBuilder<CharacterClothing> builder)
        {
            builder.HasKey(x => new { x.CharacterId, x.ClothingId });
            builder.HasOne(x => x.Character).WithMany(c => c.Clothing)
                .HasForeignKey(x => x.CharacterId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Clothing).WithMany(cl => cl.Wearers)
                .HasForeignKey(x => x.ClothingId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.ClothingId);
        }
    }

    public class SchoolStudentConfiguration : IEntityTypeConfiguration<SchoolStudent>
    {
        public void Configure(EntityTypeBuilder<SchoolStudent> builder)
        {
            builder.HasKey(x => new { x.SchoolId, x.CharacterId });
            builder.HasOne(x => x.School).WithMany(s => s.Students)
                .HasForeignKey(x => x.SchoolId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Character).WithMany()
                .HasForeignKey(x => x.CharacterId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.CharacterId);
        }
    }

    public class SchoolTeacherConfiguration : IEntityTypeConfiguration<SchoolTeacher>
    {
        public void Configure(EntityTypeBuilder<SchoolTeacher> builder)
        {
            builder.HasKey(x => new { x.SchoolId, x.CharacterId });
            builder.HasOne(x => x.School).WithMany(s => s.Teachers)
                .HasForeignKey(x => x.SchoolId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Character).WithMany()
                .HasForeignKey(x => x.CharacterId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.CharacterId);
        }
    }

    public class UniversityStudentConfiguration : IEntityTypeConfiguration<UniversityStudent>
    {
        public void Configure(EntityTypeBuilder<UniversityStudent> builder)
        {
            builder.HasKey(x => new { x.UniversityId, x.CharacterId });
            builder.HasOne(x => x.University).WithMany(u => u.Students)
                .HasForeignKey(x => x.UniversityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Character).WithMany()
                .HasForeignKey(x => x.CharacterId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.CharacterId);
        }
    }

    public class UniversityProfessorConfiguration : IEntityTypeConfiguration<UniversityProfessor>
    {
        public void Configure(EntityTypeBuilder<UniversityProfessor> builder)
        {
            builder.HasKey(x => new { x.UniversityId, x.CharacterId });
            builder.HasOne(x => x.University).WithMany(u => u.Professors)
                .HasForeignKey(x => x.UniversityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Character).WithMany()
                .HasForeignKey(x => x.CharacterId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.CharacterId);
        }
    }

    public class LibraryScholarConfiguration : IEntityTypeConfiguration<LibraryScholar>
    {
        public void Configure(EntityTypeBuilder<LibraryScholar> builder)
        {
            builder.HasKey(x => new { x.LibraryId, x.CharacterId });
            builder.HasOne(x => x.Library).WithMany(l => l.Scholars)
                .HasForeignKey(x => x.LibraryId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Character).WithMany()
                .HasForeignKey(x => x.CharacterId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.CharacterId);
        }
    }

    public class CulturalInstitutionArtistConfiguration : IEntityTypeConfiguration<CulturalInstitutionArtist>
    {
        public void Configure(EntityTypeBuilder<CulturalInstitutionArtist> builder)
        {
            builder.HasKey(x => new { x.CulturalInstitutionId, x.CharacterId });
            builder.HasOne(x => x.CulturalInstitution).WithMany(ci => ci.NotableArtists)
                .HasForeignKey(x => x.CulturalInstitutionId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Character).WithMany()
                .HasForeignKey(x => x.CharacterId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.CharacterId);
        }
    }
}
