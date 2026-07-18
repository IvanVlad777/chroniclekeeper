using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.Social.Religions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    // ---------------- HolySite ----------------
    public class HolySiteConfiguration : LoreEntityConfiguration<HolySite>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<HolySite> builder)
        {
            builder.Property(h => h.Significance).HasMaxLength(500);

            builder.HasOne(h => h.Religion)
                .WithMany(r => r.HolySites)
                .HasForeignKey(h => h.ReligionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(h => h.Deity)
                .WithMany(d => d.SacredSitesOfDeity)
                .HasForeignKey(h => h.DeityId)
                .OnDelete(DeleteBehavior.SetNull);

            // Required physical location — Restrict + delete-guard (Location is a link target, asymmetric).
            builder.HasOne(h => h.Location)
                .WithMany()
                .HasForeignKey(h => h.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(h => h.History)
                .WithMany()
                .HasForeignKey(h => h.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    // ---------------- ReligiousText ----------------
    public class ReligiousTextConfiguration : LoreEntityConfiguration<ReligiousText>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ReligiousText> builder)
        {
            builder.Property(t => t.Type).HasMaxLength(100);
            builder.Property(t => t.ContentSummary).HasMaxLength(1000);

            builder.HasOne(t => t.Religion)
                .WithMany(r => r.ReligiousTexts)
                .HasForeignKey(t => t.ReligionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.Deity)
                .WithMany(d => d.SacredTexts)
                .HasForeignKey(t => t.DeityId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(t => t.History)
                .WithMany()
                .HasForeignKey(t => t.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    // ---------------- ReligiousOrder ----------------
    public class ReligiousOrderConfiguration : LoreEntityConfiguration<ReligiousOrder>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ReligiousOrder> builder)
        {
            builder.Property(o => o.OrderType).HasMaxLength(100);
            builder.Property(o => o.Beliefs).HasMaxLength(500);

            builder.HasOne(o => o.Religion)
                .WithMany(r => r.ReligiousOrders)
                .HasForeignKey(o => o.ReligionId)
                .OnDelete(DeleteBehavior.SetNull);

            // 1:N to ReligiousEducation (its optional ReligiousOrderId FK) — SetNull.
            builder.HasMany(o => o.ClergyTraining)
                .WithOne(re => re.ReligiousOrder)
                .HasForeignKey(re => re.ReligiousOrderId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(o => o.History)
                .WithMany()
                .HasForeignKey(o => o.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    // ---------------- ReligiousFestival ----------------
    public class ReligiousFestivalConfiguration : LoreEntityConfiguration<ReligiousFestival>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ReligiousFestival> builder)
        {
            builder.Property(f => f.StartDate).HasMaxLength(100);
            builder.Property(f => f.Traditions).HasMaxLength(500);

            builder.HasOne(f => f.Religion)
                .WithMany(r => r.ReligiousFestivals)
                .HasForeignKey(f => f.ReligionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional holy site — Restrict (NOT SetNull) to avoid a second cascade path into
            // ReligiousFestivals: Religion cascades to both HolySite and Festival, and a SetNull here
            // would count as a converging path (SQL Server rejects). Restrict = no propagating path.
            // A festival must be deleted before its holy site (WorldRepository orders this; direct
            // HolySite delete is guarded). No reverse nav (asymmetric).
            builder.HasOne(f => f.HolySite)
                .WithMany()
                .HasForeignKey(f => f.HolySiteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.History)
                .WithMany()
                .HasForeignKey(f => f.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    // ---------------- Deity (own LoreEntity table) ----------------
    public class DeityConfiguration : LoreEntityConfiguration<Deity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Deity> builder)
        {
            builder.Property(d => d.Domain).HasMaxLength(100);
            builder.Property(d => d.WorshipMethods).HasMaxLength(500);
            builder.Property(d => d.DeityType).HasConversion<string>().HasMaxLength(30);

            builder.HasOne(d => d.Religion)
                .WithMany(r => r.Deities)
                .HasForeignKey(d => d.ReligionId)
                .OnDelete(DeleteBehavior.SetNull);

            // 1:N reverse to Myth (its optional DeityId FK) — SetNull.
            builder.HasMany(d => d.MajorMyths)
                .WithOne(m => m.Deity)
                .HasForeignKey(m => m.DeityId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(d => d.History)
                .WithMany()
                .HasForeignKey(d => d.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    // ---- Join tables ----

    public class DeityReligiousOrderConfiguration : IEntityTypeConfiguration<DeityReligiousOrder>
    {
        public void Configure(EntityTypeBuilder<DeityReligiousOrder> builder)
        {
            builder.HasKey(x => new { x.DeityId, x.ReligiousOrderId });

            builder.HasOne(x => x.Deity)
                .WithMany(d => d.OrdersDedicatedToDeity)
                .HasForeignKey(x => x.DeityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ReligiousOrder)
                .WithMany(o => o.Deities)
                .HasForeignKey(x => x.ReligiousOrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ReligiousOrderFactionConfiguration : IEntityTypeConfiguration<ReligiousOrderFaction>
    {
        public void Configure(EntityTypeBuilder<ReligiousOrderFaction> builder)
        {
            builder.HasKey(x => new { x.ReligiousOrderId, x.FactionId });

            builder.HasOne(x => x.ReligiousOrder)
                .WithMany(o => o.Factions)
                .HasForeignKey(x => x.ReligiousOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Faction)
                .WithMany()
                .HasForeignKey(x => x.FactionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    // Self-referencing M:N — two FKs to Deity, only DeityId cascades (Restrict on the other side).
    public class DeityAllianceConfiguration : IEntityTypeConfiguration<DeityAlliance>
    {
        public void Configure(EntityTypeBuilder<DeityAlliance> builder)
        {
            builder.HasKey(x => new { x.DeityId, x.AlliedDeityId });

            builder.HasOne(x => x.Deity)
                .WithMany(d => d.AlliedDeities)
                .HasForeignKey(x => x.DeityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.AlliedDeity)
                .WithMany()
                .HasForeignKey(x => x.AlliedDeityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class DeityRivalryConfiguration : IEntityTypeConfiguration<DeityRivalry>
    {
        public void Configure(EntityTypeBuilder<DeityRivalry> builder)
        {
            builder.HasKey(x => new { x.DeityId, x.RivalDeityId });

            builder.HasOne(x => x.Deity)
                .WithMany(d => d.RivalDeities)
                .HasForeignKey(x => x.DeityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.RivalDeity)
                .WithMany()
                .HasForeignKey(x => x.RivalDeityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
