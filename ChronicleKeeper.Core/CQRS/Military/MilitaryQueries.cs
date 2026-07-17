using ChronicleKeeper.Core.DTOs.Military;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Military
{
    public class GetAllMilitaryDoctrinesQuery : IRequest<List<MilitaryDoctrineDto>> { public int? WorldId { get; set; } }
    public class GetMilitaryDoctrineByIdQuery : IRequest<MilitaryDoctrineDetailsDto?> { public int Id { get; set; } }

    public class GetAllMilitaryOrganizationsQuery : IRequest<List<MilitaryOrganizationDto>> { public int? WorldId { get; set; } }
    public class GetMilitaryOrganizationByIdQuery : IRequest<MilitaryOrganizationDetailsDto?> { public int Id { get; set; } }

    public class GetAllArmiesQuery : IRequest<List<ArmyDto>> { public int? WorldId { get; set; } }
    public class GetArmyByIdQuery : IRequest<ArmyDetailsDto?> { public int Id { get; set; } }

    public class GetAllMilitaryUnitsQuery : IRequest<List<MilitaryUnitDto>> { public int? WorldId { get; set; } public int? ArmyId { get; set; } }
    public class GetMilitaryUnitByIdQuery : IRequest<MilitaryUnitDetailsDto?> { public int Id { get; set; } }

    public class GetAllMilitaryRanksQuery : IRequest<List<MilitaryRankDto>> { public int? WorldId { get; set; } public int? UnitId { get; set; } }
    public class GetMilitaryRankByIdQuery : IRequest<MilitaryRankDto?> { public int Id { get; set; } }

    public class GetAllBattlesQuery : IRequest<List<BattleDto>> { public int? WorldId { get; set; } }
    public class GetBattleByIdQuery : IRequest<BattleDetailsDto?> { public int Id { get; set; } }

    public class GetAllMilitaryEquipmentQuery : IRequest<List<MilitaryEquipmentDto>> { public int? WorldId { get; set; } }
    public class GetMilitaryEquipmentByIdQuery : IRequest<MilitaryEquipmentDetailsDto?> { public int Id { get; set; } }
}
