using ChronicleKeeper.Core.Entities.Professions;
using ChronicleKeeper.Core.Entities.Social.Education;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    /// <summary>
    /// TPH root. TradeSchool (Professions namespace) shares this "Schools" table,
    /// distinguished by the "SchoolType" string discriminator.
    /// </summary>
    public class SchoolConfiguration : LoreEntityConfiguration<School>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<School> builder)
        {
            builder.HasOne(s => s.EducationSystem)
                .WithMany(e => e.Schools)
                .HasForeignKey(s => s.EducationSystemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasDiscriminator<string>("SchoolType")
                .HasValue<School>("School")
                .HasValue<TradeSchool>("TradeSchool");
        }
    }
}
