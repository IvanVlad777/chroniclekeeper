using ChronicleKeeper.Core.DTOs.CultureDetails;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.CultureDetails
{
    public class GetAllCustomsQuery : IRequest<List<CustomDto>> { public int? WorldId { get; set; } }
    public class GetCustomByIdQuery : IRequest<CustomDetailsDto?> { public int Id { get; set; } }

    public class GetAllArtFormsQuery : IRequest<List<ArtFormDto>> { public int? WorldId { get; set; } }
    public class GetArtFormByIdQuery : IRequest<ArtFormDetailsDto?> { public int Id { get; set; } }

    public class GetAllCuisinesQuery : IRequest<List<CuisineDto>> { public int? WorldId { get; set; } }
    public class GetCuisineByIdQuery : IRequest<CuisineDetailsDto?> { public int Id { get; set; } }

    public class GetAllClothingQuery : IRequest<List<ClothingDto>> { public int? WorldId { get; set; } }
    public class GetClothingByIdQuery : IRequest<ClothingDetailsDto?> { public int Id { get; set; } }

    public class GetAllTraditionsQuery : IRequest<List<TraditionDto>> { public int? WorldId { get; set; } }
    public class GetTraditionByIdQuery : IRequest<TraditionDetailsDto?> { public int Id { get; set; } }
}
