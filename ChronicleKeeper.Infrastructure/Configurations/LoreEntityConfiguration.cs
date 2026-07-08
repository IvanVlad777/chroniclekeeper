using ChronicleKeeper.Core.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    /// <summary>
    /// Shared configuration for every lore entity. The WorldId FK is always Restrict —
    /// cascading from World to everything would create multiple-cascade-path cycles on
    /// SQL Server; deleting a world goes through WorldRepository.DeleteAsync instead.
    /// </summary>
    public abstract class LoreEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : LoreEntity
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .HasMaxLength(4000);

            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired();

            builder.HasOne(e => e.World)
                .WithMany()
                .HasForeignKey(e => e.WorldId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.WorldId, e.Name });

            ConfigureEntity(builder);
        }

        protected abstract void ConfigureEntity(EntityTypeBuilder<T> builder);
    }
}
