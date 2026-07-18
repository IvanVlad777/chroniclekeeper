using AutoMapper;
using ChronicleKeeper.Core.CQRS.CultureDetails;
using ChronicleKeeper.Core.DTOs.CultureDetails;
using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.CultureDetails.Handlers
{
    internal static class CultureDetailsValidationB
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

        public static async Task ValidateCultureAsync(ICultureRepository cultureRepo, int? cultureId, int worldId, CancellationToken ct)
        {
            if (cultureId is int cid)
            {
                var culture = await cultureRepo.FindByIdAsync(cid, ct)
                    ?? throw new DomainValidationException($"Culture with ID {cid} does not exist.");
                if (culture.WorldId != worldId)
                    throw new DomainValidationException($"Culture with ID {cid} does not belong to this world.");
            }
        }

        public static async Task ValidateReligionAsync(IReligionRepository religionRepo, int? religionId, int worldId, CancellationToken ct)
        {
            if (religionId is int rid)
            {
                var religion = await religionRepo.FindByIdAsync(rid, ct)
                    ?? throw new DomainValidationException($"Religion with ID {rid} does not exist.");
                if (religion.WorldId != worldId)
                    throw new DomainValidationException($"Religion with ID {rid} does not belong to this world.");
            }
        }

        public static async Task ValidateDeityAsync(IDeityRepository deityRepo, int? deityId, int worldId, CancellationToken ct)
        {
            if (deityId is int did)
            {
                var deity = await deityRepo.FindByIdAsync(did, ct)
                    ?? throw new DomainValidationException($"Deity with ID {did} does not exist.");
                if (deity.WorldId != worldId)
                    throw new DomainValidationException($"Deity with ID {did} does not belong to this world.");
            }
        }

        public static async Task ValidateLocationAsync(ILocationRepository locationRepo, int? locationId, int worldId, CancellationToken ct)
        {
            if (locationId is int lid)
            {
                if (!await locationRepo.ExistsInWorldAsync(lid, worldId, ct))
                    throw new DomainValidationException($"Location with ID {lid} does not exist in this world.");
            }
        }
    }

    // ============ ArchitectureStyle ============
    public class GetAllArchitectureStylesQueryHandler : IRequestHandler<GetAllArchitectureStylesQuery, List<ArchitectureStyleDto>>
    {
        private readonly IArchitectureStyleRepository _repo; private readonly IMapper _mapper;
        public GetAllArchitectureStylesQueryHandler(IArchitectureStyleRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<ArchitectureStyleDto>> Handle(GetAllArchitectureStylesQuery q, CancellationToken ct) =>
            _mapper.Map<List<ArchitectureStyleDto>>(await _repo.GetAllAsync(q.WorldId, ct));
    }
    public class GetArchitectureStyleByIdQueryHandler : IRequestHandler<GetArchitectureStyleByIdQuery, ArchitectureStyleDetailsDto?>
    {
        private readonly IArchitectureStyleRepository _repo; private readonly IMapper _mapper;
        public GetArchitectureStyleByIdQueryHandler(IArchitectureStyleRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<ArchitectureStyleDetailsDto?> Handle(GetArchitectureStyleByIdQuery q, CancellationToken ct)
        { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<ArchitectureStyleDetailsDto>(e); }
    }
    public class CreateArchitectureStyleCommandHandler : IRequestHandler<CreateArchitectureStyleCommand, ArchitectureStyleDto>
    {
        private readonly IArchitectureStyleRepository _repo; private readonly IWorldRepository _world; private readonly IHistoryRepository _history; private readonly ICultureRepository _culture; private readonly IMapper _mapper;
        public CreateArchitectureStyleCommandHandler(IArchitectureStyleRepository repo, IWorldRepository world, IHistoryRepository history, ICultureRepository culture, IMapper mapper) { _repo = repo; _world = world; _history = history; _culture = culture; _mapper = mapper; }
        public async Task<ArchitectureStyleDto> Handle(CreateArchitectureStyleCommand c, CancellationToken ct)
        {
            if (!await _world.ExistsAsync(c.Dto.WorldId, ct)) throw new DomainValidationException($"World with ID {c.Dto.WorldId} does not exist.");
            await CultureDetailsValidationB.ValidateHistoryAsync(_history, c.Dto.HistoryId, c.Dto.WorldId, ct);
            await CultureDetailsValidationB.ValidateCultureAsync(_culture, c.Dto.CultureId, c.Dto.WorldId, ct);
            return _mapper.Map<ArchitectureStyleDto>(await _repo.CreateAsync(_mapper.Map<ArchitectureStyle>(c.Dto), ct));
        }
    }
    public class UpdateArchitectureStyleCommandHandler : IRequestHandler<UpdateArchitectureStyleCommand, ArchitectureStyleDto>
    {
        private readonly IArchitectureStyleRepository _repo; private readonly IHistoryRepository _history; private readonly ICultureRepository _culture; private readonly IMapper _mapper;
        public UpdateArchitectureStyleCommandHandler(IArchitectureStyleRepository repo, IHistoryRepository history, ICultureRepository culture, IMapper mapper) { _repo = repo; _history = history; _culture = culture; _mapper = mapper; }
        public async Task<ArchitectureStyleDto> Handle(UpdateArchitectureStyleCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("ArchitectureStyle", c.Id);
            await CultureDetailsValidationB.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            await CultureDetailsValidationB.ValidateCultureAsync(_culture, c.Dto.CultureId, e.WorldId, ct);
            _mapper.Map(c.Dto, e);
            return _mapper.Map<ArchitectureStyleDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteArchitectureStyleCommandHandler : IRequestHandler<DeleteArchitectureStyleCommand, bool>
    {
        private readonly IArchitectureStyleRepository _repo;
        public DeleteArchitectureStyleCommandHandler(IArchitectureStyleRepository repo) { _repo = repo; }
        public Task<bool> Handle(DeleteArchitectureStyleCommand c, CancellationToken ct) => _repo.DeleteAsync(c.Id, ct);
    }
    public class AddArchitectureStyleLocationCommandHandler : IRequestHandler<AddArchitectureStyleLocationCommand, Unit>
    {
        private readonly IArchitectureStyleRepository _repo;
        public AddArchitectureStyleLocationCommandHandler(IArchitectureStyleRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(AddArchitectureStyleLocationCommand c, CancellationToken ct)
        {
            var style = await _repo.FindByIdAsync(c.ArchitectureStyleId, ct) ?? throw new EntityNotFoundException("ArchitectureStyle", c.ArchitectureStyleId);
            if (!await _repo.LocationExistsInWorldAsync(c.LocationId, style.WorldId, ct)) throw new DomainValidationException("Location not found in this world.");
            await _repo.AddLocationAsync(c.ArchitectureStyleId, c.LocationId, ct);
            return Unit.Value;
        }
    }
    public class RemoveArchitectureStyleLocationCommandHandler : IRequestHandler<RemoveArchitectureStyleLocationCommand, Unit>
    {
        private readonly IArchitectureStyleRepository _repo;
        public RemoveArchitectureStyleLocationCommandHandler(IArchitectureStyleRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(RemoveArchitectureStyleLocationCommand c, CancellationToken ct) { await _repo.RemoveLocationAsync(c.ArchitectureStyleId, c.LocationId, ct); return Unit.Value; }
    }

    // ============ Folklore ============
    public class GetAllFolkloreQueryHandler : IRequestHandler<GetAllFolkloreQuery, List<FolkloreDto>>
    {
        private readonly IFolkloreRepository _repo; private readonly IMapper _mapper;
        public GetAllFolkloreQueryHandler(IFolkloreRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<FolkloreDto>> Handle(GetAllFolkloreQuery q, CancellationToken ct) =>
            _mapper.Map<List<FolkloreDto>>(await _repo.GetAllAsync(q.WorldId, ct));
    }
    public class GetFolkloreByIdQueryHandler : IRequestHandler<GetFolkloreByIdQuery, FolkloreDetailsDto?>
    {
        private readonly IFolkloreRepository _repo; private readonly IMapper _mapper;
        public GetFolkloreByIdQueryHandler(IFolkloreRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<FolkloreDetailsDto?> Handle(GetFolkloreByIdQuery q, CancellationToken ct)
        { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<FolkloreDetailsDto>(e); }
    }
    public class CreateFolkloreCommandHandler : IRequestHandler<CreateFolkloreCommand, FolkloreDto>
    {
        private readonly IFolkloreRepository _repo; private readonly IWorldRepository _world; private readonly IHistoryRepository _history; private readonly ICultureRepository _culture; private readonly IMapper _mapper;
        public CreateFolkloreCommandHandler(IFolkloreRepository repo, IWorldRepository world, IHistoryRepository history, ICultureRepository culture, IMapper mapper) { _repo = repo; _world = world; _history = history; _culture = culture; _mapper = mapper; }
        public async Task<FolkloreDto> Handle(CreateFolkloreCommand c, CancellationToken ct)
        {
            if (!await _world.ExistsAsync(c.Dto.WorldId, ct)) throw new DomainValidationException($"World with ID {c.Dto.WorldId} does not exist.");
            await CultureDetailsValidationB.ValidateHistoryAsync(_history, c.Dto.HistoryId, c.Dto.WorldId, ct);
            await CultureDetailsValidationB.ValidateCultureAsync(_culture, c.Dto.CultureId, c.Dto.WorldId, ct);
            return _mapper.Map<FolkloreDto>(await _repo.CreateAsync(_mapper.Map<Folklore>(c.Dto), ct));
        }
    }
    public class UpdateFolkloreCommandHandler : IRequestHandler<UpdateFolkloreCommand, FolkloreDto>
    {
        private readonly IFolkloreRepository _repo; private readonly IHistoryRepository _history; private readonly ICultureRepository _culture; private readonly IMapper _mapper;
        public UpdateFolkloreCommandHandler(IFolkloreRepository repo, IHistoryRepository history, ICultureRepository culture, IMapper mapper) { _repo = repo; _history = history; _culture = culture; _mapper = mapper; }
        public async Task<FolkloreDto> Handle(UpdateFolkloreCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("Folklore", c.Id);
            await CultureDetailsValidationB.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            await CultureDetailsValidationB.ValidateCultureAsync(_culture, c.Dto.CultureId, e.WorldId, ct);
            _mapper.Map(c.Dto, e);
            return _mapper.Map<FolkloreDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteFolkloreCommandHandler : IRequestHandler<DeleteFolkloreCommand, bool>
    {
        private readonly IFolkloreRepository _repo;
        public DeleteFolkloreCommandHandler(IFolkloreRepository repo) { _repo = repo; }
        public Task<bool> Handle(DeleteFolkloreCommand c, CancellationToken ct) => _repo.DeleteAsync(c.Id, ct);
    }
    public class AddFolkloreEventCommandHandler : IRequestHandler<AddFolkloreEventCommand, Unit>
    {
        private readonly IFolkloreRepository _repo;
        public AddFolkloreEventCommandHandler(IFolkloreRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(AddFolkloreEventCommand c, CancellationToken ct)
        {
            var folklore = await _repo.FindByIdAsync(c.FolkloreId, ct) ?? throw new EntityNotFoundException("Folklore", c.FolkloreId);
            if (!await _repo.EventExistsInWorldAsync(c.EventId, folklore.WorldId, ct)) throw new DomainValidationException("Timeline event not found in this world.");
            await _repo.AddEventAsync(c.FolkloreId, c.EventId, ct);
            return Unit.Value;
        }
    }
    public class RemoveFolkloreEventCommandHandler : IRequestHandler<RemoveFolkloreEventCommand, Unit>
    {
        private readonly IFolkloreRepository _repo;
        public RemoveFolkloreEventCommandHandler(IFolkloreRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(RemoveFolkloreEventCommand c, CancellationToken ct) { await _repo.RemoveEventAsync(c.FolkloreId, c.EventId, ct); return Unit.Value; }
    }
    public class AddFolkloreSpeciesCommandHandler : IRequestHandler<AddFolkloreSpeciesCommand, Unit>
    {
        private readonly IFolkloreRepository _repo;
        public AddFolkloreSpeciesCommandHandler(IFolkloreRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(AddFolkloreSpeciesCommand c, CancellationToken ct)
        {
            var folklore = await _repo.FindByIdAsync(c.FolkloreId, ct) ?? throw new EntityNotFoundException("Folklore", c.FolkloreId);
            if (!await _repo.SpeciesExistsInWorldAsync(c.SpeciesId, folklore.WorldId, ct)) throw new DomainValidationException("Species not found in this world.");
            await _repo.AddSpeciesAsync(c.FolkloreId, c.SpeciesId, ct);
            return Unit.Value;
        }
    }
    public class RemoveFolkloreSpeciesCommandHandler : IRequestHandler<RemoveFolkloreSpeciesCommand, Unit>
    {
        private readonly IFolkloreRepository _repo;
        public RemoveFolkloreSpeciesCommandHandler(IFolkloreRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(RemoveFolkloreSpeciesCommand c, CancellationToken ct) { await _repo.RemoveSpeciesAsync(c.FolkloreId, c.SpeciesId, ct); return Unit.Value; }
    }

    // ============ Myth ============
    public class GetAllMythsQueryHandler : IRequestHandler<GetAllMythsQuery, List<MythDto>>
    {
        private readonly IMythRepository _repo; private readonly IMapper _mapper;
        public GetAllMythsQueryHandler(IMythRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<MythDto>> Handle(GetAllMythsQuery q, CancellationToken ct) =>
            _mapper.Map<List<MythDto>>(await _repo.GetAllAsync(q.WorldId, ct));
    }
    public class GetMythByIdQueryHandler : IRequestHandler<GetMythByIdQuery, MythDetailsDto?>
    {
        private readonly IMythRepository _repo; private readonly IMapper _mapper;
        public GetMythByIdQueryHandler(IMythRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<MythDetailsDto?> Handle(GetMythByIdQuery q, CancellationToken ct)
        { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<MythDetailsDto>(e); }
    }
    public class CreateMythCommandHandler : IRequestHandler<CreateMythCommand, MythDto>
    {
        private readonly IMythRepository _repo; private readonly IWorldRepository _world; private readonly IHistoryRepository _history; private readonly ICultureRepository _culture; private readonly IReligionRepository _religion; private readonly IDeityRepository _deity; private readonly IMapper _mapper;
        public CreateMythCommandHandler(IMythRepository repo, IWorldRepository world, IHistoryRepository history, ICultureRepository culture, IReligionRepository religion, IDeityRepository deity, IMapper mapper) { _repo = repo; _world = world; _history = history; _culture = culture; _religion = religion; _deity = deity; _mapper = mapper; }
        public async Task<MythDto> Handle(CreateMythCommand c, CancellationToken ct)
        {
            if (!await _world.ExistsAsync(c.Dto.WorldId, ct)) throw new DomainValidationException($"World with ID {c.Dto.WorldId} does not exist.");
            await CultureDetailsValidationB.ValidateHistoryAsync(_history, c.Dto.HistoryId, c.Dto.WorldId, ct);
            await CultureDetailsValidationB.ValidateCultureAsync(_culture, c.Dto.CultureId, c.Dto.WorldId, ct);
            await CultureDetailsValidationB.ValidateReligionAsync(_religion, c.Dto.ReligionId, c.Dto.WorldId, ct);
            await CultureDetailsValidationB.ValidateDeityAsync(_deity, c.Dto.DeityId, c.Dto.WorldId, ct);
            return _mapper.Map<MythDto>(await _repo.CreateAsync(_mapper.Map<Myth>(c.Dto), ct));
        }
    }
    public class UpdateMythCommandHandler : IRequestHandler<UpdateMythCommand, MythDto>
    {
        private readonly IMythRepository _repo; private readonly IHistoryRepository _history; private readonly ICultureRepository _culture; private readonly IReligionRepository _religion; private readonly IDeityRepository _deity; private readonly IMapper _mapper;
        public UpdateMythCommandHandler(IMythRepository repo, IHistoryRepository history, ICultureRepository culture, IReligionRepository religion, IDeityRepository deity, IMapper mapper) { _repo = repo; _history = history; _culture = culture; _religion = religion; _deity = deity; _mapper = mapper; }
        public async Task<MythDto> Handle(UpdateMythCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("Myth", c.Id);
            await CultureDetailsValidationB.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            await CultureDetailsValidationB.ValidateCultureAsync(_culture, c.Dto.CultureId, e.WorldId, ct);
            await CultureDetailsValidationB.ValidateReligionAsync(_religion, c.Dto.ReligionId, e.WorldId, ct);
            await CultureDetailsValidationB.ValidateDeityAsync(_deity, c.Dto.DeityId, e.WorldId, ct);
            _mapper.Map(c.Dto, e);
            return _mapper.Map<MythDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteMythCommandHandler : IRequestHandler<DeleteMythCommand, bool>
    {
        private readonly IMythRepository _repo;
        public DeleteMythCommandHandler(IMythRepository repo) { _repo = repo; }
        public Task<bool> Handle(DeleteMythCommand c, CancellationToken ct) => _repo.DeleteAsync(c.Id, ct);
    }

    // ============ CulturalFestival ============
    public class GetAllCulturalFestivalsQueryHandler : IRequestHandler<GetAllCulturalFestivalsQuery, List<CulturalFestivalDto>>
    {
        private readonly ICulturalFestivalRepository _repo; private readonly IMapper _mapper;
        public GetAllCulturalFestivalsQueryHandler(ICulturalFestivalRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<CulturalFestivalDto>> Handle(GetAllCulturalFestivalsQuery q, CancellationToken ct) =>
            _mapper.Map<List<CulturalFestivalDto>>(await _repo.GetAllAsync(q.WorldId, ct));
    }
    public class GetCulturalFestivalByIdQueryHandler : IRequestHandler<GetCulturalFestivalByIdQuery, CulturalFestivalDetailsDto?>
    {
        private readonly ICulturalFestivalRepository _repo; private readonly IMapper _mapper;
        public GetCulturalFestivalByIdQueryHandler(ICulturalFestivalRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<CulturalFestivalDetailsDto?> Handle(GetCulturalFestivalByIdQuery q, CancellationToken ct)
        { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<CulturalFestivalDetailsDto>(e); }
    }
    public class CreateCulturalFestivalCommandHandler : IRequestHandler<CreateCulturalFestivalCommand, CulturalFestivalDto>
    {
        private readonly ICulturalFestivalRepository _repo; private readonly IWorldRepository _world; private readonly IHistoryRepository _history; private readonly ICultureRepository _culture; private readonly ILocationRepository _location; private readonly IMapper _mapper;
        public CreateCulturalFestivalCommandHandler(ICulturalFestivalRepository repo, IWorldRepository world, IHistoryRepository history, ICultureRepository culture, ILocationRepository location, IMapper mapper) { _repo = repo; _world = world; _history = history; _culture = culture; _location = location; _mapper = mapper; }
        public async Task<CulturalFestivalDto> Handle(CreateCulturalFestivalCommand c, CancellationToken ct)
        {
            if (!await _world.ExistsAsync(c.Dto.WorldId, ct)) throw new DomainValidationException($"World with ID {c.Dto.WorldId} does not exist.");
            await CultureDetailsValidationB.ValidateHistoryAsync(_history, c.Dto.HistoryId, c.Dto.WorldId, ct);
            await CultureDetailsValidationB.ValidateCultureAsync(_culture, c.Dto.CultureId, c.Dto.WorldId, ct);
            await CultureDetailsValidationB.ValidateLocationAsync(_location, c.Dto.LocationId, c.Dto.WorldId, ct);
            return _mapper.Map<CulturalFestivalDto>(await _repo.CreateAsync(_mapper.Map<CulturalFestival>(c.Dto), ct));
        }
    }
    public class UpdateCulturalFestivalCommandHandler : IRequestHandler<UpdateCulturalFestivalCommand, CulturalFestivalDto>
    {
        private readonly ICulturalFestivalRepository _repo; private readonly IHistoryRepository _history; private readonly ICultureRepository _culture; private readonly ILocationRepository _location; private readonly IMapper _mapper;
        public UpdateCulturalFestivalCommandHandler(ICulturalFestivalRepository repo, IHistoryRepository history, ICultureRepository culture, ILocationRepository location, IMapper mapper) { _repo = repo; _history = history; _culture = culture; _location = location; _mapper = mapper; }
        public async Task<CulturalFestivalDto> Handle(UpdateCulturalFestivalCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("CulturalFestival", c.Id);
            await CultureDetailsValidationB.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            await CultureDetailsValidationB.ValidateCultureAsync(_culture, c.Dto.CultureId, e.WorldId, ct);
            await CultureDetailsValidationB.ValidateLocationAsync(_location, c.Dto.LocationId, e.WorldId, ct);
            _mapper.Map(c.Dto, e);
            return _mapper.Map<CulturalFestivalDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteCulturalFestivalCommandHandler : IRequestHandler<DeleteCulturalFestivalCommand, bool>
    {
        private readonly ICulturalFestivalRepository _repo;
        public DeleteCulturalFestivalCommandHandler(ICulturalFestivalRepository repo) { _repo = repo; }
        public Task<bool> Handle(DeleteCulturalFestivalCommand c, CancellationToken ct) => _repo.DeleteAsync(c.Id, ct);
    }

    // ============ CulturalInstitution ============
    public class GetAllCulturalInstitutionsQueryHandler : IRequestHandler<GetAllCulturalInstitutionsQuery, List<CulturalInstitutionDto>>
    {
        private readonly ICulturalInstitutionRepository _repo; private readonly IMapper _mapper;
        public GetAllCulturalInstitutionsQueryHandler(ICulturalInstitutionRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<CulturalInstitutionDto>> Handle(GetAllCulturalInstitutionsQuery q, CancellationToken ct) =>
            _mapper.Map<List<CulturalInstitutionDto>>(await _repo.GetAllAsync(q.WorldId, ct));
    }
    public class GetCulturalInstitutionByIdQueryHandler : IRequestHandler<GetCulturalInstitutionByIdQuery, CulturalInstitutionDetailsDto?>
    {
        private readonly ICulturalInstitutionRepository _repo; private readonly IMapper _mapper;
        public GetCulturalInstitutionByIdQueryHandler(ICulturalInstitutionRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<CulturalInstitutionDetailsDto?> Handle(GetCulturalInstitutionByIdQuery q, CancellationToken ct)
        { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<CulturalInstitutionDetailsDto>(e); }
    }
    public class CreateCulturalInstitutionCommandHandler : IRequestHandler<CreateCulturalInstitutionCommand, CulturalInstitutionDto>
    {
        private readonly ICulturalInstitutionRepository _repo; private readonly IWorldRepository _world; private readonly IHistoryRepository _history; private readonly ICultureRepository _culture; private readonly IMapper _mapper;
        public CreateCulturalInstitutionCommandHandler(ICulturalInstitutionRepository repo, IWorldRepository world, IHistoryRepository history, ICultureRepository culture, IMapper mapper) { _repo = repo; _world = world; _history = history; _culture = culture; _mapper = mapper; }
        public async Task<CulturalInstitutionDto> Handle(CreateCulturalInstitutionCommand c, CancellationToken ct)
        {
            if (!await _world.ExistsAsync(c.Dto.WorldId, ct)) throw new DomainValidationException($"World with ID {c.Dto.WorldId} does not exist.");
            await CultureDetailsValidationB.ValidateHistoryAsync(_history, c.Dto.HistoryId, c.Dto.WorldId, ct);
            await CultureDetailsValidationB.ValidateCultureAsync(_culture, c.Dto.CultureId, c.Dto.WorldId, ct);
            if (c.Dto.CityId is int cityId && !await _repo.CityExistsInWorldAsync(cityId, c.Dto.WorldId, ct))
                throw new DomainValidationException($"City with ID {cityId} does not exist in this world.");
            return _mapper.Map<CulturalInstitutionDto>(await _repo.CreateAsync(_mapper.Map<CulturalInstitution>(c.Dto), ct));
        }
    }
    public class UpdateCulturalInstitutionCommandHandler : IRequestHandler<UpdateCulturalInstitutionCommand, CulturalInstitutionDto>
    {
        private readonly ICulturalInstitutionRepository _repo; private readonly IHistoryRepository _history; private readonly ICultureRepository _culture; private readonly IMapper _mapper;
        public UpdateCulturalInstitutionCommandHandler(ICulturalInstitutionRepository repo, IHistoryRepository history, ICultureRepository culture, IMapper mapper) { _repo = repo; _history = history; _culture = culture; _mapper = mapper; }
        public async Task<CulturalInstitutionDto> Handle(UpdateCulturalInstitutionCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("CulturalInstitution", c.Id);
            await CultureDetailsValidationB.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            await CultureDetailsValidationB.ValidateCultureAsync(_culture, c.Dto.CultureId, e.WorldId, ct);
            if (c.Dto.CityId is int cityId && !await _repo.CityExistsInWorldAsync(cityId, e.WorldId, ct))
                throw new DomainValidationException($"City with ID {cityId} does not exist in this world.");
            _mapper.Map(c.Dto, e);
            return _mapper.Map<CulturalInstitutionDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteCulturalInstitutionCommandHandler : IRequestHandler<DeleteCulturalInstitutionCommand, bool>
    {
        private readonly ICulturalInstitutionRepository _repo;
        public DeleteCulturalInstitutionCommandHandler(ICulturalInstitutionRepository repo) { _repo = repo; }
        public Task<bool> Handle(DeleteCulturalInstitutionCommand c, CancellationToken ct) => _repo.DeleteAsync(c.Id, ct);
    }
}
