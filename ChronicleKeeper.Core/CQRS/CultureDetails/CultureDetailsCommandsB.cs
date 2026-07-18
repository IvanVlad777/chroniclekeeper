using ChronicleKeeper.Core.DTOs.CultureDetails;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.CultureDetails
{
    // ArchitectureStyle
    public class CreateArchitectureStyleCommand : IRequest<ArchitectureStyleDto> { public ArchitectureStyleCreateDto Dto { get; set; } = new(); }
    public class UpdateArchitectureStyleCommand : IRequest<ArchitectureStyleDto> { public int Id { get; set; } public ArchitectureStyleUpdateDto Dto { get; set; } = new(); }
    public class DeleteArchitectureStyleCommand : IRequest<bool> { public int Id { get; set; } }
    public class AddArchitectureStyleLocationCommand : IRequest<Unit> { public int ArchitectureStyleId { get; set; } public int LocationId { get; set; } }
    public class RemoveArchitectureStyleLocationCommand : IRequest<Unit> { public int ArchitectureStyleId { get; set; } public int LocationId { get; set; } }

    // Folklore
    public class CreateFolkloreCommand : IRequest<FolkloreDto> { public FolkloreCreateDto Dto { get; set; } = new(); }
    public class UpdateFolkloreCommand : IRequest<FolkloreDto> { public int Id { get; set; } public FolkloreUpdateDto Dto { get; set; } = new(); }
    public class DeleteFolkloreCommand : IRequest<bool> { public int Id { get; set; } }
    public class AddFolkloreEventCommand : IRequest<Unit> { public int FolkloreId { get; set; } public int EventId { get; set; } }
    public class RemoveFolkloreEventCommand : IRequest<Unit> { public int FolkloreId { get; set; } public int EventId { get; set; } }
    public class AddFolkloreSpeciesCommand : IRequest<Unit> { public int FolkloreId { get; set; } public int SpeciesId { get; set; } }
    public class RemoveFolkloreSpeciesCommand : IRequest<Unit> { public int FolkloreId { get; set; } public int SpeciesId { get; set; } }

    // Myth
    public class CreateMythCommand : IRequest<MythDto> { public MythCreateDto Dto { get; set; } = new(); }
    public class UpdateMythCommand : IRequest<MythDto> { public int Id { get; set; } public MythUpdateDto Dto { get; set; } = new(); }
    public class DeleteMythCommand : IRequest<bool> { public int Id { get; set; } }

    // CulturalFestival
    public class CreateCulturalFestivalCommand : IRequest<CulturalFestivalDto> { public CulturalFestivalCreateDto Dto { get; set; } = new(); }
    public class UpdateCulturalFestivalCommand : IRequest<CulturalFestivalDto> { public int Id { get; set; } public CulturalFestivalUpdateDto Dto { get; set; } = new(); }
    public class DeleteCulturalFestivalCommand : IRequest<bool> { public int Id { get; set; } }

    // CulturalInstitution
    public class CreateCulturalInstitutionCommand : IRequest<CulturalInstitutionDto> { public CulturalInstitutionCreateDto Dto { get; set; } = new(); }
    public class UpdateCulturalInstitutionCommand : IRequest<CulturalInstitutionDto> { public int Id { get; set; } public CulturalInstitutionUpdateDto Dto { get; set; } = new(); }
    public class DeleteCulturalInstitutionCommand : IRequest<bool> { public int Id { get; set; } }
    public class AddCulturalInstitutionArtistCommand : IRequest<bool> { public int InstitutionId { get; set; } public int CharacterId { get; set; } }
    public class RemoveCulturalInstitutionArtistCommand : IRequest<bool> { public int InstitutionId { get; set; } public int CharacterId { get; set; } }
}
