using ChronicleKeeper.Core.Entities.Social.Education;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class ReligiousEducationConfiguration : LoreEntityConfiguration<ReligiousEducation>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ReligiousEducation> builder)
        {
            // Lighter, non-owned cross-reference — no reverse nav on Character
            builder.HasOne(r => r.Character)
                .WithMany()
                .HasForeignKey(r => r.CharacterId)
                .OnDelete(DeleteBehavior.Restrict);

            // Religion is a shared reference entity, no back-collection — matches Culture→Religion
            builder.HasOne(r => r.Religion)
                .WithMany()
                .HasForeignKey(r => r.ReligionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
