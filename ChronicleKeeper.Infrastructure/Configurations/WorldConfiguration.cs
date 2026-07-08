using ChronicleKeeper.Core.Entities.Worlds;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class WorldConfiguration : IEntityTypeConfiguration<World>
    {
        public void Configure(EntityTypeBuilder<World> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(w => w.Description)
                .HasMaxLength(4000);

            builder.Property(w => w.OwnerId)
                .IsRequired();

            // Deleting an Identity user must not silently delete their worlds
            builder.HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey(w => w.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(w => w.OwnerId);
        }
    }
}
