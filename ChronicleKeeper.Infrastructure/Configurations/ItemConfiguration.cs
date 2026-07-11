using ChronicleKeeper.Core.Entities.Characters.Equipment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class ItemConfiguration : LoreEntityConfiguration<Item>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Item> builder)
        {
            builder.Property(i => i.Category)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(i => i.Rarity)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(i => i.Material)
                .HasMaxLength(100);

            builder.Property(i => i.SpecialProperties)
                .HasMaxLength(500);

            // Pointer-only reference (poput Faction.LeaderId) — SetNull, ne blokira brisanje lika
            builder.HasOne(i => i.CurrentOwner)
                .WithMany(c => c.Equipments)
                .HasForeignKey(i => i.CurrentOwnerId)
                .OnDelete(DeleteBehavior.SetNull);

            // Pointer-only reference — SetNull, ne blokira brisanje lokacije
            builder.HasOne(i => i.StoredAt)
                .WithMany(l => l.Items)
                .HasForeignKey(i => i.StoredAtId)
                .OnDelete(DeleteBehavior.SetNull);

            // Pointer-only reference — SetNull, ne blokira brisanje frakcije
            builder.HasOne(i => i.Faction)
                .WithMany(f => f.NotableItemsInPossesion)
                .HasForeignKey(i => i.FactionId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(i => i.CurrentOwnerId);
            builder.HasIndex(i => i.StoredAtId);
            builder.HasIndex(i => i.FactionId);
        }
    }

    public class OwnershipHistoryConfiguration : LoreEntityConfiguration<OwnershipHistory>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<OwnershipHistory> builder)
        {
            builder.Property(o => o.DateAcquired)
                .HasMaxLength(100);

            builder.Property(o => o.TransferReason)
                .HasConversion<string>()
                .HasMaxLength(30);

            // Compositional audit-trail child of Item, required FK — kaskadira kao TimelineEvent→Timeline
            builder.HasOne(o => o.Item)
                .WithMany(i => i.OwnershipHistory)
                .HasForeignKey(o => o.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Dva FK-a na Character — SQL Server odbija dva SetNull (ili Cascade) puta sa
            // iste tablice na Characters kao "multiple cascade paths"; oba Restrict, kao
            // DiplomaticAgreement.FirstNationId/SecondNationId. Repozitoriji ih ručno null-aju
            // prije brisanja lika (CharacterRepository.DeleteAsync, WorldRepository.DeleteAsync).
            builder.HasOne(o => o.PreviousOwner)
                .WithMany()
                .HasForeignKey(o => o.PreviousOwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.NewOwner)
                .WithMany()
                .HasForeignKey(o => o.NewOwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(o => o.ItemId);
        }
    }
}
