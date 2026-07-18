using ChronicleKeeper.Core.DTOs.CultureDetails;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.CultureDetails
{
    // Custom
    public class CreateCustomCommand : IRequest<CustomDto> { public CustomCreateDto Dto { get; set; } = new(); }
    public class UpdateCustomCommand : IRequest<CustomDto> { public int Id { get; set; } public CustomUpdateDto Dto { get; set; } = new(); }
    public class DeleteCustomCommand : IRequest<bool> { public int Id { get; set; } }

    // ArtForm
    public class CreateArtFormCommand : IRequest<ArtFormDto> { public ArtFormCreateDto Dto { get; set; } = new(); }
    public class UpdateArtFormCommand : IRequest<ArtFormDto> { public int Id { get; set; } public ArtFormUpdateDto Dto { get; set; } = new(); }
    public class DeleteArtFormCommand : IRequest<bool> { public int Id { get; set; } }

    // Cuisine
    public class CreateCuisineCommand : IRequest<CuisineDto> { public CuisineCreateDto Dto { get; set; } = new(); }
    public class UpdateCuisineCommand : IRequest<CuisineDto> { public int Id { get; set; } public CuisineUpdateDto Dto { get; set; } = new(); }
    public class DeleteCuisineCommand : IRequest<bool> { public int Id { get; set; } }

    // Clothing
    public class CreateClothingCommand : IRequest<ClothingDto> { public ClothingCreateDto Dto { get; set; } = new(); }
    public class UpdateClothingCommand : IRequest<ClothingDto> { public int Id { get; set; } public ClothingUpdateDto Dto { get; set; } = new(); }
    public class DeleteClothingCommand : IRequest<bool> { public int Id { get; set; } }

    // Tradition
    public class CreateTraditionCommand : IRequest<TraditionDto> { public TraditionCreateDto Dto { get; set; } = new(); }
    public class UpdateTraditionCommand : IRequest<TraditionDto> { public int Id { get; set; } public TraditionUpdateDto Dto { get; set; } = new(); }
    public class DeleteTraditionCommand : IRequest<bool> { public int Id { get; set; } }
}
