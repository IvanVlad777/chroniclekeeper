using AutoMapper;
using ChronicleKeeper.Core.CQRS.Nations.Commands;
using ChronicleKeeper.Core.CQRS.Nations.Queries;
using ChronicleKeeper.Core.DTOs.Nation;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Nations.Handlers
{
    public class GetAllNationsQueryHandler : IRequestHandler<GetAllNationsQuery, List<NationDto>>
    {
        private readonly INationRepository _repository;
        private readonly IMapper _mapper;

        public GetAllNationsQueryHandler(INationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<NationDto>> Handle(GetAllNationsQuery request, CancellationToken cancellationToken)
        {
            var nations = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<NationDto>>(nations);
        }
    }

    public class GetNationByIdQueryHandler : IRequestHandler<GetNationByIdQuery, NationDetailsDto?>
    {
        private readonly INationRepository _repository;
        private readonly IMapper _mapper;

        public GetNationByIdQueryHandler(INationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<NationDetailsDto?> Handle(GetNationByIdQuery request, CancellationToken cancellationToken)
        {
            var nation = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return nation == null ? null : _mapper.Map<NationDetailsDto>(nation);
        }
    }

    public class CreateNationCommandHandler : IRequestHandler<CreateNationCommand, NationDto>
    {
        private readonly INationRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateNationCommandHandler> _logger;

        public CreateNationCommandHandler(
            INationRepository repository,
            IWorldRepository worldRepository,
            IMapper mapper,
            ILogger<CreateNationCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<NationDto> Handle(CreateNationCommand request, CancellationToken cancellationToken)
        {
            var dto = request.NationCreateDto;
            _logger.LogInformation("Creating new nation: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Social.Nationality.Nation>(dto), cancellationToken);
            return _mapper.Map<NationDto>(created);
        }
    }

    public class UpdateNationCommandHandler : IRequestHandler<UpdateNationCommand, NationDto>
    {
        private readonly INationRepository _repository;
        private readonly IMapper _mapper;

        public UpdateNationCommandHandler(INationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<NationDto> Handle(UpdateNationCommand request, CancellationToken cancellationToken)
        {
            var nation = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Nation", request.Id);

            _mapper.Map(request.NationUpdateDto, nation);
            var updated = await _repository.UpdateAsync(nation, cancellationToken);
            return _mapper.Map<NationDto>(updated);
        }
    }

    public class DeleteNationCommandHandler : IRequestHandler<DeleteNationCommand, bool>
    {
        private readonly INationRepository _repository;
        private readonly ILogger<DeleteNationCommandHandler> _logger;

        public DeleteNationCommandHandler(INationRepository repository, ILogger<DeleteNationCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteNationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting nation with ID {Id}", request.Id);

            var inUse = await _repository.CountCharactersUsingNationAsync(request.Id, cancellationToken);
            if (inUse > 0)
            {
                throw new DomainValidationException(
                    $"This nation is used by {inUse} character(s). Reassign them first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
