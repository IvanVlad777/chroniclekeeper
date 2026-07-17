using AutoMapper;
using ChronicleKeeper.Core.CQRS.Military;
using ChronicleKeeper.Core.DTOs.Military;
using ChronicleKeeper.Core.Entities.Social.Military;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Military.Handlers
{
    internal static class MilitaryValidation
    {
        public static async Task ValidateHistoryAsync(IHistoryRepository historyRepo, int? historyId, int worldId, CancellationToken ct)
        {
            if (historyId is int hid)
            {
                var history = await historyRepo.FindByIdAsync(hid, ct)
                    ?? throw new DomainValidationException($"History with ID {hid} does not exist.");
                if (history.WorldId != worldId)
                    throw new DomainValidationException($"History with ID {hid} does not belong to this world.");
            }
        }
    }

    // ============ MilitaryDoctrine ============
    public class GetAllMilitaryDoctrinesQueryHandler : IRequestHandler<GetAllMilitaryDoctrinesQuery, List<MilitaryDoctrineDto>>
    {
        private readonly IMilitaryDoctrineRepository _repo; private readonly IMapper _mapper;
        public GetAllMilitaryDoctrinesQueryHandler(IMilitaryDoctrineRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<MilitaryDoctrineDto>> Handle(GetAllMilitaryDoctrinesQuery q, CancellationToken ct) =>
            _mapper.Map<List<MilitaryDoctrineDto>>(await _repo.GetAllAsync(q.WorldId, ct));
    }
    public class GetMilitaryDoctrineByIdQueryHandler : IRequestHandler<GetMilitaryDoctrineByIdQuery, MilitaryDoctrineDetailsDto?>
    {
        private readonly IMilitaryDoctrineRepository _repo; private readonly IMapper _mapper;
        public GetMilitaryDoctrineByIdQueryHandler(IMilitaryDoctrineRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<MilitaryDoctrineDetailsDto?> Handle(GetMilitaryDoctrineByIdQuery q, CancellationToken ct)
        { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<MilitaryDoctrineDetailsDto>(e); }
    }
    public class CreateMilitaryDoctrineCommandHandler : IRequestHandler<CreateMilitaryDoctrineCommand, MilitaryDoctrineDto>
    {
        private readonly IMilitaryDoctrineRepository _repo; private readonly IWorldRepository _world; private readonly IHistoryRepository _history; private readonly IMapper _mapper;
        public CreateMilitaryDoctrineCommandHandler(IMilitaryDoctrineRepository repo, IWorldRepository world, IHistoryRepository history, IMapper mapper) { _repo = repo; _world = world; _history = history; _mapper = mapper; }
        public async Task<MilitaryDoctrineDto> Handle(CreateMilitaryDoctrineCommand c, CancellationToken ct)
        {
            if (!await _world.ExistsAsync(c.Dto.WorldId, ct)) throw new DomainValidationException($"World with ID {c.Dto.WorldId} does not exist.");
            await MilitaryValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, c.Dto.WorldId, ct);
            return _mapper.Map<MilitaryDoctrineDto>(await _repo.CreateAsync(_mapper.Map<MilitaryDoctrine>(c.Dto), ct));
        }
    }
    public class UpdateMilitaryDoctrineCommandHandler : IRequestHandler<UpdateMilitaryDoctrineCommand, MilitaryDoctrineDto>
    {
        private readonly IMilitaryDoctrineRepository _repo; private readonly IHistoryRepository _history; private readonly IMapper _mapper;
        public UpdateMilitaryDoctrineCommandHandler(IMilitaryDoctrineRepository repo, IHistoryRepository history, IMapper mapper) { _repo = repo; _history = history; _mapper = mapper; }
        public async Task<MilitaryDoctrineDto> Handle(UpdateMilitaryDoctrineCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("MilitaryDoctrine", c.Id);
            await MilitaryValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            _mapper.Map(c.Dto, e);
            return _mapper.Map<MilitaryDoctrineDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteMilitaryDoctrineCommandHandler : IRequestHandler<DeleteMilitaryDoctrineCommand, bool>
    {
        private readonly IMilitaryDoctrineRepository _repo;
        public DeleteMilitaryDoctrineCommandHandler(IMilitaryDoctrineRepository repo) { _repo = repo; }
        public Task<bool> Handle(DeleteMilitaryDoctrineCommand c, CancellationToken ct) => _repo.DeleteAsync(c.Id, ct);
    }

    // ============ MilitaryOrganization ============
    public class GetAllMilitaryOrganizationsQueryHandler : IRequestHandler<GetAllMilitaryOrganizationsQuery, List<MilitaryOrganizationDto>>
    {
        private readonly IMilitaryOrganizationRepository _repo; private readonly IMapper _mapper;
        public GetAllMilitaryOrganizationsQueryHandler(IMilitaryOrganizationRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<MilitaryOrganizationDto>> Handle(GetAllMilitaryOrganizationsQuery q, CancellationToken ct) =>
            _mapper.Map<List<MilitaryOrganizationDto>>(await _repo.GetAllAsync(q.WorldId, ct));
    }
    public class GetMilitaryOrganizationByIdQueryHandler : IRequestHandler<GetMilitaryOrganizationByIdQuery, MilitaryOrganizationDetailsDto?>
    {
        private readonly IMilitaryOrganizationRepository _repo; private readonly IMapper _mapper;
        public GetMilitaryOrganizationByIdQueryHandler(IMilitaryOrganizationRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<MilitaryOrganizationDetailsDto?> Handle(GetMilitaryOrganizationByIdQuery q, CancellationToken ct)
        { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<MilitaryOrganizationDetailsDto>(e); }
    }
    public class CreateMilitaryOrganizationCommandHandler : IRequestHandler<CreateMilitaryOrganizationCommand, MilitaryOrganizationDto>
    {
        private readonly IMilitaryOrganizationRepository _repo; private readonly IMilitaryDoctrineRepository _doctrine; private readonly IWorldRepository _world; private readonly IHistoryRepository _history; private readonly IMapper _mapper;
        public CreateMilitaryOrganizationCommandHandler(IMilitaryOrganizationRepository repo, IMilitaryDoctrineRepository doctrine, IWorldRepository world, IHistoryRepository history, IMapper mapper) { _repo = repo; _doctrine = doctrine; _world = world; _history = history; _mapper = mapper; }
        public async Task<MilitaryOrganizationDto> Handle(CreateMilitaryOrganizationCommand c, CancellationToken ct)
        {
            if (!await _world.ExistsAsync(c.Dto.WorldId, ct)) throw new DomainValidationException($"World with ID {c.Dto.WorldId} does not exist.");
            await MilitaryValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, c.Dto.WorldId, ct);
            await ValidateDoctrineAsync(_doctrine, c.Dto.MilitaryDoctrineId, c.Dto.WorldId, ct);
            return _mapper.Map<MilitaryOrganizationDto>(await _repo.CreateAsync(_mapper.Map<MilitaryOrganization>(c.Dto), ct));
        }
        internal static async Task ValidateDoctrineAsync(IMilitaryDoctrineRepository doctrineRepo, int? doctrineId, int worldId, CancellationToken ct)
        {
            if (doctrineId is int did)
            {
                var d = await doctrineRepo.FindByIdAsync(did, ct) ?? throw new DomainValidationException($"Military doctrine with ID {did} does not exist.");
                if (d.WorldId != worldId) throw new DomainValidationException($"Military doctrine with ID {did} does not belong to this world.");
            }
        }
    }
    public class UpdateMilitaryOrganizationCommandHandler : IRequestHandler<UpdateMilitaryOrganizationCommand, MilitaryOrganizationDto>
    {
        private readonly IMilitaryOrganizationRepository _repo; private readonly IMilitaryDoctrineRepository _doctrine; private readonly IHistoryRepository _history; private readonly IMapper _mapper;
        public UpdateMilitaryOrganizationCommandHandler(IMilitaryOrganizationRepository repo, IMilitaryDoctrineRepository doctrine, IHistoryRepository history, IMapper mapper) { _repo = repo; _doctrine = doctrine; _history = history; _mapper = mapper; }
        public async Task<MilitaryOrganizationDto> Handle(UpdateMilitaryOrganizationCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("MilitaryOrganization", c.Id);
            await MilitaryValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            await CreateMilitaryOrganizationCommandHandler.ValidateDoctrineAsync(_doctrine, c.Dto.MilitaryDoctrineId, e.WorldId, ct);
            _mapper.Map(c.Dto, e);
            return _mapper.Map<MilitaryOrganizationDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteMilitaryOrganizationCommandHandler : IRequestHandler<DeleteMilitaryOrganizationCommand, bool>
    {
        private readonly IMilitaryOrganizationRepository _repo;
        public DeleteMilitaryOrganizationCommandHandler(IMilitaryOrganizationRepository repo) { _repo = repo; }
        public Task<bool> Handle(DeleteMilitaryOrganizationCommand c, CancellationToken ct) => _repo.DeleteAsync(c.Id, ct);
    }
    public class AddOrganizationCountryCommandHandler : IRequestHandler<AddOrganizationCountryCommand, Unit>
    {
        private readonly IMilitaryOrganizationRepository _repo;
        public AddOrganizationCountryCommandHandler(IMilitaryOrganizationRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(AddOrganizationCountryCommand c, CancellationToken ct)
        {
            var org = await _repo.FindByIdAsync(c.OrganizationId, ct) ?? throw new EntityNotFoundException("MilitaryOrganization", c.OrganizationId);
            if (!await _repo.CountryExistsInWorldAsync(c.CountryId, org.WorldId, ct)) throw new DomainValidationException("Country not found in this world.");
            await _repo.AddCountryAsync(c.OrganizationId, c.CountryId, ct);
            return Unit.Value;
        }
    }
    public class RemoveOrganizationCountryCommandHandler : IRequestHandler<RemoveOrganizationCountryCommand, Unit>
    {
        private readonly IMilitaryOrganizationRepository _repo;
        public RemoveOrganizationCountryCommandHandler(IMilitaryOrganizationRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(RemoveOrganizationCountryCommand c, CancellationToken ct) { await _repo.RemoveCountryAsync(c.OrganizationId, c.CountryId, ct); return Unit.Value; }
    }
    public class AddOrganizationFactionCommandHandler : IRequestHandler<AddOrganizationFactionCommand, Unit>
    {
        private readonly IMilitaryOrganizationRepository _repo;
        public AddOrganizationFactionCommandHandler(IMilitaryOrganizationRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(AddOrganizationFactionCommand c, CancellationToken ct)
        {
            var org = await _repo.FindByIdAsync(c.OrganizationId, ct) ?? throw new EntityNotFoundException("MilitaryOrganization", c.OrganizationId);
            if (!await _repo.FactionExistsInWorldAsync(c.FactionId, org.WorldId, ct)) throw new DomainValidationException("Faction not found in this world.");
            await _repo.AddFactionAsync(c.OrganizationId, c.FactionId, ct);
            return Unit.Value;
        }
    }
    public class RemoveOrganizationFactionCommandHandler : IRequestHandler<RemoveOrganizationFactionCommand, Unit>
    {
        private readonly IMilitaryOrganizationRepository _repo;
        public RemoveOrganizationFactionCommandHandler(IMilitaryOrganizationRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(RemoveOrganizationFactionCommand c, CancellationToken ct) { await _repo.RemoveFactionAsync(c.OrganizationId, c.FactionId, ct); return Unit.Value; }
    }

    // ============ Army ============
    public class GetAllArmiesQueryHandler : IRequestHandler<GetAllArmiesQuery, List<ArmyDto>>
    {
        private readonly IArmyRepository _repo; private readonly IMapper _mapper;
        public GetAllArmiesQueryHandler(IArmyRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<ArmyDto>> Handle(GetAllArmiesQuery q, CancellationToken ct) => _mapper.Map<List<ArmyDto>>(await _repo.GetAllAsync(q.WorldId, ct));
    }
    public class GetArmyByIdQueryHandler : IRequestHandler<GetArmyByIdQuery, ArmyDetailsDto?>
    {
        private readonly IArmyRepository _repo; private readonly IMapper _mapper;
        public GetArmyByIdQueryHandler(IArmyRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<ArmyDetailsDto?> Handle(GetArmyByIdQuery q, CancellationToken ct) { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<ArmyDetailsDto>(e); }
    }
    public class CreateArmyCommandHandler : IRequestHandler<CreateArmyCommand, ArmyDto>
    {
        private readonly IArmyRepository _repo; private readonly IMilitaryOrganizationRepository _org; private readonly IWorldRepository _world; private readonly IHistoryRepository _history; private readonly IMapper _mapper;
        public CreateArmyCommandHandler(IArmyRepository repo, IMilitaryOrganizationRepository org, IWorldRepository world, IHistoryRepository history, IMapper mapper) { _repo = repo; _org = org; _world = world; _history = history; _mapper = mapper; }
        public async Task<ArmyDto> Handle(CreateArmyCommand c, CancellationToken ct)
        {
            if (!await _world.ExistsAsync(c.Dto.WorldId, ct)) throw new DomainValidationException($"World with ID {c.Dto.WorldId} does not exist.");
            await MilitaryValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, c.Dto.WorldId, ct);
            await ValidateOrgAsync(_org, c.Dto.MilitaryOrganizationId, c.Dto.WorldId, ct);
            return _mapper.Map<ArmyDto>(await _repo.CreateAsync(_mapper.Map<Army>(c.Dto), ct));
        }
        internal static async Task ValidateOrgAsync(IMilitaryOrganizationRepository orgRepo, int? orgId, int worldId, CancellationToken ct)
        {
            if (orgId is int oid)
            {
                var o = await orgRepo.FindByIdAsync(oid, ct) ?? throw new DomainValidationException($"Military organization with ID {oid} does not exist.");
                if (o.WorldId != worldId) throw new DomainValidationException($"Military organization with ID {oid} does not belong to this world.");
            }
        }
    }
    public class UpdateArmyCommandHandler : IRequestHandler<UpdateArmyCommand, ArmyDto>
    {
        private readonly IArmyRepository _repo; private readonly IMilitaryOrganizationRepository _org; private readonly IHistoryRepository _history; private readonly IMapper _mapper;
        public UpdateArmyCommandHandler(IArmyRepository repo, IMilitaryOrganizationRepository org, IHistoryRepository history, IMapper mapper) { _repo = repo; _org = org; _history = history; _mapper = mapper; }
        public async Task<ArmyDto> Handle(UpdateArmyCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("Army", c.Id);
            await MilitaryValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            await CreateArmyCommandHandler.ValidateOrgAsync(_org, c.Dto.MilitaryOrganizationId, e.WorldId, ct);
            _mapper.Map(c.Dto, e);
            return _mapper.Map<ArmyDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteArmyCommandHandler : IRequestHandler<DeleteArmyCommand, bool>
    {
        private readonly IArmyRepository _repo;
        public DeleteArmyCommandHandler(IArmyRepository repo) { _repo = repo; }
        public Task<bool> Handle(DeleteArmyCommand c, CancellationToken ct) => _repo.DeleteAsync(c.Id, ct);
    }
    public class AddArmyBattleCommandHandler : IRequestHandler<AddArmyBattleCommand, Unit>
    {
        private readonly IArmyRepository _repo;
        public AddArmyBattleCommandHandler(IArmyRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(AddArmyBattleCommand c, CancellationToken ct)
        {
            var army = await _repo.FindByIdAsync(c.ArmyId, ct) ?? throw new EntityNotFoundException("Army", c.ArmyId);
            if (!await _repo.BattleExistsInWorldAsync(c.BattleId, army.WorldId, ct)) throw new DomainValidationException("Battle not found in this world.");
            await _repo.AddBattleAsync(c.ArmyId, c.BattleId, ct);
            return Unit.Value;
        }
    }
    public class RemoveArmyBattleCommandHandler : IRequestHandler<RemoveArmyBattleCommand, Unit>
    {
        private readonly IArmyRepository _repo;
        public RemoveArmyBattleCommandHandler(IArmyRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(RemoveArmyBattleCommand c, CancellationToken ct) { await _repo.RemoveBattleAsync(c.ArmyId, c.BattleId, ct); return Unit.Value; }
    }

    // ============ MilitaryUnit (child of Army) ============
    public class GetAllMilitaryUnitsQueryHandler : IRequestHandler<GetAllMilitaryUnitsQuery, List<MilitaryUnitDto>>
    {
        private readonly IMilitaryUnitRepository _repo; private readonly IMapper _mapper;
        public GetAllMilitaryUnitsQueryHandler(IMilitaryUnitRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<MilitaryUnitDto>> Handle(GetAllMilitaryUnitsQuery q, CancellationToken ct) => _mapper.Map<List<MilitaryUnitDto>>(await _repo.GetAllAsync(q.WorldId, q.ArmyId, ct));
    }
    public class GetMilitaryUnitByIdQueryHandler : IRequestHandler<GetMilitaryUnitByIdQuery, MilitaryUnitDetailsDto?>
    {
        private readonly IMilitaryUnitRepository _repo; private readonly IMapper _mapper;
        public GetMilitaryUnitByIdQueryHandler(IMilitaryUnitRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<MilitaryUnitDetailsDto?> Handle(GetMilitaryUnitByIdQuery q, CancellationToken ct) { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<MilitaryUnitDetailsDto>(e); }
    }
    public class CreateMilitaryUnitCommandHandler : IRequestHandler<CreateMilitaryUnitCommand, MilitaryUnitDto>
    {
        private readonly IMilitaryUnitRepository _repo; private readonly IArmyRepository _army; private readonly IHistoryRepository _history; private readonly IMapper _mapper;
        public CreateMilitaryUnitCommandHandler(IMilitaryUnitRepository repo, IArmyRepository army, IHistoryRepository history, IMapper mapper) { _repo = repo; _army = army; _history = history; _mapper = mapper; }
        public async Task<MilitaryUnitDto> Handle(CreateMilitaryUnitCommand c, CancellationToken ct)
        {
            var army = await _army.FindByIdAsync(c.Dto.BelongsToArmyId, ct) ?? throw new DomainValidationException($"Army with ID {c.Dto.BelongsToArmyId} does not exist.");
            await MilitaryValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, army.WorldId, ct);
            var entity = _mapper.Map<MilitaryUnit>(c.Dto);
            entity.WorldId = army.WorldId; // world derived from parent army
            return _mapper.Map<MilitaryUnitDto>(await _repo.CreateAsync(entity, ct));
        }
    }
    public class UpdateMilitaryUnitCommandHandler : IRequestHandler<UpdateMilitaryUnitCommand, MilitaryUnitDto>
    {
        private readonly IMilitaryUnitRepository _repo; private readonly IHistoryRepository _history; private readonly IMapper _mapper;
        public UpdateMilitaryUnitCommandHandler(IMilitaryUnitRepository repo, IHistoryRepository history, IMapper mapper) { _repo = repo; _history = history; _mapper = mapper; }
        public async Task<MilitaryUnitDto> Handle(UpdateMilitaryUnitCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("MilitaryUnit", c.Id);
            await MilitaryValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            _mapper.Map(c.Dto, e);
            return _mapper.Map<MilitaryUnitDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteMilitaryUnitCommandHandler : IRequestHandler<DeleteMilitaryUnitCommand, bool>
    {
        private readonly IMilitaryUnitRepository _repo;
        public DeleteMilitaryUnitCommandHandler(IMilitaryUnitRepository repo) { _repo = repo; }
        public Task<bool> Handle(DeleteMilitaryUnitCommand c, CancellationToken ct) => _repo.DeleteAsync(c.Id, ct);
    }
    public class AddUnitEquipmentCommandHandler : IRequestHandler<AddUnitEquipmentCommand, Unit>
    {
        private readonly IMilitaryUnitRepository _repo;
        public AddUnitEquipmentCommandHandler(IMilitaryUnitRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(AddUnitEquipmentCommand c, CancellationToken ct)
        {
            var unit = await _repo.FindByIdAsync(c.UnitId, ct) ?? throw new EntityNotFoundException("MilitaryUnit", c.UnitId);
            if (!await _repo.EquipmentExistsInWorldAsync(c.EquipmentId, unit.WorldId, ct)) throw new DomainValidationException("Equipment not found in this world.");
            await _repo.AddEquipmentAsync(c.UnitId, c.EquipmentId, ct);
            return Unit.Value;
        }
    }
    public class RemoveUnitEquipmentCommandHandler : IRequestHandler<RemoveUnitEquipmentCommand, Unit>
    {
        private readonly IMilitaryUnitRepository _repo;
        public RemoveUnitEquipmentCommandHandler(IMilitaryUnitRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(RemoveUnitEquipmentCommand c, CancellationToken ct) { await _repo.RemoveEquipmentAsync(c.UnitId, c.EquipmentId, ct); return Unit.Value; }
    }

    // ============ MilitaryRank (child of MilitaryUnit) ============
    public class GetAllMilitaryRanksQueryHandler : IRequestHandler<GetAllMilitaryRanksQuery, List<MilitaryRankDto>>
    {
        private readonly IMilitaryRankRepository _repo; private readonly IMapper _mapper;
        public GetAllMilitaryRanksQueryHandler(IMilitaryRankRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<MilitaryRankDto>> Handle(GetAllMilitaryRanksQuery q, CancellationToken ct) => _mapper.Map<List<MilitaryRankDto>>(await _repo.GetAllAsync(q.WorldId, q.UnitId, ct));
    }
    public class GetMilitaryRankByIdQueryHandler : IRequestHandler<GetMilitaryRankByIdQuery, MilitaryRankDto?>
    {
        private readonly IMilitaryRankRepository _repo; private readonly IMapper _mapper;
        public GetMilitaryRankByIdQueryHandler(IMilitaryRankRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<MilitaryRankDto?> Handle(GetMilitaryRankByIdQuery q, CancellationToken ct) { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<MilitaryRankDto>(e); }
    }
    public class CreateMilitaryRankCommandHandler : IRequestHandler<CreateMilitaryRankCommand, MilitaryRankDto>
    {
        private readonly IMilitaryRankRepository _repo; private readonly IMilitaryUnitRepository _unit; private readonly IHistoryRepository _history; private readonly IMapper _mapper;
        public CreateMilitaryRankCommandHandler(IMilitaryRankRepository repo, IMilitaryUnitRepository unit, IHistoryRepository history, IMapper mapper) { _repo = repo; _unit = unit; _history = history; _mapper = mapper; }
        public async Task<MilitaryRankDto> Handle(CreateMilitaryRankCommand c, CancellationToken ct)
        {
            var unit = await _unit.FindByIdAsync(c.Dto.MilitaryUnitId, ct) ?? throw new DomainValidationException($"Military unit with ID {c.Dto.MilitaryUnitId} does not exist.");
            await MilitaryValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, unit.WorldId, ct);
            var entity = _mapper.Map<MilitaryRank>(c.Dto);
            entity.WorldId = unit.WorldId; // world derived from parent unit
            return _mapper.Map<MilitaryRankDto>(await _repo.CreateAsync(entity, ct));
        }
    }
    public class UpdateMilitaryRankCommandHandler : IRequestHandler<UpdateMilitaryRankCommand, MilitaryRankDto>
    {
        private readonly IMilitaryRankRepository _repo; private readonly IHistoryRepository _history; private readonly IMapper _mapper;
        public UpdateMilitaryRankCommandHandler(IMilitaryRankRepository repo, IHistoryRepository history, IMapper mapper) { _repo = repo; _history = history; _mapper = mapper; }
        public async Task<MilitaryRankDto> Handle(UpdateMilitaryRankCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("MilitaryRank", c.Id);
            await MilitaryValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            _mapper.Map(c.Dto, e);
            return _mapper.Map<MilitaryRankDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteMilitaryRankCommandHandler : IRequestHandler<DeleteMilitaryRankCommand, bool>
    {
        private readonly IMilitaryRankRepository _repo;
        public DeleteMilitaryRankCommandHandler(IMilitaryRankRepository repo) { _repo = repo; }
        public Task<bool> Handle(DeleteMilitaryRankCommand c, CancellationToken ct) => _repo.DeleteAsync(c.Id, ct);
    }

    // ============ Battle ============
    public class GetAllBattlesQueryHandler : IRequestHandler<GetAllBattlesQuery, List<BattleDto>>
    {
        private readonly IBattleRepository _repo; private readonly IMapper _mapper;
        public GetAllBattlesQueryHandler(IBattleRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<BattleDto>> Handle(GetAllBattlesQuery q, CancellationToken ct) => _mapper.Map<List<BattleDto>>(await _repo.GetAllAsync(q.WorldId, ct));
    }
    public class GetBattleByIdQueryHandler : IRequestHandler<GetBattleByIdQuery, BattleDetailsDto?>
    {
        private readonly IBattleRepository _repo; private readonly IMapper _mapper;
        public GetBattleByIdQueryHandler(IBattleRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<BattleDetailsDto?> Handle(GetBattleByIdQuery q, CancellationToken ct) { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<BattleDetailsDto>(e); }
    }
    public class CreateBattleCommandHandler : IRequestHandler<CreateBattleCommand, BattleDto>
    {
        private readonly IBattleRepository _repo; private readonly IWorldRepository _world; private readonly IHistoryRepository _history; private readonly IMapper _mapper;
        public CreateBattleCommandHandler(IBattleRepository repo, IWorldRepository world, IHistoryRepository history, IMapper mapper) { _repo = repo; _world = world; _history = history; _mapper = mapper; }
        public async Task<BattleDto> Handle(CreateBattleCommand c, CancellationToken ct)
        {
            if (!await _world.ExistsAsync(c.Dto.WorldId, ct)) throw new DomainValidationException($"World with ID {c.Dto.WorldId} does not exist.");
            await MilitaryValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, c.Dto.WorldId, ct);
            return _mapper.Map<BattleDto>(await _repo.CreateAsync(_mapper.Map<Battle>(c.Dto), ct));
        }
    }
    public class UpdateBattleCommandHandler : IRequestHandler<UpdateBattleCommand, BattleDto>
    {
        private readonly IBattleRepository _repo; private readonly IHistoryRepository _history; private readonly IMapper _mapper;
        public UpdateBattleCommandHandler(IBattleRepository repo, IHistoryRepository history, IMapper mapper) { _repo = repo; _history = history; _mapper = mapper; }
        public async Task<BattleDto> Handle(UpdateBattleCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("Battle", c.Id);
            await MilitaryValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            _mapper.Map(c.Dto, e);
            return _mapper.Map<BattleDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteBattleCommandHandler : IRequestHandler<DeleteBattleCommand, bool>
    {
        private readonly IBattleRepository _repo;
        public DeleteBattleCommandHandler(IBattleRepository repo) { _repo = repo; }
        public Task<bool> Handle(DeleteBattleCommand c, CancellationToken ct) => _repo.DeleteAsync(c.Id, ct);
    }

    // ============ MilitaryEquipment ============
    public class GetAllMilitaryEquipmentQueryHandler : IRequestHandler<GetAllMilitaryEquipmentQuery, List<MilitaryEquipmentDto>>
    {
        private readonly IMilitaryEquipmentRepository _repo; private readonly IMapper _mapper;
        public GetAllMilitaryEquipmentQueryHandler(IMilitaryEquipmentRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<MilitaryEquipmentDto>> Handle(GetAllMilitaryEquipmentQuery q, CancellationToken ct) => _mapper.Map<List<MilitaryEquipmentDto>>(await _repo.GetAllAsync(q.WorldId, ct));
    }
    public class GetMilitaryEquipmentByIdQueryHandler : IRequestHandler<GetMilitaryEquipmentByIdQuery, MilitaryEquipmentDetailsDto?>
    {
        private readonly IMilitaryEquipmentRepository _repo; private readonly IMapper _mapper;
        public GetMilitaryEquipmentByIdQueryHandler(IMilitaryEquipmentRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<MilitaryEquipmentDetailsDto?> Handle(GetMilitaryEquipmentByIdQuery q, CancellationToken ct) { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<MilitaryEquipmentDetailsDto>(e); }
    }
    public class CreateMilitaryEquipmentCommandHandler : IRequestHandler<CreateMilitaryEquipmentCommand, MilitaryEquipmentDto>
    {
        private readonly IMilitaryEquipmentRepository _repo; private readonly IWorldRepository _world; private readonly IHistoryRepository _history; private readonly IMapper _mapper;
        public CreateMilitaryEquipmentCommandHandler(IMilitaryEquipmentRepository repo, IWorldRepository world, IHistoryRepository history, IMapper mapper) { _repo = repo; _world = world; _history = history; _mapper = mapper; }
        public async Task<MilitaryEquipmentDto> Handle(CreateMilitaryEquipmentCommand c, CancellationToken ct)
        {
            if (!await _world.ExistsAsync(c.Dto.WorldId, ct)) throw new DomainValidationException($"World with ID {c.Dto.WorldId} does not exist.");
            await MilitaryValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, c.Dto.WorldId, ct);
            return _mapper.Map<MilitaryEquipmentDto>(await _repo.CreateAsync(_mapper.Map<MilitaryEquipment>(c.Dto), ct));
        }
    }
    public class UpdateMilitaryEquipmentCommandHandler : IRequestHandler<UpdateMilitaryEquipmentCommand, MilitaryEquipmentDto>
    {
        private readonly IMilitaryEquipmentRepository _repo; private readonly IHistoryRepository _history; private readonly IMapper _mapper;
        public UpdateMilitaryEquipmentCommandHandler(IMilitaryEquipmentRepository repo, IHistoryRepository history, IMapper mapper) { _repo = repo; _history = history; _mapper = mapper; }
        public async Task<MilitaryEquipmentDto> Handle(UpdateMilitaryEquipmentCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("MilitaryEquipment", c.Id);
            await MilitaryValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            _mapper.Map(c.Dto, e);
            return _mapper.Map<MilitaryEquipmentDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteMilitaryEquipmentCommandHandler : IRequestHandler<DeleteMilitaryEquipmentCommand, bool>
    {
        private readonly IMilitaryEquipmentRepository _repo;
        public DeleteMilitaryEquipmentCommandHandler(IMilitaryEquipmentRepository repo) { _repo = repo; }
        public Task<bool> Handle(DeleteMilitaryEquipmentCommand c, CancellationToken ct) => _repo.DeleteAsync(c.Id, ct);
    }
}
