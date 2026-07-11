using AutoMapper;
using ChronicleKeeper.Core.CQRS.Specialisations.Commands;
using ChronicleKeeper.Core.CQRS.Specialisations.Queries;
using ChronicleKeeper.Core.DTOs.Specialisation;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Specialisations.Handlers
{
    public class GetAllSpecialisationsQueryHandler : IRequestHandler<GetAllSpecialisationsQuery, List<SpecialisationDto>>
    {
        private readonly ISpecialisationRepository _repository;
        private readonly IMapper _mapper;

        public GetAllSpecialisationsQueryHandler(ISpecialisationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<SpecialisationDto>> Handle(GetAllSpecialisationsQuery request, CancellationToken cancellationToken)
        {
            var specialisations = await _repository.GetAllAsync(request.WorldId, request.ProfessionId, cancellationToken);
            return _mapper.Map<List<SpecialisationDto>>(specialisations);
        }
    }

    public class GetSpecialisationByIdQueryHandler : IRequestHandler<GetSpecialisationByIdQuery, SpecialisationDto?>
    {
        private readonly ISpecialisationRepository _repository;
        private readonly IMapper _mapper;

        public GetSpecialisationByIdQueryHandler(ISpecialisationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SpecialisationDto?> Handle(GetSpecialisationByIdQuery request, CancellationToken cancellationToken)
        {
            var specialisation = await _repository.FindByIdAsync(request.Id, cancellationToken);
            return specialisation == null ? null : _mapper.Map<SpecialisationDto>(specialisation);
        }
    }

    public class CreateSpecialisationCommandHandler : IRequestHandler<CreateSpecialisationCommand, SpecialisationDto>
    {
        private readonly ISpecialisationRepository _repository;
        private readonly IProfessionRepository _professionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSpecialisationCommandHandler> _logger;

        public CreateSpecialisationCommandHandler(
            ISpecialisationRepository repository,
            IProfessionRepository professionRepository,
            IMapper mapper,
            ILogger<CreateSpecialisationCommandHandler> logger)
        {
            _repository = repository;
            _professionRepository = professionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SpecialisationDto> Handle(CreateSpecialisationCommand request, CancellationToken cancellationToken)
        {
            var dto = request.SpecialisationCreateDto;
            _logger.LogInformation("Creating new specialisation: {Name}", dto.Name);

            var profession = await _professionRepository.FindByIdAsync(dto.ProfessionId, cancellationToken)
                ?? throw new DomainValidationException($"Profession with ID {dto.ProfessionId} does not exist.");

            var specialisation = _mapper.Map<Entities.Professions.Specialisation>(dto);
            specialisation.WorldId = profession.WorldId; // svijet specijalizacije uvijek = svijet zanimanja

            var created = await _repository.CreateAsync(specialisation, cancellationToken);
            return _mapper.Map<SpecialisationDto>(created);
        }
    }

    public class UpdateSpecialisationCommandHandler : IRequestHandler<UpdateSpecialisationCommand, SpecialisationDto>
    {
        private readonly ISpecialisationRepository _repository;
        private readonly IMapper _mapper;

        public UpdateSpecialisationCommandHandler(ISpecialisationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SpecialisationDto> Handle(UpdateSpecialisationCommand request, CancellationToken cancellationToken)
        {
            var specialisation = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Specialisation", request.Id);

            _mapper.Map(request.SpecialisationUpdateDto, specialisation);
            var updated = await _repository.UpdateAsync(specialisation, cancellationToken);
            return _mapper.Map<SpecialisationDto>(updated);
        }
    }

    public class DeleteSpecialisationCommandHandler : IRequestHandler<DeleteSpecialisationCommand, bool>
    {
        private readonly ISpecialisationRepository _repository;
        private readonly ILogger<DeleteSpecialisationCommandHandler> _logger;

        public DeleteSpecialisationCommandHandler(ISpecialisationRepository repository, ILogger<DeleteSpecialisationCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteSpecialisationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting specialisation with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
