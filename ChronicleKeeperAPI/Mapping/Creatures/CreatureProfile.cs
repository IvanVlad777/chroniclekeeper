using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Creature;
using ChronicleKeeper.Core.Entities.Geography.Creatures;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Animals;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Fungi;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Plants;
using ChronicleKeeper.Core.Enums;
using static ChronicleKeeper.Core.Enums.CreatureEnums;
using static ChronicleKeeper.Core.Enums.GlobalEnums;

namespace ChronicleKeeperAPI.Mapping.Creatures
{
    /// <summary>
    /// Creature is an abstract TPH root (Animal/Plant/Tree/Crop/Fungus; Tree and Crop are
    /// themselves Plant subclasses). Each ForMember below delegates to a static helper that
    /// pattern-matches on the runtime subtype to pick the right field(s), leaving the rest null —
    /// mirrors ContentProfile/LocationProfile.
    /// </summary>
    public class CreatureProfile : Profile
    {
        public CreatureProfile()
        {
            CreateMap<Creature, CreatureDto>()
                .ForMember(d => d.Subtype, opt => opt.MapFrom(src => GetSubtype(src)))
                .ForMember(d => d.ScientificName, opt => opt.MapFrom(src => GetScientificName(src)))
                .ForMember(d => d.IsMedicinal, opt => opt.MapFrom(src => GetIsMedicinal(src)))
                .ForMember(d => d.IsPoisonous, opt => opt.MapFrom(src => GetIsPoisonous(src)))
                .ForMember(d => d.IsBioluminescent, opt => opt.MapFrom(src => GetIsBioluminescent(src)))
                .ForMember(d => d.IsSymbiotic, opt => opt.MapFrom(src => GetIsSymbiotic(src)))
                .ForMember(d => d.SpecialProperties, opt => opt.MapFrom(src => GetSpecialProperties(src)))
                .ForMember(d => d.MythologicalSignificance, opt => opt.MapFrom(src => GetMythologicalSignificance(src)))
                .ForMember(d => d.IsDomesticated, opt => opt.MapFrom(src => GetIsDomesticated(src)))
                .ForMember(d => d.Diet, opt => opt.MapFrom(src => GetDiet(src)))
                .ForMember(d => d.NumberOfLegs, opt => opt.MapFrom(src => GetNumberOfLegs(src)))
                .ForMember(d => d.HasWings, opt => opt.MapFrom(src => GetHasWings(src)))
                .ForMember(d => d.HasMultipleHeads, opt => opt.MapFrom(src => GetHasMultipleHeads(src)))
                .ForMember(d => d.HasRegeneration, opt => opt.MapFrom(src => GetHasRegeneration(src)))
                .ForMember(d => d.IsSacred, opt => opt.MapFrom(src => GetIsSacred(src)))
                .ForMember(d => d.IsMythical, opt => opt.MapFrom(src => GetIsMythical(src)))
                .ForMember(d => d.IsEndangered, opt => opt.MapFrom(src => GetIsEndangered(src)))
                .ForMember(d => d.Intelligence, opt => opt.MapFrom(src => GetIntelligence(src)))
                .ForMember(d => d.SpecialAbilities, opt => opt.MapFrom(src => GetSpecialAbilities(src)))
                .ForMember(d => d.IsPackAnimal, opt => opt.MapFrom(src => GetIsPackAnimal(src)))
                .ForMember(d => d.IsAggressive, opt => opt.MapFrom(src => GetIsAggressive(src)))
                .ForMember(d => d.PlantType, opt => opt.MapFrom(src => GetPlantType(src)))
                .ForMember(d => d.Sunlight, opt => opt.MapFrom(src => GetSunlight(src)))
                .ForMember(d => d.PreferredSoil, opt => opt.MapFrom(src => GetPreferredSoil(src)))
                .ForMember(d => d.TemperatureRange, opt => opt.MapFrom(src => GetTemperatureRange(src)))
                .ForMember(d => d.Rarity, opt => opt.MapFrom(src => GetRarity(src)))
                .ForMember(d => d.IsCarnivorous, opt => opt.MapFrom(src => GetIsCarnivorous(src)))
                .ForMember(d => d.HasRegenerativeProperties, opt => opt.MapFrom(src => GetHasRegenerativeProperties(src)))
                .ForMember(d => d.CanMove, opt => opt.MapFrom(src => GetCanMove(src)))
                .ForMember(d => d.IsParasitic, opt => opt.MapFrom(src => GetIsParasitic(src)))
                .ForMember(d => d.MaxHeight, opt => opt.MapFrom(src => GetMaxHeight(src)))
                .ForMember(d => d.Lifespan, opt => opt.MapFrom(src => GetLifespan(src)))
                .ForMember(d => d.LeafType, opt => opt.MapFrom(src => GetLeafType(src)))
                .ForMember(d => d.YieldPerHectare, opt => opt.MapFrom(src => GetYieldPerHectare(src)))
                .ForMember(d => d.CropType, opt => opt.MapFrom(src => GetCropType(src)))
                .ForMember(d => d.IsEdible, opt => opt.MapFrom(src => GetIsEdible(src)))
                .ForMember(d => d.IsHallucinogenic, opt => opt.MapFrom(src => GetIsHallucinogenic(src)))
                .ForMember(d => d.HasMutagenicProperties, opt => opt.MapFrom(src => GetHasMutagenicProperties(src)))
                .ForMember(d => d.CanCommunicate, opt => opt.MapFrom(src => GetCanCommunicate(src)));

            CreateMap<Creature, CreatureDetailsDto>()
                .IncludeBase<Creature, CreatureDto>()
                .ForMember(dest => dest.ParentCreature, opt => opt.MapFrom(src => src.ParentCreature == null
                    ? null
                    : new ReferenceDto { Id = src.ParentCreature.Id, Name = src.ParentCreature.Name }))
                .ForMember(dest => dest.Subspecies, opt => opt.MapFrom(src => src.Subspecies
                    .Select(c => new ReferenceDto { Id = c.Id, Name = c.Name })))
                .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History == null
                    ? null
                    : new ReferenceDto { Id = src.History.Id, Name = src.History.Name }))
                .ForMember(dest => dest.Cities, opt => opt.MapFrom(src => src.CitiesItInhabits
                    .Where(cc => cc.City != null)
                    .Select(cc => new ReferenceDto { Id = cc.City!.Id, Name = cc.City.Name })))
                .ForMember(dest => dest.Habitats, opt => opt.MapFrom(src => src.Habitants
                    .Where(ce => ce.Ecosystem != null)
                    .Select(ce => new ReferenceDto { Id = ce.Ecosystem!.Id, Name = ce.Ecosystem.Name })));
        }

        private static string GetSubtype(Creature creature) => creature switch
        {
            Tree => "Tree",
            Crop => "Crop",
            Plant => "Plant",
            Animal => "Animal",
            Fungus => "Fungus",
            _ => creature.GetType().Name
        };

        private static string? GetScientificName(Creature creature) => creature switch
        {
            Plant p => p.ScientificName,
            Fungus f => f.ScientificName,
            _ => null
        };

        private static bool? GetIsMedicinal(Creature creature) => creature switch
        {
            Plant p => p.IsMedicinal,
            Fungus f => f.IsMedicinal,
            _ => null
        };

        private static bool? GetIsPoisonous(Creature creature) => creature switch
        {
            Plant p => p.IsPoisonous,
            Fungus f => f.IsPoisonous,
            _ => null
        };

        private static bool? GetIsBioluminescent(Creature creature) => creature switch
        {
            Plant p => p.IsBioluminescent,
            Fungus f => f.IsBioluminescent,
            _ => null
        };

        private static bool? GetIsSymbiotic(Creature creature) => creature switch
        {
            Animal a => a.IsSymbiotic,
            Plant p => p.IsSymbiotic,
            Fungus f => f.IsSymbiotic,
            _ => null
        };

        private static string? GetSpecialProperties(Creature creature) => creature switch
        {
            Plant p => p.SpecialProperties,
            Fungus f => f.SpecialProperties,
            _ => null
        };

        private static string? GetMythologicalSignificance(Creature creature) => creature switch
        {
            Plant p => p.MythologicalSignificance,
            Fungus f => f.MythologicalSignificance,
            _ => null
        };

        private static bool? GetIsDomesticated(Creature creature) => creature switch
        {
            Animal a => a.IsDomesticated,
            Crop c => c.IsDomesticated,
            _ => null
        };

        private static DietType? GetDiet(Creature creature) => creature is Animal a ? a.Diet : null;
        private static int? GetNumberOfLegs(Creature creature) => creature is Animal a ? a.NumberOfLegs : null;
        private static bool? GetHasWings(Creature creature) => creature is Animal a ? a.HasWings : null;
        private static bool? GetHasMultipleHeads(Creature creature) => creature is Animal a ? a.HasMultipleHeads : null;
        private static bool? GetHasRegeneration(Creature creature) => creature is Animal a ? a.HasRegeneration : null;
        private static bool? GetIsSacred(Creature creature) => creature is Animal a ? a.IsSacred : null;
        private static bool? GetIsMythical(Creature creature) => creature is Animal a ? a.IsMythical : null;
        private static bool? GetIsEndangered(Creature creature) => creature is Animal a ? a.IsEndangered : null;
        private static string? GetIntelligence(Creature creature) => creature is Animal a ? a.Intelligence : null;
        private static string? GetSpecialAbilities(Creature creature) => creature is Animal a ? a.SpecialAbilities : null;
        private static bool? GetIsPackAnimal(Creature creature) => creature is Animal a ? a.IsPackAnimal : null;
        private static bool? GetIsAggressive(Creature creature) => creature is Animal a ? a.IsAggressive : null;

        private static PlantType? GetPlantType(Creature creature) => creature is Plant p ? p.PlantType : null;
        private static SunlightRequirement? GetSunlight(Creature creature) => creature is Plant p ? p.Sunlight : null;
        private static SoilType? GetPreferredSoil(Creature creature) => creature is Plant p ? p.PreferredSoil : null;
        private static TemperatureRange? GetTemperatureRange(Creature creature) => creature is Plant p ? p.TemperatureRange : null;
        private static Rarity? GetRarity(Creature creature) => creature is Plant p ? p.Rarity : null;
        private static bool? GetIsCarnivorous(Creature creature) => creature is Plant p ? p.IsCarnivorous : null;
        private static bool? GetHasRegenerativeProperties(Creature creature) => creature is Plant p ? p.HasRegenerativeProperties : null;
        private static bool? GetCanMove(Creature creature) => creature is Plant p ? p.CanMove : null;
        private static bool? GetIsParasitic(Creature creature) => creature is Plant p ? p.IsParasitic : null;

        private static double? GetMaxHeight(Creature creature) => creature is Tree t ? t.MaxHeight : null;
        private static int? GetLifespan(Creature creature) => creature is Tree t ? t.Lifespan : null;
        private static LeafType? GetLeafType(Creature creature) => creature is Tree t ? t.LeafType : null;

        private static double? GetYieldPerHectare(Creature creature) => creature is Crop c ? c.YieldPerHectare : null;
        private static CropType? GetCropType(Creature creature) => creature is Crop c ? c.CropType : null;

        private static bool? GetIsEdible(Creature creature) => creature is Fungus f ? f.IsEdible : null;
        private static bool? GetIsHallucinogenic(Creature creature) => creature is Fungus f ? f.IsHallucinogenic : null;
        private static bool? GetHasMutagenicProperties(Creature creature) => creature is Fungus f ? f.HasMutagenicProperties : null;
        private static bool? GetCanCommunicate(Creature creature) => creature is Fungus f ? f.CanCommunicate : null;
    }
}
