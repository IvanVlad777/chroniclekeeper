using AutoMapper;
using ChronicleKeeper.Core.CQRS.TradeSchools.Commands;
using ChronicleKeeper.Core.CQRS.TradeSchools.Queries;
using ChronicleKeeper.Core.DTOs.TradeSchool;
using ChronicleKeeper.Core.Entities.Professions;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.TradeSchools.Handlers
{
    public class GetAllTradeSchoolsQueryHandler : IRequestHandler<GetAllTradeSchoolsQuery, List<TradeSchoolDto>>
    {
        private readonly ITradeSchoolRepository _repository;
        private readonly IMapper _mapper;

        public GetAllTradeSchoolsQueryHandler(ITradeSchoolRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<TradeSchoolDto>> Handle(GetAllTradeSchoolsQuery request, CancellationToken cancellationToken)
        {
            var tradeSchools = await _repository.GetAllAsync(request.WorldId, request.EducationSystemId, cancellationToken);
            return _mapper.Map<List<TradeSchoolDto>>(tradeSchools);
        }
    }

    public class GetTradeSchoolByIdQueryHandler : IRequestHandler<GetTradeSchoolByIdQuery, TradeSchoolDetailsDto?>
    {
        private readonly ITradeSchoolRepository _repository;
        private readonly IMapper _mapper;

        public GetTradeSchoolByIdQueryHandler(ITradeSchoolRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TradeSchoolDetailsDto?> Handle(GetTradeSchoolByIdQuery request, CancellationToken cancellationToken)
        {
            var tradeSchool = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return tradeSchool == null ? null : _mapper.Map<TradeSchoolDetailsDto>(tradeSchool);
        }
    }

    public class CreateTradeSchoolCommandHandler : IRequestHandler<CreateTradeSchoolCommand, TradeSchoolDto>
    {
        private readonly ITradeSchoolRepository _repository;
        private readonly IEducationSystemRepository _educationSystemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTradeSchoolCommandHandler> _logger;

        public CreateTradeSchoolCommandHandler(
            ITradeSchoolRepository repository,
            IEducationSystemRepository educationSystemRepository,
            IMapper mapper,
            ILogger<CreateTradeSchoolCommandHandler> logger)
        {
            _repository = repository;
            _educationSystemRepository = educationSystemRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TradeSchoolDto> Handle(CreateTradeSchoolCommand request, CancellationToken cancellationToken)
        {
            var dto = request.TradeSchoolCreateDto;
            _logger.LogInformation("Creating new trade school: {Name}", dto.Name);

            var educationSystem = await _educationSystemRepository.FindByIdAsync(dto.EducationSystemId, cancellationToken)
                ?? throw new DomainValidationException($"Education system with ID {dto.EducationSystemId} does not exist.");

            var tradeSchool = _mapper.Map<TradeSchool>(dto);
            tradeSchool.WorldId = educationSystem.WorldId; // svijet strukovne škole uvijek = svijet sustava obrazovanja

            var created = await _repository.CreateAsync(tradeSchool, cancellationToken);
            return _mapper.Map<TradeSchoolDto>(created);
        }
    }

    public class UpdateTradeSchoolCommandHandler : IRequestHandler<UpdateTradeSchoolCommand, TradeSchoolDto>
    {
        private readonly ITradeSchoolRepository _repository;
        private readonly IMapper _mapper;

        public UpdateTradeSchoolCommandHandler(ITradeSchoolRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TradeSchoolDto> Handle(UpdateTradeSchoolCommand request, CancellationToken cancellationToken)
        {
            var tradeSchool = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("TradeSchool", request.Id);

            _mapper.Map(request.TradeSchoolUpdateDto, tradeSchool);
            var updated = await _repository.UpdateAsync(tradeSchool, cancellationToken);
            return _mapper.Map<TradeSchoolDto>(updated);
        }
    }

    public class DeleteTradeSchoolCommandHandler : IRequestHandler<DeleteTradeSchoolCommand, bool>
    {
        private readonly ITradeSchoolRepository _repository;
        private readonly ILogger<DeleteTradeSchoolCommandHandler> _logger;

        public DeleteTradeSchoolCommandHandler(ITradeSchoolRepository repository, ILogger<DeleteTradeSchoolCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteTradeSchoolCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting trade school with ID {Id}", request.Id);

            var apprenticeshipsInUse = await _repository.CountApprenticeshipsUsingTradeSchoolAsync(request.Id, cancellationToken);
            if (apprenticeshipsInUse > 0)
            {
                throw new DomainValidationException(
                    $"This trade school is used by {apprenticeshipsInUse} apprenticeship(s). Remove or reassign them first.");
            }

            var educationRecordsInUse = await _repository.CountEducationRecordsUsingSchoolAsync(request.Id, cancellationToken);
            if (educationRecordsInUse > 0)
            {
                throw new DomainValidationException(
                    $"This school is used by {educationRecordsInUse} education record(s). Remove them first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
