using AutoMapper;
using ChronicleKeeper.Core.CQRS.JobRanks.Commands;
using ChronicleKeeper.Core.CQRS.JobRanks.Queries;
using ChronicleKeeper.Core.DTOs.JobRank;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.JobRanks.Handlers
{
    public class GetAllJobRanksQueryHandler : IRequestHandler<GetAllJobRanksQuery, List<JobRankDto>>
    {
        private readonly IJobRankRepository _repository;
        private readonly IMapper _mapper;

        public GetAllJobRanksQueryHandler(IJobRankRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<JobRankDto>> Handle(GetAllJobRanksQuery request, CancellationToken cancellationToken)
        {
            var jobRanks = await _repository.GetAllAsync(request.WorldId, request.ProfessionId, cancellationToken);
            return _mapper.Map<List<JobRankDto>>(jobRanks);
        }
    }

    public class GetJobRankByIdQueryHandler : IRequestHandler<GetJobRankByIdQuery, JobRankDto?>
    {
        private readonly IJobRankRepository _repository;
        private readonly IMapper _mapper;

        public GetJobRankByIdQueryHandler(IJobRankRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<JobRankDto?> Handle(GetJobRankByIdQuery request, CancellationToken cancellationToken)
        {
            var jobRank = await _repository.FindByIdAsync(request.Id, cancellationToken);
            return jobRank == null ? null : _mapper.Map<JobRankDto>(jobRank);
        }
    }

    public class CreateJobRankCommandHandler : IRequestHandler<CreateJobRankCommand, JobRankDto>
    {
        private readonly IJobRankRepository _repository;
        private readonly IProfessionRepository _professionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateJobRankCommandHandler> _logger;

        public CreateJobRankCommandHandler(
            IJobRankRepository repository,
            IProfessionRepository professionRepository,
            IMapper mapper,
            ILogger<CreateJobRankCommandHandler> logger)
        {
            _repository = repository;
            _professionRepository = professionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<JobRankDto> Handle(CreateJobRankCommand request, CancellationToken cancellationToken)
        {
            var dto = request.JobRankCreateDto;
            _logger.LogInformation("Creating new job rank: {Name}", dto.Name);

            var profession = await _professionRepository.FindByIdAsync(dto.ProfessionId, cancellationToken)
                ?? throw new DomainValidationException($"Profession with ID {dto.ProfessionId} does not exist.");

            var jobRank = _mapper.Map<Entities.Professions.JobRank>(dto);
            jobRank.WorldId = profession.WorldId; // svijet ranga uvijek = svijet zanimanja

            var created = await _repository.CreateAsync(jobRank, cancellationToken);
            return _mapper.Map<JobRankDto>(created);
        }
    }

    public class UpdateJobRankCommandHandler : IRequestHandler<UpdateJobRankCommand, JobRankDto>
    {
        private readonly IJobRankRepository _repository;
        private readonly IMapper _mapper;

        public UpdateJobRankCommandHandler(IJobRankRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<JobRankDto> Handle(UpdateJobRankCommand request, CancellationToken cancellationToken)
        {
            var jobRank = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("JobRank", request.Id);

            _mapper.Map(request.JobRankUpdateDto, jobRank);
            var updated = await _repository.UpdateAsync(jobRank, cancellationToken);
            return _mapper.Map<JobRankDto>(updated);
        }
    }

    public class DeleteJobRankCommandHandler : IRequestHandler<DeleteJobRankCommand, bool>
    {
        private readonly IJobRankRepository _repository;
        private readonly ILogger<DeleteJobRankCommandHandler> _logger;

        public DeleteJobRankCommandHandler(IJobRankRepository repository, ILogger<DeleteJobRankCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteJobRankCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting job rank with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
