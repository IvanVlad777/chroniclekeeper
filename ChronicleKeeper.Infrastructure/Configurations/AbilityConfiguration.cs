using ChronicleKeeper.Core.Entities.Characters.Abilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class AbilityConfiguration : LoreEntityConfiguration<Ability>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Ability> builder)
        {
            builder.Property(a => a.Type)
                .HasConversion<string>()
                .HasMaxLength(30);
        }
    }

    public class AbilityLevelConfiguration : LoreEntityConfiguration<AbilityLevel>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<AbilityLevel> builder)
        {
            builder.Property(l => l.Rank)
                .HasConversion<string>()
                .HasMaxLength(30);

            // Compositional child of Ability, required FK — same shape as JobRank→Profession
            builder.HasOne(l => l.Ability)
                .WithMany(a => a.AbilityLevels)
                .HasForeignKey(l => l.AbilityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class CharacterAbilityConfiguration : IEntityTypeConfiguration<CharacterAbility>
    {
        public void Configure(EntityTypeBuilder<CharacterAbility> builder)
        {
            builder.HasKey(ca => new { ca.CharacterId, ca.AbilityId });

            // Join tablice smiju kaskadirati s obje strane: entiteti nemaju
            // zajedničkog kaskadirajućeg pretka (WorldId je Restrict).
            builder.HasOne(ca => ca.Character)
                .WithMany(c => c.Abilities)
                .HasForeignKey(ca => ca.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ca => ca.Ability)
                .WithMany(a => a.Characters)
                .HasForeignKey(ca => ca.AbilityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(ca => ca.AbilityId);
        }
    }
}
