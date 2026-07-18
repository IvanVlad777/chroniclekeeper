using ChronicleKeeper.Core.CQRS.CultureDetails;
using ChronicleKeeper.Core.DTOs.CultureDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChronicleKeeperAPI.Controllers
{
    [ApiController]
    [Route("api/customs")]
    public class CustomController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CustomController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<CustomDto>>> GetAll([FromQuery] int? worldId = null) =>
            Ok(await _mediator.Send(new GetAllCustomsQuery { WorldId = worldId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<CustomDetailsDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetCustomByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<CustomDto>> Create([FromBody] CustomCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateCustomCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<CustomDto>> Update(int id, [FromBody] CustomUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateCustomCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteCustomCommand { Id = id }) ? NoContent() : NotFound();
    }

    [ApiController]
    [Route("api/art-forms")]
    public class ArtFormController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ArtFormController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<ArtFormDto>>> GetAll([FromQuery] int? worldId = null) =>
            Ok(await _mediator.Send(new GetAllArtFormsQuery { WorldId = worldId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<ArtFormDetailsDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetArtFormByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<ArtFormDto>> Create([FromBody] ArtFormCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateArtFormCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<ArtFormDto>> Update(int id, [FromBody] ArtFormUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateArtFormCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteArtFormCommand { Id = id }) ? NoContent() : NotFound();
    }

    [ApiController]
    [Route("api/cuisines")]
    public class CuisineController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CuisineController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<CuisineDto>>> GetAll([FromQuery] int? worldId = null) =>
            Ok(await _mediator.Send(new GetAllCuisinesQuery { WorldId = worldId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<CuisineDetailsDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetCuisineByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<CuisineDto>> Create([FromBody] CuisineCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateCuisineCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<CuisineDto>> Update(int id, [FromBody] CuisineUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateCuisineCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteCuisineCommand { Id = id }) ? NoContent() : NotFound();
    }

    [ApiController]
    [Route("api/clothing")]
    public class ClothingController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ClothingController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<ClothingDto>>> GetAll([FromQuery] int? worldId = null) =>
            Ok(await _mediator.Send(new GetAllClothingQuery { WorldId = worldId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<ClothingDetailsDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetClothingByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<ClothingDto>> Create([FromBody] ClothingCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateClothingCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<ClothingDto>> Update(int id, [FromBody] ClothingUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateClothingCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteClothingCommand { Id = id }) ? NoContent() : NotFound();
    }

    [ApiController]
    [Route("api/traditions")]
    public class TraditionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TraditionController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<TraditionDto>>> GetAll([FromQuery] int? worldId = null) =>
            Ok(await _mediator.Send(new GetAllTraditionsQuery { WorldId = worldId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<TraditionDetailsDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetTraditionByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<TraditionDto>> Create([FromBody] TraditionCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateTraditionCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<TraditionDto>> Update(int id, [FromBody] TraditionUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateTraditionCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteTraditionCommand { Id = id }) ? NoContent() : NotFound();
    }
}
