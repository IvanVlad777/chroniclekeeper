using AutoMapper;
using ChronicleKeeper.Core.CQRS.Mythology;
using ChronicleKeeper.Core.DTOs.Mythology;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.Social.Religions;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Mythology.Handlers
{
    internal static class MythologyValidationB
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
    }

    // ============ ReligiousOrder ============
    public class GetAllReligiousOrdersQueryHandler : IRequestHandler<GetAllReligiousOrdersQuery, List<ReligiousOrderDto>>
    {
        private readonly IReligiousOrderRepository _repo; private readonly IMapper _mapper;
        public GetAllReligiousOrdersQueryHandler(IReligiousOrderRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<ReligiousOrderDto>> Handle(GetAllReligiousOrdersQuery q, CancellationToken ct) =>
            _mapper.Map<List<ReligiousOrderDto>>(await _repo.GetAllAsync(q.WorldId, ct));
    }
    public class GetReligiousOrderByIdQueryHandler : IRequestHandler<GetReligiousOrderByIdQuery, ReligiousOrderDetailsDto?>
    {
        private readonly IReligiousOrderRepository _repo; private readonly IMapper _mapper;
        public GetReligiousOrderByIdQueryHandler(IReligiousOrderRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<ReligiousOrderDetailsDto?> Handle(GetReligiousOrderByIdQuery q, CancellationToken ct)
        { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<ReligiousOrderDetailsDto>(e); }
    }
    public class CreateReligiousOrderCommandHandler : IRequestHandler<CreateReligiousOrderCommand, ReligiousOrderDto>
    {
        private readonly IReligiousOrderRepository _repo; private readonly IWorldRepository _world; private readonly IHistoryRepository _history; private readonly IReligionRepository _religion; private readonly IMapper _mapper;
        public CreateReligiousOrderCommandHandler(IReligiousOrderRepository repo, IWorldRepository world, IHistoryRepository history, IReligionRepository religion, IMapper mapper) { _repo = repo; _world = world; _history = history; _religion = religion; _mapper = mapper; }
        public async Task<ReligiousOrderDto> Handle(CreateReligiousOrderCommand c, CancellationToken ct)
        {
            if (!await _world.ExistsAsync(c.Dto.WorldId, ct)) throw new DomainValidationException($"World with ID {c.Dto.WorldId} does not exist.");
            await MythologyValidationB.ValidateHistoryAsync(_history, c.Dto.HistoryId, c.Dto.WorldId, ct);
            await MythologyValidationB.ValidateReligionAsync(_religion, c.Dto.ReligionId, c.Dto.WorldId, ct);
            return _mapper.Map<ReligiousOrderDto>(await _repo.CreateAsync(_mapper.Map<ReligiousOrder>(c.Dto), ct));
        }
    }
    public class UpdateReligiousOrderCommandHandler : IRequestHandler<UpdateReligiousOrderCommand, ReligiousOrderDto>
    {
        private readonly IReligiousOrderRepository _repo; private readonly IHistoryRepository _history; private readonly IReligionRepository _religion; private readonly IMapper _mapper;
        public UpdateReligiousOrderCommandHandler(IReligiousOrderRepository repo, IHistoryRepository history, IReligionRepository religion, IMapper mapper) { _repo = repo; _history = history; _religion = religion; _mapper = mapper; }
        public async Task<ReligiousOrderDto> Handle(UpdateReligiousOrderCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("ReligiousOrder", c.Id);
            await MythologyValidationB.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            await MythologyValidationB.ValidateReligionAsync(_religion, c.Dto.ReligionId, e.WorldId, ct);
            _mapper.Map(c.Dto, e);
            return _mapper.Map<ReligiousOrderDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteReligiousOrderCommandHandler : IRequestHandler<DeleteReligiousOrderCommand, bool>
    {
        private readonly IReligiousOrderRepository _repo;
        public DeleteReligiousOrderCommandHandler(IReligiousOrderRepository repo) { _repo = repo; }
        public Task<bool> Handle(DeleteReligiousOrderCommand c, CancellationToken ct) => _repo.DeleteAsync(c.Id, ct);
    }
    public class AddReligiousOrderFactionCommandHandler : IRequestHandler<AddReligiousOrderFactionCommand, Unit>
    {
        private readonly IReligiousOrderRepository _repo;
        public AddReligiousOrderFactionCommandHandler(IReligiousOrderRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(AddReligiousOrderFactionCommand c, CancellationToken ct)
        {
            var order = await _repo.FindByIdAsync(c.OrderId, ct) ?? throw new EntityNotFoundException("ReligiousOrder", c.OrderId);
            if (!await _repo.FactionExistsInWorldAsync(c.FactionId, order.WorldId, ct)) throw new DomainValidationException("Faction not found in this world.");
            await _repo.AddFactionAsync(c.OrderId, c.FactionId, ct);
            return Unit.Value;
        }
    }
    public class RemoveReligiousOrderFactionCommandHandler : IRequestHandler<RemoveReligiousOrderFactionCommand, Unit>
    {
        private readonly IReligiousOrderRepository _repo;
        public RemoveReligiousOrderFactionCommandHandler(IReligiousOrderRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(RemoveReligiousOrderFactionCommand c, CancellationToken ct) { await _repo.RemoveFactionAsync(c.OrderId, c.FactionId, ct); return Unit.Value; }
    }

    // ============ Deity ============
    public class GetAllDeitiesQueryHandler : IRequestHandler<GetAllDeitiesQuery, List<DeityDto>>
    {
        private readonly IDeityRepository _repo; private readonly IMapper _mapper;
        public GetAllDeitiesQueryHandler(IDeityRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<DeityDto>> Handle(GetAllDeitiesQuery q, CancellationToken ct) =>
            _mapper.Map<List<DeityDto>>(await _repo.GetAllAsync(q.WorldId, ct));
    }
    public class GetDeityByIdQueryHandler : IRequestHandler<GetDeityByIdQuery, DeityDetailsDto?>
    {
        private readonly IDeityRepository _repo; private readonly IMapper _mapper;
        public GetDeityByIdQueryHandler(IDeityRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<DeityDetailsDto?> Handle(GetDeityByIdQuery q, CancellationToken ct)
        { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<DeityDetailsDto>(e); }
    }
    public class CreateDeityCommandHandler : IRequestHandler<CreateDeityCommand, DeityDto>
    {
        private readonly IDeityRepository _repo; private readonly IWorldRepository _world; private readonly IHistoryRepository _history; private readonly IReligionRepository _religion; private readonly IMapper _mapper;
        public CreateDeityCommandHandler(IDeityRepository repo, IWorldRepository world, IHistoryRepository history, IReligionRepository religion, IMapper mapper) { _repo = repo; _world = world; _history = history; _religion = religion; _mapper = mapper; }
        public async Task<DeityDto> Handle(CreateDeityCommand c, CancellationToken ct)
        {
            if (!await _world.ExistsAsync(c.Dto.WorldId, ct)) throw new DomainValidationException($"World with ID {c.Dto.WorldId} does not exist.");
            await MythologyValidationB.ValidateHistoryAsync(_history, c.Dto.HistoryId, c.Dto.WorldId, ct);
            await MythologyValidationB.ValidateReligionAsync(_religion, c.Dto.ReligionId, c.Dto.WorldId, ct);
            return _mapper.Map<DeityDto>(await _repo.CreateAsync(_mapper.Map<Deity>(c.Dto), ct));
        }
    }
    public class UpdateDeityCommandHandler : IRequestHandler<UpdateDeityCommand, DeityDto>
    {
        private readonly IDeityRepository _repo; private readonly IHistoryRepository _history; private readonly IReligionRepository _religion; private readonly IMapper _mapper;
        public UpdateDeityCommandHandler(IDeityRepository repo, IHistoryRepository history, IReligionRepository religion, IMapper mapper) { _repo = repo; _history = history; _religion = religion; _mapper = mapper; }
        public async Task<DeityDto> Handle(UpdateDeityCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("Deity", c.Id);
            await MythologyValidationB.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            await MythologyValidationB.ValidateReligionAsync(_religion, c.Dto.ReligionId, e.WorldId, ct);
            _mapper.Map(c.Dto, e);
            return _mapper.Map<DeityDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteDeityCommandHandler : IRequestHandler<DeleteDeityCommand, bool>
    {
        private readonly IDeityRepository _repo;
        public DeleteDeityCommandHandler(IDeityRepository repo) { _repo = repo; }
        public Task<bool> Handle(DeleteDeityCommand c, CancellationToken ct) => _repo.DeleteAsync(c.Id, ct);
    }
    public class AddDeityOrderCommandHandler : IRequestHandler<AddDeityOrderCommand, Unit>
    {
        private readonly IDeityRepository _repo;
        public AddDeityOrderCommandHandler(IDeityRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(AddDeityOrderCommand c, CancellationToken ct)
        {
            var deity = await _repo.FindByIdAsync(c.DeityId, ct) ?? throw new EntityNotFoundException("Deity", c.DeityId);
            if (!await _repo.OrderExistsInWorldAsync(c.OrderId, deity.WorldId, ct)) throw new DomainValidationException("Religious order not found in this world.");
            await _repo.AddOrderAsync(c.DeityId, c.OrderId, ct);
            return Unit.Value;
        }
    }
    public class RemoveDeityOrderCommandHandler : IRequestHandler<RemoveDeityOrderCommand, Unit>
    {
        private readonly IDeityRepository _repo;
        public RemoveDeityOrderCommandHandler(IDeityRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(RemoveDeityOrderCommand c, CancellationToken ct) { await _repo.RemoveOrderAsync(c.DeityId, c.OrderId, ct); return Unit.Value; }
    }
    public class AddDeityAllyCommandHandler : IRequestHandler<AddDeityAllyCommand, Unit>
    {
        private readonly IDeityRepository _repo;
        public AddDeityAllyCommandHandler(IDeityRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(AddDeityAllyCommand c, CancellationToken ct)
        {
            if (c.DeityId == c.AlliedDeityId) throw new DomainValidationException("A deity cannot be allied with itself.");
            var deity = await _repo.FindByIdAsync(c.DeityId, ct) ?? throw new EntityNotFoundException("Deity", c.DeityId);
            if (!await _repo.DeityExistsInWorldAsync(c.AlliedDeityId, deity.WorldId, ct)) throw new DomainValidationException("Allied deity not found in this world.");
            if (await _repo.AllianceExistsAsync(c.DeityId, c.AlliedDeityId, ct)) throw new DomainValidationException("This alliance already exists.");
            await _repo.AddAllyAsync(c.DeityId, c.AlliedDeityId, ct);
            return Unit.Value;
        }
    }
    public class RemoveDeityAllyCommandHandler : IRequestHandler<RemoveDeityAllyCommand, Unit>
    {
        private readonly IDeityRepository _repo;
        public RemoveDeityAllyCommandHandler(IDeityRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(RemoveDeityAllyCommand c, CancellationToken ct) { await _repo.RemoveAllyAsync(c.DeityId, c.AlliedDeityId, ct); return Unit.Value; }
    }
    public class AddDeityRivalCommandHandler : IRequestHandler<AddDeityRivalCommand, Unit>
    {
        private readonly IDeityRepository _repo;
        public AddDeityRivalCommandHandler(IDeityRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(AddDeityRivalCommand c, CancellationToken ct)
        {
            if (c.DeityId == c.RivalDeityId) throw new DomainValidationException("A deity cannot be a rival of itself.");
            var deity = await _repo.FindByIdAsync(c.DeityId, ct) ?? throw new EntityNotFoundException("Deity", c.DeityId);
            if (!await _repo.DeityExistsInWorldAsync(c.RivalDeityId, deity.WorldId, ct)) throw new DomainValidationException("Rival deity not found in this world.");
            if (await _repo.RivalryExistsAsync(c.DeityId, c.RivalDeityId, ct)) throw new DomainValidationException("This rivalry already exists.");
            await _repo.AddRivalAsync(c.DeityId, c.RivalDeityId, ct);
            return Unit.Value;
        }
    }
    public class RemoveDeityRivalCommandHandler : IRequestHandler<RemoveDeityRivalCommand, Unit>
    {
        private readonly IDeityRepository _repo;
        public RemoveDeityRivalCommandHandler(IDeityRepository repo) { _repo = repo; }
        public async Task<Unit> Handle(RemoveDeityRivalCommand c, CancellationToken ct) { await _repo.RemoveRivalAsync(c.DeityId, c.RivalDeityId, ct); return Unit.Value; }
    }
}
