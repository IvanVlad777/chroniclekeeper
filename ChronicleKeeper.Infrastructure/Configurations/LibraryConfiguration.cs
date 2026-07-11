using ChronicleKeeper.Core.Entities.Social.Education;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class LibraryConfiguration : LoreEntityConfiguration<Library>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Library> builder)
        {
            // No back-collection on University/Location — independent entity that
            // optionally affiliates, same shape as Culture→Religion.
            builder.HasOne(l => l.University)
                .WithMany()
                .HasForeignKey(l => l.UniversityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.Location)
                .WithMany()
                .HasForeignKey(l => l.LocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
