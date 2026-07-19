using ChronicleKeeper.Core.Entities.Social.Military;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChronicleKeeper.Infrastructure.Configurations
{
    public class MilitaryDoctrineConfiguration : LoreEntityConfiguration<MilitaryDoctrine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<MilitaryDoctrine> builder)
        {
            builder.Property(d => d.Strategy).HasMaxLength(200);
            builder.Property(d => d.Philosophy).HasMaxLength(200);

            builder.HasOne(d => d.History)
                .WithMany()
                .HasForeignKey(d => d.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class MilitaryOrganizationConfiguration : LoreEntityConfiguration<MilitaryOrganization>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<MilitaryOrganization> builder)
        {
            builder.Property(o => o.Type).HasMaxLength(100);
            builder.Property(o => o.Role).HasMaxLength(100);

            // Optional doctrine — SetNull: deleting a doctrine just unlinks the organization.
            builder.HasOne(o => o.MilitaryDoctrine)
                .WithMany(d => d.MilitaryOrganizationsUsing)
                .HasForeignKey(o => o.MilitaryDoctrineId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(o => o.History)
                .WithMany()
                .HasForeignKey(o => o.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class ArmyConfiguration : LoreEntityConfiguration<Army>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Army> builder)
        {
            // Optional pointers — SetNull.
            builder.HasOne(a => a.City)
                .WithMany(c => c.Armies)
                .HasForeignKey(a => a.CityId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(a => a.MilitaryOrganization)
                .WithMany(o => o.Armies)
                .HasForeignKey(a => a.MilitaryOrganizationId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(a => a.Faction)
                .WithMany()
                .HasForeignKey(a => a.FactionId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(a => a.History)
                .WithMany()
                .HasForeignKey(a => a.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class MilitaryUnitConfiguration : LoreEntityConfiguration<MilitaryUnit>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<MilitaryUnit> builder)
        {
            builder.Property(u => u.UnitType).HasMaxLength(100);

            // Compositional child of an army — Cascade.
            builder.HasOne(u => u.BelongsToArmy)
                .WithMany(a => a.Units)
                .HasForeignKey(u => u.BelongsToArmyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.History)
                .WithMany()
                .HasForeignKey(u => u.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class MilitaryRankConfiguration : LoreEntityConfiguration<MilitaryRank>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<MilitaryRank> builder)
        {
            builder.Property(r => r.RankTitle).HasMaxLength(100);

            // Compositional child of a unit — Cascade.
            builder.HasOne(r => r.MilitaryUnit)
                .WithMany(u => u.Ranks)
                .HasForeignKey(r => r.MilitaryUnitId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.History)
                .WithMany()
                .HasForeignKey(r => r.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class BattleConfiguration : LoreEntityConfiguration<Battle>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Battle> builder)
        {
            builder.Property(b => b.BattleDate).HasMaxLength(100);
            builder.Property(b => b.Location).HasMaxLength(200);
            builder.Property(b => b.Outcome).HasMaxLength(200);

            builder.HasOne(b => b.History)
                .WithMany()
                .HasForeignKey(b => b.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class MilitaryEquipmentConfiguration : LoreEntityConfiguration<MilitaryEquipment>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<MilitaryEquipment> builder)
        {
            builder.Property(e => e.EquipmentType).HasMaxLength(100);

            builder.HasOne(e => e.History)
                .WithMany()
                .HasForeignKey(e => e.HistoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    // ---- Join tables (composite PK, Cascade both sides; target side asymmetric — no reverse nav) ----

    public class ArmyBattleConfiguration : IEntityTypeConfiguration<ArmyBattle>
    {
        public void Configure(EntityTypeBuilder<ArmyBattle> builder)
        {
            builder.HasKey(ab => new { ab.ArmyId, ab.BattleId });

            builder.HasOne(ab => ab.Army)
                .WithMany(a => a.Battles)
                .HasForeignKey(ab => ab.ArmyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ab => ab.Battle)
                .WithMany(b => b.ParticipatingArmies)
                .HasForeignKey(ab => ab.BattleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class MilitaryUnitEquipmentConfiguration : IEntityTypeConfiguration<MilitaryUnitEquipment>
    {
        public void Configure(EntityTypeBuilder<MilitaryUnitEquipment> builder)
        {
            builder.HasKey(ue => new { ue.MilitaryUnitId, ue.MilitaryEquipmentId });

            builder.HasOne(ue => ue.MilitaryUnit)
                .WithMany(u => u.Equipment)
                .HasForeignKey(ue => ue.MilitaryUnitId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ue => ue.MilitaryEquipment)
                .WithMany(e => e.MilitaryUnits)
                .HasForeignKey(ue => ue.MilitaryEquipmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class MilitaryOrganizationCountryConfiguration : IEntityTypeConfiguration<MilitaryOrganizationCountry>
    {
        public void Configure(EntityTypeBuilder<MilitaryOrganizationCountry> builder)
        {
            builder.HasKey(oc => new { oc.MilitaryOrganizationId, oc.CountryId });

            builder.HasOne(oc => oc.MilitaryOrganization)
                .WithMany(o => o.Countries)
                .HasForeignKey(oc => oc.MilitaryOrganizationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oc => oc.Country)
                .WithMany(c => c.MilitaryOrganizations)
                .HasForeignKey(oc => oc.CountryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class MilitaryOrganizationFactionConfiguration : IEntityTypeConfiguration<MilitaryOrganizationFaction>
    {
        public void Configure(EntityTypeBuilder<MilitaryOrganizationFaction> builder)
        {
            builder.HasKey(of => new { of.MilitaryOrganizationId, of.FactionId });

            builder.HasOne(of => of.MilitaryOrganization)
                .WithMany(o => o.Factions)
                .HasForeignKey(of => of.MilitaryOrganizationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(of => of.Faction)
                .WithMany()
                .HasForeignKey(of => of.FactionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
