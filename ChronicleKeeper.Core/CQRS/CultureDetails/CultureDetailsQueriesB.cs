using ChronicleKeeper.Core.DTOs.CultureDetails;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.CultureDetails
{
    public class GetAllArchitectureStylesQuery : IRequest<List<ArchitectureStyleDto>> { public int? WorldId { get; set; } }
    public class GetArchitectureStyleByIdQuery : IRequest<ArchitectureStyleDetailsDto?> { public int Id { get; set; } }

    public class GetAllFolkloreQuery : IRequest<List<FolkloreDto>> { public int? WorldId { get; set; } }
    public class GetFolkloreByIdQuery : IRequest<FolkloreDetailsDto?> { public int Id { get; set; } }

    public class GetAllMythsQuery : IRequest<List<MythDto>> { public int? WorldId { get; set; } }
    public class GetMythByIdQuery : IRequest<MythDetailsDto?> { public int Id { get; set; } }

    public class GetAllCulturalFestivalsQuery : IRequest<List<CulturalFestivalDto>> { public int? WorldId { get; set; } }
    public class GetCulturalFestivalByIdQuery : IRequest<CulturalFestivalDetailsDto?> { public int Id { get; set; } }

    public class GetAllCulturalInstitutionsQuery : IRequest<List<CulturalInstitutionDto>> { public int? WorldId { get; set; } }
    public class GetCulturalInstitutionByIdQuery : IRequest<CulturalInstitutionDetailsDto?> { public int Id { get; set; } }
}
