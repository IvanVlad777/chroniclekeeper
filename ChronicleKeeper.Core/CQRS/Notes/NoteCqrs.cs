using AutoMapper;
using ChronicleKeeper.Core.DTOs.Note;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using NoteEntity = ChronicleKeeper.Core.Entities.Notes.Note;

namespace ChronicleKeeper.Core.CQRS.Notes
{
    // ----- Queries -----

    public class GetAllNotesQuery : IRequest<List<NoteDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetNoteByIdQuery : IRequest<NoteDto?>
    {
        public int Id { get; set; }
    }

    // ----- Commands -----

    public class CreateNoteCommand : IRequest<NoteDto>
    {
        public NoteCreateDto NoteCreateDto { get; set; } = new();
    }

    public class UpdateNoteCommand : IRequest<NoteDto>
    {
        public int Id { get; set; }
        public NoteUpdateDto NoteUpdateDto { get; set; } = new();
    }

    public class DeleteNoteCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    // ----- Handlers -----

    public class GetAllNotesQueryHandler : IRequestHandler<GetAllNotesQuery, List<NoteDto>>
    {
        private readonly INoteRepository _repository;
        private readonly IMapper _mapper;

        public GetAllNotesQueryHandler(INoteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<NoteDto>> Handle(GetAllNotesQuery request, CancellationToken cancellationToken)
        {
            var notes = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<NoteDto>>(notes);
        }
    }

    public class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdQuery, NoteDto?>
    {
        private readonly INoteRepository _repository;
        private readonly IMapper _mapper;

        public GetNoteByIdQueryHandler(INoteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<NoteDto?> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
        {
            var note = await _repository.FindByIdAsync(request.Id, cancellationToken);
            return note == null ? null : _mapper.Map<NoteDto>(note);
        }
    }

    public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, NoteDto>
    {
        private readonly INoteRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateNoteCommandHandler> _logger;

        public CreateNoteCommandHandler(
            INoteRepository repository,
            IWorldRepository worldRepository,
            IMapper mapper,
            ILogger<CreateNoteCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<NoteDto> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
        {
            var dto = request.NoteCreateDto;
            _logger.LogInformation("Creating new note: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            var created = await _repository.CreateAsync(_mapper.Map<NoteEntity>(dto), cancellationToken);
            return _mapper.Map<NoteDto>(created);
        }
    }

    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, NoteDto>
    {
        private readonly INoteRepository _repository;
        private readonly IMapper _mapper;

        public UpdateNoteCommandHandler(INoteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<NoteDto> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
        {
            var note = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Note", request.Id);

            _mapper.Map(request.NoteUpdateDto, note);
            var updated = await _repository.UpdateAsync(note, cancellationToken);
            return _mapper.Map<NoteDto>(updated);
        }
    }

    public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, bool>
    {
        private readonly INoteRepository _repository;

        public DeleteNoteCommandHandler(INoteRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
