using ChronicleKeeper.Core.Entities.Social;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class FactionMemberConfiguration : IEntityTypeConfiguration<FactionMember>
    {
        public void Configure(EntityTypeBuilder<FactionMember> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Role)
                .HasMaxLength(100);

            // Oba FK-a smiju kaskadirati: Character i Faction nemaju zajedničkog
            // kaskadirajućeg pretka (WorldId je Restrict)
            builder.HasOne(m => m.Faction)
                .WithMany(f => f.Members)
                .HasForeignKey(m => m.FactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Character)
                .WithMany(c => c.Memberships)
                .HasForeignKey(m => m.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(m => new { m.FactionId, m.CharacterId })
                .IsUnique();
        }
    }
}
