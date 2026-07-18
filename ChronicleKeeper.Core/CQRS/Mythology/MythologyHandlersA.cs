using AutoMapper;
using ChronicleKeeper.Core.DTOs.Mythology;
using ChronicleKeeper.Core.Entities.Social.Religions;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Mythology.Handlers
{
    internal static class MythologyValidationA
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

        public static async Task ValidateReligionAsync(IReligionRepository religionRepo, int religionId, int worldId, CancellationToken ct)
        {
            var religion = await religionRepo.FindByIdAsync(religionId, ct)
                ?? throw new DomainValidationException($"Religion with ID {religionId} does not exist.");
            if (religion.WorldId != worldId)
                throw new DomainValidationException($"Religion with ID {religionId} does not belong to this world.");
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

        public static async Task ValidateLocationAsync(ILocationRepository locationRepo, int locationId, int worldId, CancellationToken ct)
        {
            var location = await locationRepo.FindByIdAsync(locationId, ct)
                ?? throw new DomainValidationException($"Location with ID {locationId} does not exist.");
            if (location.WorldId != worldId)
                throw new DomainValidationException($"Location with ID {locationId} does not belong to this world.");
        }

        public static async Task ValidateHolySiteAsync(IHolySiteRepository holySiteRepo, int? holySiteId, int worldId, CancellationToken ct)
        {
            if (holySiteId is int hsid)
            {
                var holySite = await holySiteRepo.FindByIdAsync(hsid, ct)
                    ?? throw new DomainValidationException($"Holy site with ID {hsid} does not exist.");
                if (holySite.WorldId != worldId)
                    throw new DomainValidationException($"Holy site with ID {hsid} does not belong to this world.");
            }
        }
    }

    // ============ HolySite ============
    public class GetAllHolySitesQueryHandler : IRequestHandler<GetAllHolySitesQuery, List<HolySiteDto>>
    {
        private readonly IHolySiteRepository _repo; private readonly IMapper _mapper;
        public GetAllHolySitesQueryHandler(IHolySiteRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<HolySiteDto>> Handle(GetAllHolySitesQuery q, CancellationToken ct) =>
            _mapper.Map<List<HolySiteDto>>(await _repo.GetAllAsync(q.WorldId, ct));
    }
    public class GetHolySiteByIdQueryHandler : IRequestHandler<GetHolySiteByIdQuery, HolySiteDetailsDto?>
    {
        private readonly IHolySiteRepository _repo; private readonly IMapper _mapper;
        public GetHolySiteByIdQueryHandler(IHolySiteRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<HolySiteDetailsDto?> Handle(GetHolySiteByIdQuery q, CancellationToken ct)
        { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<HolySiteDetailsDto>(e); }
    }
    public class CreateHolySiteCommandHandler : IRequestHandler<CreateHolySiteCommand, HolySiteDto>
    {
        private readonly IHolySiteRepository _repo; private readonly IWorldRepository _world; private readonly IHistoryRepository _history;
        private readonly IReligionRepository _religion; private readonly IDeityRepository _deity; private readonly ILocationRepository _location; private readonly IMapper _mapper;
        public CreateHolySiteCommandHandler(IHolySiteRepository repo, IWorldRepository world, IHistoryRepository history, IReligionRepository religion, IDeityRepository deity, ILocationRepository location, IMapper mapper)
        { _repo = repo; _world = world; _history = history; _religion = religion; _deity = deity; _location = location; _mapper = mapper; }
        public async Task<HolySiteDto> Handle(CreateHolySiteCommand c, CancellationToken ct)
        {
            if (!await _world.ExistsAsync(c.Dto.WorldId, ct)) throw new DomainValidationException($"World with ID {c.Dto.WorldId} does not exist.");
            await MythologyValidationA.ValidateHistoryAsync(_history, c.Dto.HistoryId, c.Dto.WorldId, ct);
            await MythologyValidationA.ValidateReligionAsync(_religion, c.Dto.ReligionId, c.Dto.WorldId, ct);
            await MythologyValidationA.ValidateDeityAsync(_deity, c.Dto.DeityId, c.Dto.WorldId, ct);
            await MythologyValidationA.ValidateLocationAsync(_location, c.Dto.LocationId, c.Dto.WorldId, ct);
            return _mapper.Map<HolySiteDto>(await _repo.CreateAsync(_mapper.Map<HolySite>(c.Dto), ct));
        }
    }
    public class UpdateHolySiteCommandHandler : IRequestHandler<UpdateHolySiteCommand, HolySiteDto>
    {
        private readonly IHolySiteRepository _repo; private readonly IHistoryRepository _history;
        private readonly IReligionRepository _religion; private readonly IDeityRepository _deity; private readonly ILocationRepository _location; private readonly IMapper _mapper;
        public UpdateHolySiteCommandHandler(IHolySiteRepository repo, IHistoryRepository history, IReligionRepository religion, IDeityRepository deity, ILocationRepository location, IMapper mapper)
        { _repo = repo; _history = history; _religion = religion; _deity = deity; _location = location; _mapper = mapper; }
        public async Task<HolySiteDto> Handle(UpdateHolySiteCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("HolySite", c.Id);
            await MythologyValidationA.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            await MythologyValidationA.ValidateReligionAsync(_religion, c.Dto.ReligionId, e.WorldId, ct);
            await MythologyValidationA.ValidateDeityAsync(_deity, c.Dto.DeityId, e.WorldId, ct);
            await MythologyValidationA.ValidateLocationAsync(_location, c.Dto.LocationId, e.WorldId, ct);
            _mapper.Map(c.Dto, e);
            return _mapper.Map<HolySiteDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteHolySiteCommandHandler : IRequestHandler<DeleteHolySiteCommand, bool>
    {
        private readonly IHolySiteRepository _repo;
        public DeleteHolySiteCommandHandler(IHolySiteRepository repo) { _repo = repo; }
        public async Task<bool> Handle(DeleteHolySiteCommand c, CancellationToken ct)
        {
            if (await _repo.IsReferencedByFestivalAsync(c.Id, ct))
                throw new DomainValidationException("This holy site is used by a religious festival. Delete or reassign the festival first.");
            return await _repo.DeleteAsync(c.Id, ct);
        }
    }

    // ============ ReligiousText ============
    public class GetAllReligiousTextsQueryHandler : IRequestHandler<GetAllReligiousTextsQuery, List<ReligiousTextDto>>
    {
        private readonly IReligiousTextRepository _repo; private readonly IMapper _mapper;
        public GetAllReligiousTextsQueryHandler(IReligiousTextRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<ReligiousTextDto>> Handle(GetAllReligiousTextsQuery q, CancellationToken ct) =>
            _mapper.Map<List<ReligiousTextDto>>(await _repo.GetAllAsync(q.WorldId, ct));
    }
    public class GetReligiousTextByIdQueryHandler : IRequestHandler<GetReligiousTextByIdQuery, ReligiousTextDetailsDto?>
    {
        private readonly IReligiousTextRepository _repo; private readonly IMapper _mapper;
        public GetReligiousTextByIdQueryHandler(IReligiousTextRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<ReligiousTextDetailsDto?> Handle(GetReligiousTextByIdQuery q, CancellationToken ct)
        { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<ReligiousTextDetailsDto>(e); }
    }
    public class CreateReligiousTextCommandHandler : IRequestHandler<CreateReligiousTextCommand, ReligiousTextDto>
    {
        private readonly IReligiousTextRepository _repo; private readonly IWorldRepository _world; private readonly IHistoryRepository _history;
        private readonly IReligionRepository _religion; private readonly IDeityRepository _deity; private readonly IMapper _mapper;
        public CreateReligiousTextCommandHandler(IReligiousTextRepository repo, IWorldRepository world, IHistoryRepository history, IReligionRepository religion, IDeityRepository deity, IMapper mapper)
        { _repo = repo; _world = world; _history = history; _religion = religion; _deity = deity; _mapper = mapper; }
        public async Task<ReligiousTextDto> Handle(CreateReligiousTextCommand c, CancellationToken ct)
        {
            if (!await _world.ExistsAsync(c.Dto.WorldId, ct)) throw new DomainValidationException($"World with ID {c.Dto.WorldId} does not exist.");
            await MythologyValidationA.ValidateHistoryAsync(_history, c.Dto.HistoryId, c.Dto.WorldId, ct);
            await MythologyValidationA.ValidateReligionAsync(_religion, c.Dto.ReligionId, c.Dto.WorldId, ct);
            await MythologyValidationA.ValidateDeityAsync(_deity, c.Dto.DeityId, c.Dto.WorldId, ct);
            return _mapper.Map<ReligiousTextDto>(await _repo.CreateAsync(_mapper.Map<ReligiousText>(c.Dto), ct));
        }
    }
    public class UpdateReligiousTextCommandHandler : IRequestHandler<UpdateReligiousTextCommand, ReligiousTextDto>
    {
        private readonly IReligiousTextRepository _repo; private readonly IHistoryRepository _history;
        private readonly IReligionRepository _religion; private readonly IDeityRepository _deity; private readonly IMapper _mapper;
        public UpdateReligiousTextCommandHandler(IReligiousTextRepository repo, IHistoryRepository history, IReligionRepository religion, IDeityRepository deity, IMapper mapper)
        { _repo = repo; _history = history; _religion = religion; _deity = deity; _mapper = mapper; }
        public async Task<ReligiousTextDto> Handle(UpdateReligiousTextCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("ReligiousText", c.Id);
            await MythologyValidationA.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            await MythologyValidationA.ValidateReligionAsync(_religion, c.Dto.ReligionId, e.WorldId, ct);
            await MythologyValidationA.ValidateDeityAsync(_deity, c.Dto.DeityId, e.WorldId, ct);
            _mapper.Map(c.Dto, e);
            return _mapper.Map<ReligiousTextDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteReligiousTextCommandHandler : IRequestHandler<DeleteReligiousTextCommand, bool>
    {
        private readonly IReligiousTextRepository _repo;
        public DeleteReligiousTextCommandHandler(IReligiousTextRepository repo) { _repo = repo; }
        public Task<bool> Handle(DeleteReligiousTextCommand c, CancellationToken ct) => _repo.DeleteAsync(c.Id, ct);
    }

    // ============ ReligiousFestival ============
    public class GetAllReligiousFestivalsQueryHandler : IRequestHandler<GetAllReligiousFestivalsQuery, List<ReligiousFestivalDto>>
    {
        private readonly IReligiousFestivalRepository _repo; private readonly IMapper _mapper;
        public GetAllReligiousFestivalsQueryHandler(IReligiousFestivalRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<ReligiousFestivalDto>> Handle(GetAllReligiousFestivalsQuery q, CancellationToken ct) =>
            _mapper.Map<List<ReligiousFestivalDto>>(await _repo.GetAllAsync(q.WorldId, ct));
    }
    public class GetReligiousFestivalByIdQueryHandler : IRequestHandler<GetReligiousFestivalByIdQuery, ReligiousFestivalDetailsDto?>
    {
        private readonly IReligiousFestivalRepository _repo; private readonly IMapper _mapper;
        public GetReligiousFestivalByIdQueryHandler(IReligiousFestivalRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<ReligiousFestivalDetailsDto?> Handle(GetReligiousFestivalByIdQuery q, CancellationToken ct)
        { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<ReligiousFestivalDetailsDto>(e); }
    }
    public class CreateReligiousFestivalCommandHandler : IRequestHandler<CreateReligiousFestivalCommand, ReligiousFestivalDto>
    {
        private readonly IReligiousFestivalRepository _repo; private readonly IWorldRepository _world; private readonly IHistoryRepository _history;
        private readonly IReligionRepository _religion; private readonly IHolySiteRepository _holySite; private readonly IMapper _mapper;
        public CreateReligiousFestivalCommandHandler(IReligiousFestivalRepository repo, IWorldRepository world, IHistoryRepository history, IReligionRepository religion, IHolySiteRepository holySite, IMapper mapper)
        { _repo = repo; _world = world; _history = history; _religion = religion; _holySite = holySite; _mapper = mapper; }
        public async Task<ReligiousFestivalDto> Handle(CreateReligiousFestivalCommand c, CancellationToken ct)
        {
            if (!await _world.ExistsAsync(c.Dto.WorldId, ct)) throw new DomainValidationException($"World with ID {c.Dto.WorldId} does not exist.");
            await MythologyValidationA.ValidateHistoryAsync(_history, c.Dto.HistoryId, c.Dto.WorldId, ct);
            await MythologyValidationA.ValidateReligionAsync(_religion, c.Dto.ReligionId, c.Dto.WorldId, ct);
            await MythologyValidationA.ValidateHolySiteAsync(_holySite, c.Dto.HolySiteId, c.Dto.WorldId, ct);
            return _mapper.Map<ReligiousFestivalDto>(await _repo.CreateAsync(_mapper.Map<ReligiousFestival>(c.Dto), ct));
        }
    }
    public class UpdateReligiousFestivalCommandHandler : IRequestHandler<UpdateReligiousFestivalCommand, ReligiousFestivalDto>
    {
        private readonly IReligiousFestivalRepository _repo; private readonly IHistoryRepository _history;
        private readonly IReligionRepository _religion; private readonly IHolySiteRepository _holySite; private readonly IMapper _mapper;
        public UpdateReligiousFestivalCommandHandler(IReligiousFestivalRepository repo, IHistoryRepository history, IReligionRepository religion, IHolySiteRepository holySite, IMapper mapper)
        { _repo = repo; _history = history; _religion = religion; _holySite = holySite; _mapper = mapper; }
        public async Task<ReligiousFestivalDto> Handle(UpdateReligiousFestivalCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("ReligiousFestival", c.Id);
            await MythologyValidationA.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            await MythologyValidationA.ValidateReligionAsync(_religion, c.Dto.ReligionId, e.WorldId, ct);
            await MythologyValidationA.ValidateHolySiteAsync(_holySite, c.Dto.HolySiteId, e.WorldId, ct);
            _mapper.Map(c.Dto, e);
            return _mapper.Map<ReligiousFestivalDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteReligiousFestivalCommandHandler : IRequestHandler<DeleteReligiousFestivalCommand, bool>
    {
        private readonly IReligiousFestivalRepository _repo;
        public DeleteReligiousFestivalCommandHandler(IReligiousFestivalRepository repo) { _repo = repo; }
        public Task<bool> Handle(DeleteReligiousFestivalCommand c, CancellationToken ct) => _repo.DeleteAsync(c.Id, ct);
    }
}
