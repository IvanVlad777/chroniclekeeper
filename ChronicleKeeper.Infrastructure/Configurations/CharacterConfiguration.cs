using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.Characters.Equipment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        // Self-referencing Parent-Child Relationship
        builder.HasOne(c => c.Father)
            .WithMany()
            .HasForeignKey(c => c.FatherId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        builder.HasOne(c => c.Mother)
            .WithMany()
            .HasForeignKey(c => c.MotherId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        // Sibling Relationship
        builder.HasMany(c => c.Siblings)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);
    }
}