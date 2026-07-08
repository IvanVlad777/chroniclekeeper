using ChronicleKeeper.Core.Entities.Characters.CharacterInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class CharacterRelationshipConfiguration : IEntityTypeConfiguration<CharacterRelationship>
    {
        public void Configure(EntityTypeBuilder<CharacterRelationship> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Type)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(r => r.Description)
                .HasMaxLength(500);

            // Vlasnička strana kaskadira; druga strana Restrict (dva cascade FK-a na istu
            // tablicu = ciklus) — CharacterRepository.DeleteAsync čisti RelatedCharacterId strane
            builder.HasOne(r => r.Character)
                .WithMany(c => c.Relationships)
                .HasForeignKey(r => r.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.RelatedCharacter)
                .WithMany()
                .HasForeignKey(r => r.RelatedCharacterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(r => new { r.CharacterId, r.RelatedCharacterId, r.Type })
                .IsUnique();
            builder.HasIndex(r => r.RelatedCharacterId);

            builder.ToTable(t =>
                t.HasCheckConstraint("CK_CharacterRelationships_NotSelf", "[CharacterId] <> [RelatedCharacterId]"));
        }
    }
}
