using AutoMapper;
using ChronicleKeeper.Core.DTOs.CultureDetails;
using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.CultureDetails.Handlers
{
    internal static class CultureDetailsValidation
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
    }

    // ==================== Custom ====================
    public class GetAllCustomsQueryHandler : IRequestHandler<GetAllCustomsQuery, List<CustomDto>>
    {
        private readonly ICustomRepository _repo; private readonly IMapper _mapper;
        public GetAllCustomsQueryHandler(ICustomRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<CustomDto>> Handle(GetAllCustomsQuery q, CancellationToken ct) =>
            _mapper.Map<List<CustomDto>>(await _repo.GetAllAsync(q.WorldId, ct));
    }
    public class GetCustomByIdQueryHandler : IRequestHandler<GetCustomByIdQuery, CustomDetailsDto?>
    {
        private readonly ICustomRepository _repo; private readonly IMapper _mapper;
        public GetCustomByIdQueryHandler(ICustomRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<CustomDetailsDto?> Handle(GetCustomByIdQuery q, CancellationToken ct)
        { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<CustomDetailsDto>(e); }
    }
    public class CreateCustomCommandHandler : IRequestHandler<CreateCustomCommand, CustomDto>
    {
        private readonly ICustomRepository _repo; private readonly IWorldRepository _world; private readonly IHistoryRepository _history; private readonly ICultureRepository _culture; private readonly IMapper _mapper;
        public CreateCustomCommandHandler(ICustomRepository repo, IWorldRepository world, IHistoryRepository history, ICultureRepository culture, IMapper mapper) { _repo = repo; _world = world; _history = history; _culture = culture; _mapper = mapper; }
        public async Task<CustomDto> Handle(CreateCustomCommand c, CancellationToken ct)
        {
            if (!await _world.ExistsAsync(c.Dto.WorldId, ct)) throw new DomainValidationException($"World with ID {c.Dto.WorldId} does not exist.");
            await CultureDetailsValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, c.Dto.WorldId, ct);
            await CultureDetailsValidation.ValidateCultureAsync(_culture, c.Dto.CultureId, c.Dto.WorldId, ct);
            return _mapper.Map<CustomDto>(await _repo.CreateAsync(_mapper.Map<Custom>(c.Dto), ct));
        }
    }
    public class UpdateCustomCommandHandler : IRequestHandler<UpdateCustomCommand, CustomDto>
    {
        private readonly ICustomRepository _repo; private readonly IHistoryRepository _history; private readonly ICultureRepository _culture; private readonly IMapper _mapper;
        public UpdateCustomCommandHandler(ICustomRepository repo, IHistoryRepository history, ICultureRepository culture, IMapper mapper) { _repo = repo; _history = history; _culture = culture; _mapper = mapper; }
        public async Task<CustomDto> Handle(UpdateCustomCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("Custom", c.Id);
            await CultureDetailsValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            await CultureDetailsValidation.ValidateCultureAsync(_culture, c.Dto.CultureId, e.WorldId, ct);
            _mapper.Map(c.Dto, e);
            return _mapper.Map<CustomDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteCustomCommandHandler : IRequestHandler<DeleteCustomCommand, bool>
    {
        private readonly ICustomRepository _repo;
        public DeleteCustomCommandHandler(ICustomRepository repo) { _repo = repo; }
        public Task<bool> Handle(DeleteCustomCommand c, CancellationToken ct) => _repo.DeleteAsync(c.Id, ct);
    }

    // ==================== ArtForm ====================
    public class GetAllArtFormsQueryHandler : IRequestHandler<GetAllArtFormsQuery, List<ArtFormDto>>
    {
        private readonly IArtFormRepository _repo; private readonly IMapper _mapper;
        public GetAllArtFormsQueryHandler(IArtFormRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<ArtFormDto>> Handle(GetAllArtFormsQuery q, CancellationToken ct) =>
            _mapper.Map<List<ArtFormDto>>(await _repo.GetAllAsync(q.WorldId, ct));
    }
    public class GetArtFormByIdQueryHandler : IRequestHandler<GetArtFormByIdQuery, ArtFormDetailsDto?>
    {
        private readonly IArtFormRepository _repo; private readonly IMapper _mapper;
        public GetArtFormByIdQueryHandler(IArtFormRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<ArtFormDetailsDto?> Handle(GetArtFormByIdQuery q, CancellationToken ct)
        { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<ArtFormDetailsDto>(e); }
    }
    public class CreateArtFormCommandHandler : IRequestHandler<CreateArtFormCommand, ArtFormDto>
    {
        private readonly IArtFormRepository _repo; private readonly IWorldRepository _world; private readonly IHistoryRepository _history; private readonly ICultureRepository _culture; private readonly IMapper _mapper;
        public CreateArtFormCommandHandler(IArtFormRepository repo, IWorldRepository world, IHistoryRepository history, ICultureRepository culture, IMapper mapper) { _repo = repo; _world = world; _history = history; _culture = culture; _mapper = mapper; }
        public async Task<ArtFormDto> Handle(CreateArtFormCommand c, CancellationToken ct)
        {
            if (!await _world.ExistsAsync(c.Dto.WorldId, ct)) throw new DomainValidationException($"World with ID {c.Dto.WorldId} does not exist.");
            await CultureDetailsValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, c.Dto.WorldId, ct);
            await CultureDetailsValidation.ValidateCultureAsync(_culture, c.Dto.CultureId, c.Dto.WorldId, ct);
            return _mapper.Map<ArtFormDto>(await _repo.CreateAsync(_mapper.Map<ArtForm>(c.Dto), ct));
        }
    }
    public class UpdateArtFormCommandHandler : IRequestHandler<UpdateArtFormCommand, ArtFormDto>
    {
        private readonly IArtFormRepository _repo; private readonly IHistoryRepository _history; private readonly ICultureRepository _culture; private readonly IMapper _mapper;
        public UpdateArtFormCommandHandler(IArtFormRepository repo, IHistoryRepository history, ICultureRepository culture, IMapper mapper) { _repo = repo; _history = history; _culture = culture; _mapper = mapper; }
        public async Task<ArtFormDto> Handle(UpdateArtFormCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("ArtForm", c.Id);
            await CultureDetailsValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            await CultureDetailsValidation.ValidateCultureAsync(_culture, c.Dto.CultureId, e.WorldId, ct);
            _mapper.Map(c.Dto, e);
            return _mapper.Map<ArtFormDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteArtFormCommandHandler : IRequestHandler<DeleteArtFormCommand, bool>
    {
        private readonly IArtFormRepository _repo;
        public DeleteArtFormCommandHandler(IArtFormRepository repo) { _repo = repo; }
        public Task<bool> Handle(DeleteArtFormCommand c, CancellationToken ct) => _repo.DeleteAsync(c.Id, ct);
    }

    // ==================== Cuisine ====================
    public class GetAllCuisinesQueryHandler : IRequestHandler<GetAllCuisinesQuery, List<CuisineDto>>
    {
        private readonly ICuisineRepository _repo; private readonly IMapper _mapper;
        public GetAllCuisinesQueryHandler(ICuisineRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<CuisineDto>> Handle(GetAllCuisinesQuery q, CancellationToken ct) =>
            _mapper.Map<List<CuisineDto>>(await _repo.GetAllAsync(q.WorldId, ct));
    }
    public class GetCuisineByIdQueryHandler : IRequestHandler<GetCuisineByIdQuery, CuisineDetailsDto?>
    {
        private readonly ICuisineRepository _repo; private readonly IMapper _mapper;
        public GetCuisineByIdQueryHandler(ICuisineRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<CuisineDetailsDto?> Handle(GetCuisineByIdQuery q, CancellationToken ct)
        { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<CuisineDetailsDto>(e); }
    }
    public class CreateCuisineCommandHandler : IRequestHandler<CreateCuisineCommand, CuisineDto>
    {
        private readonly ICuisineRepository _repo; private readonly IWorldRepository _world; private readonly IHistoryRepository _history; private readonly ICultureRepository _culture; private readonly IMapper _mapper;
        public CreateCuisineCommandHandler(ICuisineRepository repo, IWorldRepository world, IHistoryRepository history, ICultureRepository culture, IMapper mapper) { _repo = repo; _world = world; _history = history; _culture = culture; _mapper = mapper; }
        public async Task<CuisineDto> Handle(CreateCuisineCommand c, CancellationToken ct)
        {
            if (!await _world.ExistsAsync(c.Dto.WorldId, ct)) throw new DomainValidationException($"World with ID {c.Dto.WorldId} does not exist.");
            await CultureDetailsValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, c.Dto.WorldId, ct);
            await CultureDetailsValidation.ValidateCultureAsync(_culture, c.Dto.CultureId, c.Dto.WorldId, ct);
            return _mapper.Map<CuisineDto>(await _repo.CreateAsync(_mapper.Map<Cuisine>(c.Dto), ct));
        }
    }
    public class UpdateCuisineCommandHandler : IRequestHandler<UpdateCuisineCommand, CuisineDto>
    {
        private readonly ICuisineRepository _repo; private readonly IHistoryRepository _history; private readonly ICultureRepository _culture; private readonly IMapper _mapper;
        public UpdateCuisineCommandHandler(ICuisineRepository repo, IHistoryRepository history, ICultureRepository culture, IMapper mapper) { _repo = repo; _history = history; _culture = culture; _mapper = mapper; }
        public async Task<CuisineDto> Handle(UpdateCuisineCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("Cuisine", c.Id);
            await CultureDetailsValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            await CultureDetailsValidation.ValidateCultureAsync(_culture, c.Dto.CultureId, e.WorldId, ct);
            _mapper.Map(c.Dto, e);
            return _mapper.Map<CuisineDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteCuisineCommandHandler : IRequestHandler<DeleteCuisineCommand, bool>
    {
        private readonly ICuisineRepository _repo;
        public DeleteCuisineCommandHandler(ICuisineRepository repo) { _repo = repo; }
        public Task<bool> Handle(DeleteCuisineCommand c, CancellationToken ct) => _repo.DeleteAsync(c.Id, ct);
    }

    // ==================== Clothing ====================
    public class GetAllClothingQueryHandler : IRequestHandler<GetAllClothingQuery, List<ClothingDto>>
    {
        private readonly IClothingRepository _repo; private readonly IMapper _mapper;
        public GetAllClothingQueryHandler(IClothingRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<ClothingDto>> Handle(GetAllClothingQuery q, CancellationToken ct) =>
            _mapper.Map<List<ClothingDto>>(await _repo.GetAllAsync(q.WorldId, ct));
    }
    public class GetClothingByIdQueryHandler : IRequestHandler<GetClothingByIdQuery, ClothingDetailsDto?>
    {
        private readonly IClothingRepository _repo; private readonly IMapper _mapper;
        public GetClothingByIdQueryHandler(IClothingRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<ClothingDetailsDto?> Handle(GetClothingByIdQuery q, CancellationToken ct)
        { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<ClothingDetailsDto>(e); }
    }
    public class CreateClothingCommandHandler : IRequestHandler<CreateClothingCommand, ClothingDto>
    {
        private readonly IClothingRepository _repo; private readonly IWorldRepository _world; private readonly IHistoryRepository _history; private readonly ICultureRepository _culture; private readonly IMapper _mapper;
        public CreateClothingCommandHandler(IClothingRepository repo, IWorldRepository world, IHistoryRepository history, ICultureRepository culture, IMapper mapper) { _repo = repo; _world = world; _history = history; _culture = culture; _mapper = mapper; }
        public async Task<ClothingDto> Handle(CreateClothingCommand c, CancellationToken ct)
        {
            if (!await _world.ExistsAsync(c.Dto.WorldId, ct)) throw new DomainValidationException($"World with ID {c.Dto.WorldId} does not exist.");
            await CultureDetailsValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, c.Dto.WorldId, ct);
            await CultureDetailsValidation.ValidateCultureAsync(_culture, c.Dto.CultureId, c.Dto.WorldId, ct);
            return _mapper.Map<ClothingDto>(await _repo.CreateAsync(_mapper.Map<Clothing>(c.Dto), ct));
        }
    }
    public class UpdateClothingCommandHandler : IRequestHandler<UpdateClothingCommand, ClothingDto>
    {
        private readonly IClothingRepository _repo; private readonly IHistoryRepository _history; private readonly ICultureRepository _culture; private readonly IMapper _mapper;
        public UpdateClothingCommandHandler(IClothingRepository repo, IHistoryRepository history, ICultureRepository culture, IMapper mapper) { _repo = repo; _history = history; _culture = culture; _mapper = mapper; }
        public async Task<ClothingDto> Handle(UpdateClothingCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("Clothing", c.Id);
            await CultureDetailsValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            await CultureDetailsValidation.ValidateCultureAsync(_culture, c.Dto.CultureId, e.WorldId, ct);
            _mapper.Map(c.Dto, e);
            return _mapper.Map<ClothingDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteClothingCommandHandler : IRequestHandler<DeleteClothingCommand, bool>
    {
        private readonly IClothingRepository _repo;
        public DeleteClothingCommandHandler(IClothingRepository repo) { _repo = repo; }
        public Task<bool> Handle(DeleteClothingCommand c, CancellationToken ct) => _repo.DeleteAsync(c.Id, ct);
    }

    // ==================== Tradition ====================
    public class GetAllTraditionsQueryHandler : IRequestHandler<GetAllTraditionsQuery, List<TraditionDto>>
    {
        private readonly ITraditionRepository _repo; private readonly IMapper _mapper;
        public GetAllTraditionsQueryHandler(ITraditionRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<List<TraditionDto>> Handle(GetAllTraditionsQuery q, CancellationToken ct) =>
            _mapper.Map<List<TraditionDto>>(await _repo.GetAllAsync(q.WorldId, ct));
    }
    public class GetTraditionByIdQueryHandler : IRequestHandler<GetTraditionByIdQuery, TraditionDetailsDto?>
    {
        private readonly ITraditionRepository _repo; private readonly IMapper _mapper;
        public GetTraditionByIdQueryHandler(ITraditionRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<TraditionDetailsDto?> Handle(GetTraditionByIdQuery q, CancellationToken ct)
        { var e = await _repo.GetByIdAsync(q.Id, ct); return e == null ? null : _mapper.Map<TraditionDetailsDto>(e); }
    }
    public class CreateTraditionCommandHandler : IRequestHandler<CreateTraditionCommand, TraditionDto>
    {
        private readonly ITraditionRepository _repo; private readonly IWorldRepository _world; private readonly IHistoryRepository _history; private readonly ICultureRepository _culture; private readonly IReligionRepository _religion; private readonly IMapper _mapper;
        public CreateTraditionCommandHandler(ITraditionRepository repo, IWorldRepository world, IHistoryRepository history, ICultureRepository culture, IReligionRepository religion, IMapper mapper) { _repo = repo; _world = world; _history = history; _culture = culture; _religion = religion; _mapper = mapper; }
        public async Task<TraditionDto> Handle(CreateTraditionCommand c, CancellationToken ct)
        {
            if (!await _world.ExistsAsync(c.Dto.WorldId, ct)) throw new DomainValidationException($"World with ID {c.Dto.WorldId} does not exist.");
            await CultureDetailsValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, c.Dto.WorldId, ct);
            await CultureDetailsValidation.ValidateCultureAsync(_culture, c.Dto.CultureId, c.Dto.WorldId, ct);
            await CultureDetailsValidation.ValidateReligionAsync(_religion, c.Dto.ReligionId, c.Dto.WorldId, ct);
            return _mapper.Map<TraditionDto>(await _repo.CreateAsync(_mapper.Map<Tradition>(c.Dto), ct));
        }
    }
    public class UpdateTraditionCommandHandler : IRequestHandler<UpdateTraditionCommand, TraditionDto>
    {
        private readonly ITraditionRepository _repo; private readonly IHistoryRepository _history; private readonly ICultureRepository _culture; private readonly IReligionRepository _religion; private readonly IMapper _mapper;
        public UpdateTraditionCommandHandler(ITraditionRepository repo, IHistoryRepository history, ICultureRepository culture, IReligionRepository religion, IMapper mapper) { _repo = repo; _history = history; _culture = culture; _religion = religion; _mapper = mapper; }
        public async Task<TraditionDto> Handle(UpdateTraditionCommand c, CancellationToken ct)
        {
            var e = await _repo.FindByIdAsync(c.Id, ct) ?? throw new EntityNotFoundException("Tradition", c.Id);
            await CultureDetailsValidation.ValidateHistoryAsync(_history, c.Dto.HistoryId, e.WorldId, ct);
            await CultureDetailsValidation.ValidateCultureAsync(_culture, c.Dto.CultureId, e.WorldId, ct);
            await CultureDetailsValidation.ValidateReligionAsync(_religion, c.Dto.ReligionId, e.WorldId, ct);
            _mapper.Map(c.Dto, e);
            return _mapper.Map<TraditionDto>(await _repo.UpdateAsync(e, ct));
        }
    }
    public class DeleteTraditionCommandHandler : IRequestHandler<DeleteTraditionCommand, bool>
    {
        private readonly ITraditionRepository _repo;
        public DeleteTraditionCommandHandler(ITraditionRepository repo) { _repo = repo; }
        public Task<bool> Handle(DeleteTraditionCommand c, CancellationToken ct) => _repo.DeleteAsync(c.Id, ct);
    }
}
