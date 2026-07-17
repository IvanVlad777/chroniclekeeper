using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Military;
using ChronicleKeeper.Core.Entities.Social.Military;

namespace ChronicleKeeperAPI.Mapping.Military
{
    public class MilitaryProfiles : Profile
    {
        public MilitaryProfiles()
        {
            // ---- MilitaryDoctrine ----
            CreateMap<MilitaryDoctrine, MilitaryDoctrineDto>();
            CreateMap<MilitaryDoctrineCreateDto, MilitaryDoctrine>();
            CreateMap<MilitaryDoctrineUpdateDto, MilitaryDoctrine>();
            CreateMap<MilitaryDoctrine, MilitaryDoctrineDetailsDto>()
                .ForMember(d => d.History, o => o.MapFrom(s => s.History == null ? null : new ReferenceDto { Id = s.History.Id, Name = s.History.Name }))
                .ForMember(d => d.Organizations, o => o.MapFrom(s => s.MilitaryOrganizationsUsing.Select(x => new ReferenceDto { Id = x.Id, Name = x.Name })));

            // ---- MilitaryOrganization ----
            CreateMap<MilitaryOrganization, MilitaryOrganizationDto>();
            CreateMap<MilitaryOrganizationCreateDto, MilitaryOrganization>();
            CreateMap<MilitaryOrganizationUpdateDto, MilitaryOrganization>();
            CreateMap<MilitaryOrganization, MilitaryOrganizationDetailsDto>()
                .ForMember(d => d.History, o => o.MapFrom(s => s.History == null ? null : new ReferenceDto { Id = s.History.Id, Name = s.History.Name }))
                .ForMember(d => d.MilitaryDoctrine, o => o.MapFrom(s => s.MilitaryDoctrine == null ? null : new ReferenceDto { Id = s.MilitaryDoctrine.Id, Name = s.MilitaryDoctrine.Name }))
                .ForMember(d => d.Armies, o => o.MapFrom(s => s.Armies.Select(x => new ReferenceDto { Id = x.Id, Name = x.Name })))
                .ForMember(d => d.Countries, o => o.MapFrom(s => s.Countries.Where(x => x.Country != null).Select(x => new ReferenceDto { Id = x.Country!.Id, Name = x.Country.Name })))
                .ForMember(d => d.Factions, o => o.MapFrom(s => s.Factions.Where(x => x.Faction != null).Select(x => new ReferenceDto { Id = x.Faction!.Id, Name = x.Faction.Name })));

            // ---- Army ----
            CreateMap<Army, ArmyDto>();
            CreateMap<ArmyCreateDto, Army>();
            CreateMap<ArmyUpdateDto, Army>();
            CreateMap<Army, ArmyDetailsDto>()
                .ForMember(d => d.History, o => o.MapFrom(s => s.History == null ? null : new ReferenceDto { Id = s.History.Id, Name = s.History.Name }))
                .ForMember(d => d.City, o => o.MapFrom(s => s.City == null ? null : new ReferenceDto { Id = s.City.Id, Name = s.City.Name }))
                .ForMember(d => d.MilitaryOrganization, o => o.MapFrom(s => s.MilitaryOrganization == null ? null : new ReferenceDto { Id = s.MilitaryOrganization.Id, Name = s.MilitaryOrganization.Name }))
                .ForMember(d => d.Faction, o => o.MapFrom(s => s.Faction == null ? null : new ReferenceDto { Id = s.Faction.Id, Name = s.Faction.Name }))
                .ForMember(d => d.Units, o => o.MapFrom(s => s.Units.Select(x => new ReferenceDto { Id = x.Id, Name = x.Name })))
                .ForMember(d => d.Battles, o => o.MapFrom(s => s.Battles.Where(x => x.Battle != null).Select(x => new ReferenceDto { Id = x.Battle!.Id, Name = x.Battle.Name })));

            // ---- MilitaryUnit ----
            CreateMap<MilitaryUnit, MilitaryUnitDto>();
            CreateMap<MilitaryUnitCreateDto, MilitaryUnit>();
            CreateMap<MilitaryUnitUpdateDto, MilitaryUnit>();
            CreateMap<MilitaryUnit, MilitaryUnitDetailsDto>()
                .ForMember(d => d.History, o => o.MapFrom(s => s.History == null ? null : new ReferenceDto { Id = s.History.Id, Name = s.History.Name }))
                .ForMember(d => d.BelongsToArmy, o => o.MapFrom(s => s.BelongsToArmy == null ? null : new ReferenceDto { Id = s.BelongsToArmy.Id, Name = s.BelongsToArmy.Name }))
                .ForMember(d => d.Ranks, o => o.MapFrom(s => s.Ranks.Select(x => new ReferenceDto { Id = x.Id, Name = x.Name })))
                .ForMember(d => d.Equipment, o => o.MapFrom(s => s.Equipment.Where(x => x.MilitaryEquipment != null).Select(x => new ReferenceDto { Id = x.MilitaryEquipment!.Id, Name = x.MilitaryEquipment.Name })));

            // ---- MilitaryRank ----
            CreateMap<MilitaryRank, MilitaryRankDto>();
            CreateMap<MilitaryRankCreateDto, MilitaryRank>();
            CreateMap<MilitaryRankUpdateDto, MilitaryRank>();

            // ---- Battle ----
            CreateMap<Battle, BattleDto>();
            CreateMap<BattleCreateDto, Battle>();
            CreateMap<BattleUpdateDto, Battle>();
            CreateMap<Battle, BattleDetailsDto>()
                .ForMember(d => d.History, o => o.MapFrom(s => s.History == null ? null : new ReferenceDto { Id = s.History.Id, Name = s.History.Name }))
                .ForMember(d => d.ParticipatingArmies, o => o.MapFrom(s => s.ParticipatingArmies.Where(x => x.Army != null).Select(x => new ReferenceDto { Id = x.Army!.Id, Name = x.Army.Name })));

            // ---- MilitaryEquipment ----
            CreateMap<MilitaryEquipment, MilitaryEquipmentDto>();
            CreateMap<MilitaryEquipmentCreateDto, MilitaryEquipment>();
            CreateMap<MilitaryEquipmentUpdateDto, MilitaryEquipment>();
            CreateMap<MilitaryEquipment, MilitaryEquipmentDetailsDto>()
                .ForMember(d => d.History, o => o.MapFrom(s => s.History == null ? null : new ReferenceDto { Id = s.History.Id, Name = s.History.Name }))
                .ForMember(d => d.Units, o => o.MapFrom(s => s.MilitaryUnits.Where(x => x.MilitaryUnit != null).Select(x => new ReferenceDto { Id = x.MilitaryUnit!.Id, Name = x.MilitaryUnit.Name })));
        }
    }
}
