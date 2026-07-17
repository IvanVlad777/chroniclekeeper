using ChronicleKeeper.Core.DTOs.Military;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Military
{
    // MilitaryDoctrine
    public class CreateMilitaryDoctrineCommand : IRequest<MilitaryDoctrineDto> { public MilitaryDoctrineCreateDto Dto { get; set; } = new(); }
    public class UpdateMilitaryDoctrineCommand : IRequest<MilitaryDoctrineDto> { public int Id { get; set; } public MilitaryDoctrineUpdateDto Dto { get; set; } = new(); }
    public class DeleteMilitaryDoctrineCommand : IRequest<bool> { public int Id { get; set; } }

    // MilitaryOrganization
    public class CreateMilitaryOrganizationCommand : IRequest<MilitaryOrganizationDto> { public MilitaryOrganizationCreateDto Dto { get; set; } = new(); }
    public class UpdateMilitaryOrganizationCommand : IRequest<MilitaryOrganizationDto> { public int Id { get; set; } public MilitaryOrganizationUpdateDto Dto { get; set; } = new(); }
    public class DeleteMilitaryOrganizationCommand : IRequest<bool> { public int Id { get; set; } }
    public class AddOrganizationCountryCommand : IRequest<Unit> { public int OrganizationId { get; set; } public int CountryId { get; set; } }
    public class RemoveOrganizationCountryCommand : IRequest<Unit> { public int OrganizationId { get; set; } public int CountryId { get; set; } }
    public class AddOrganizationFactionCommand : IRequest<Unit> { public int OrganizationId { get; set; } public int FactionId { get; set; } }
    public class RemoveOrganizationFactionCommand : IRequest<Unit> { public int OrganizationId { get; set; } public int FactionId { get; set; } }

    // Army
    public class CreateArmyCommand : IRequest<ArmyDto> { public ArmyCreateDto Dto { get; set; } = new(); }
    public class UpdateArmyCommand : IRequest<ArmyDto> { public int Id { get; set; } public ArmyUpdateDto Dto { get; set; } = new(); }
    public class DeleteArmyCommand : IRequest<bool> { public int Id { get; set; } }
    public class AddArmyBattleCommand : IRequest<Unit> { public int ArmyId { get; set; } public int BattleId { get; set; } }
    public class RemoveArmyBattleCommand : IRequest<Unit> { public int ArmyId { get; set; } public int BattleId { get; set; } }

    // MilitaryUnit
    public class CreateMilitaryUnitCommand : IRequest<MilitaryUnitDto> { public MilitaryUnitCreateDto Dto { get; set; } = new(); }
    public class UpdateMilitaryUnitCommand : IRequest<MilitaryUnitDto> { public int Id { get; set; } public MilitaryUnitUpdateDto Dto { get; set; } = new(); }
    public class DeleteMilitaryUnitCommand : IRequest<bool> { public int Id { get; set; } }
    public class AddUnitEquipmentCommand : IRequest<Unit> { public int UnitId { get; set; } public int EquipmentId { get; set; } }
    public class RemoveUnitEquipmentCommand : IRequest<Unit> { public int UnitId { get; set; } public int EquipmentId { get; set; } }

    // MilitaryRank
    public class CreateMilitaryRankCommand : IRequest<MilitaryRankDto> { public MilitaryRankCreateDto Dto { get; set; } = new(); }
    public class UpdateMilitaryRankCommand : IRequest<MilitaryRankDto> { public int Id { get; set; } public MilitaryRankUpdateDto Dto { get; set; } = new(); }
    public class DeleteMilitaryRankCommand : IRequest<bool> { public int Id { get; set; } }

    // Battle
    public class CreateBattleCommand : IRequest<BattleDto> { public BattleCreateDto Dto { get; set; } = new(); }
    public class UpdateBattleCommand : IRequest<BattleDto> { public int Id { get; set; } public BattleUpdateDto Dto { get; set; } = new(); }
    public class DeleteBattleCommand : IRequest<bool> { public int Id { get; set; } }

    // MilitaryEquipment
    public class CreateMilitaryEquipmentCommand : IRequest<MilitaryEquipmentDto> { public MilitaryEquipmentCreateDto Dto { get; set; } = new(); }
    public class UpdateMilitaryEquipmentCommand : IRequest<MilitaryEquipmentDto> { public int Id { get; set; } public MilitaryEquipmentUpdateDto Dto { get; set; } = new(); }
    public class DeleteMilitaryEquipmentCommand : IRequest<bool> { public int Id { get; set; } }
}
