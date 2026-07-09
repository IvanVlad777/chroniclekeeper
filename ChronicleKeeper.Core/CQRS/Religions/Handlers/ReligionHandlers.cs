using AutoMapper;
using ChronicleKeeper.Core.CQRS.Religions.Commands;
using ChronicleKeeper.Core.CQRS.Religions.Queries;
using ChronicleKeeper.Core.DTOs.Religion;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Religions.Handlers
{
    public class GetAllReligionsQueryHandler : IRequestHandler<GetAllReligionsQuery, List<ReligionDto>>
    {
        private readonly IReligionRepository _repository;
        private readonly IMapper _mapper;

        public GetAllReligionsQueryHandler(IReligionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ReligionDto>> Handle(GetAllReligionsQuery request, CancellationToken cancellationToken)
        {
            var religions = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<ReligionDto>>(religions);
        }
    }

    public class GetReligionByIdQueryHandler : IRequestHandler<GetReligionByIdQuery, ReligionDetailsDto?>
    {
        private readonly IReligionRepository _repository;
        private readonly IMapper _mapper;

        public GetReligionByIdQueryHandler(IReligionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ReligionDetailsDto?> Handle(GetReligionByIdQuery request, CancellationToken cancellationToken)
        {
            var religion = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return religion == null ? null : _mapper.Map<ReligionDetailsDto>(religion);
        }
    }

    public class CreateReligionCommandHandler : IRequestHandler<CreateReligionCommand, ReligionDto>
    {
        private readonly IReligionRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateReligionCommandHandler> _logger;

        public CreateReligionCommandHandler(
            IReligionRepository repository,
            IWorldRepository worldRepository,
            IMapper mapper,
            ILogger<CreateReligionCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReligionDto> Handle(CreateReligionCommand request, CancellationToken cancellationToken)
        {
            var dto = request.ReligionCreateDto;
            _logger.LogInformation("Creating new religion: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Social.Religions.Religion>(dto), cancellationToken);
            return _mapper.Map<ReligionDto>(created);
        }
    }

    public class UpdateReligionCommandHandler : IRequestHandler<UpdateReligionCommand, ReligionDto>
    {
        private readonly IReligionRepository _repository;
        private readonly IMapper _mapper;

        public UpdateReligionCommandHandler(IReligionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ReligionDto> Handle(UpdateReligionCommand request, CancellationToken cancellationToken)
        {
            var religion = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Religion", request.Id);

            _mapper.Map(request.ReligionUpdateDto, religion);
            var updated = await _repository.UpdateAsync(religion, cancellationToken);
            return _mapper.Map<ReligionDto>(updated);
        }
    }

    public class DeleteReligionCommandHandler : IRequestHandler<DeleteReligionCommand, bool>
    {
        private readonly IReligionRepository _repository;
        private readonly ILogger<DeleteReligionCommandHandler> _logger;

        public DeleteReligionCommandHandler(IReligionRepository repository, ILogger<DeleteReligionCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteReligionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting religion with ID {Id}", request.Id);

            var charactersInUse = await _repository.CountCharactersUsingReligionAsync(request.Id, cancellationToken);
            if (charactersInUse > 0)
            {
                throw new DomainValidationException(
                    $"This religion is followed by {charactersInUse} character(s). Reassign them first.");
            }

            var culturesInUse = await _repository.CountCulturesUsingReligionAsync(request.Id, cancellationToken);
            if (culturesInUse > 0)
            {
                throw new DomainValidationException(
                    $"This religion is used by {culturesInUse} culture(s). Reassign them first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
