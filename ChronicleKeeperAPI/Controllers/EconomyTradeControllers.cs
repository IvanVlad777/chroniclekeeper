using ChronicleKeeper.Core.CQRS.ExtractionMethods.Commands;
using ChronicleKeeper.Core.CQRS.ExtractionMethods.Queries;
using ChronicleKeeper.Core.CQRS.Industries.Commands;
using ChronicleKeeper.Core.CQRS.Industries.Queries;
using ChronicleKeeper.Core.CQRS.NaturalResources.Commands;
using ChronicleKeeper.Core.CQRS.NaturalResources.Queries;
using ChronicleKeeper.Core.CQRS.TradeRoutes.Commands;
using ChronicleKeeper.Core.CQRS.TradeRoutes.Queries;
using ChronicleKeeper.Core.DTOs.ExtractionMethod;
using ChronicleKeeper.Core.DTOs.Industry;
using ChronicleKeeper.Core.DTOs.NaturalResource;
using ChronicleKeeper.Core.DTOs.TradeRoute;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/industries")]
    [ApiController]
    public class IndustryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IndustryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get industries", Description = "Returns industries, optionally filtered by world")]
        [SwaggerResponse(200, "List of industries", typeof(IEnumerable<IndustryDto>))]
        public async Task<ActionResult<IEnumerable<IndustryDto>>> GetAll([FromQuery] int? worldId = null)
        {
            return Ok(await _mediator.Send(new GetAllIndustriesQuery { WorldId = worldId }));
        }

        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get industry by ID")]
        [SwaggerResponse(200, "Industry found", typeof(IndustryDetailsDto))]
        [SwaggerResponse(404, "Industry not found")]
        public async Task<ActionResult<IndustryDetailsDto>> GetById(int id)
        {
            var industry = await _mediator.Send(new GetIndustryByIdQuery { Id = id });
            if (industry == null) return NotFound();
            return Ok(industry);
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new industry")]
        [SwaggerResponse(201, "Industry created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<IndustryDto>> Create([FromBody] IndustryCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateIndustryCommand { IndustryCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update industry by ID")]
        [SwaggerResponse(200, "Industry updated", typeof(IndustryDto))]
        [SwaggerResponse(404, "Industry not found")]
        public async Task<ActionResult<IndustryDto>> Update(int id, [FromBody] IndustryUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateIndustryCommand { Id = id, IndustryUpdateDto = dto });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete industry by ID")]
        [SwaggerResponse(204, "Industry deleted")]
        [SwaggerResponse(404, "Industry not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteIndustryCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }

    [Route("api/extraction-methods")]
    [ApiController]
    public class ExtractionMethodController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExtractionMethodController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get extraction methods", Description = "Returns extraction methods, optionally filtered by world")]
        [SwaggerResponse(200, "List of extraction methods", typeof(IEnumerable<ExtractionMethodDto>))]
        public async Task<ActionResult<IEnumerable<ExtractionMethodDto>>> GetAll([FromQuery] int? worldId = null)
        {
            return Ok(await _mediator.Send(new GetAllExtractionMethodsQuery { WorldId = worldId }));
        }

        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get extraction method by ID")]
        [SwaggerResponse(200, "Extraction method found", typeof(ExtractionMethodDetailsDto))]
        [SwaggerResponse(404, "Extraction method not found")]
        public async Task<ActionResult<ExtractionMethodDetailsDto>> GetById(int id)
        {
            var extractionMethod = await _mediator.Send(new GetExtractionMethodByIdQuery { Id = id });
            if (extractionMethod == null) return NotFound();
            return Ok(extractionMethod);
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new extraction method")]
        [SwaggerResponse(201, "Extraction method created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<ExtractionMethodDto>> Create([FromBody] ExtractionMethodCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateExtractionMethodCommand { ExtractionMethodCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update extraction method by ID")]
        [SwaggerResponse(200, "Extraction method updated", typeof(ExtractionMethodDto))]
        [SwaggerResponse(404, "Extraction method not found")]
        public async Task<ActionResult<ExtractionMethodDto>> Update(int id, [FromBody] ExtractionMethodUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateExtractionMethodCommand { Id = id, ExtractionMethodUpdateDto = dto });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete extraction method by ID")]
        [SwaggerResponse(204, "Extraction method deleted")]
        [SwaggerResponse(404, "Extraction method not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteExtractionMethodCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }

    [Route("api/natural-resources")]
    [ApiController]
    public class NaturalResourceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NaturalResourceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get natural resources", Description = "Returns natural resources, optionally filtered by world")]
        [SwaggerResponse(200, "List of natural resources", typeof(IEnumerable<NaturalResourceDto>))]
        public async Task<ActionResult<IEnumerable<NaturalResourceDto>>> GetAll([FromQuery] int? worldId = null)
        {
            return Ok(await _mediator.Send(new GetAllNaturalResourcesQuery { WorldId = worldId }));
        }

        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get natural resource by ID")]
        [SwaggerResponse(200, "Natural resource found", typeof(NaturalResourceDetailsDto))]
        [SwaggerResponse(404, "Natural resource not found")]
        public async Task<ActionResult<NaturalResourceDetailsDto>> GetById(int id)
        {
            var naturalResource = await _mediator.Send(new GetNaturalResourceByIdQuery { Id = id });
            if (naturalResource == null) return NotFound();
            return Ok(naturalResource);
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new natural resource")]
        [SwaggerResponse(201, "Natural resource created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<NaturalResourceDto>> Create([FromBody] NaturalResourceCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateNaturalResourceCommand { NaturalResourceCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update natural resource by ID")]
        [SwaggerResponse(200, "Natural resource updated", typeof(NaturalResourceDto))]
        [SwaggerResponse(404, "Natural resource not found")]
        public async Task<ActionResult<NaturalResourceDto>> Update(int id, [FromBody] NaturalResourceUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateNaturalResourceCommand { Id = id, NaturalResourceUpdateDto = dto });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete natural resource by ID")]
        [SwaggerResponse(204, "Natural resource deleted")]
        [SwaggerResponse(404, "Natural resource not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteNaturalResourceCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/natural-resources/{id}/locations/{locationId}
        [HttpPost("{id}/locations/{locationId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Link a location to the natural resource")]
        [SwaggerResponse(204, "Location linked")]
        [SwaggerResponse(400, "Invalid target / already linked")]
        public async Task<IActionResult> AddLocation(int id, int locationId)
        {
            await _mediator.Send(new AddNaturalResourceLocationCommand { NaturalResourceId = id, LocationId = locationId });
            return NoContent();
        }

        // DELETE: /api/natural-resources/{id}/locations/{locationId}
        [HttpDelete("{id}/locations/{locationId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Unlink a location from the natural resource")]
        [SwaggerResponse(204, "Location unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveLocation(int id, int locationId)
        {
            var result = await _mediator.Send(new RemoveNaturalResourceLocationCommand { NaturalResourceId = id, LocationId = locationId });
            if (!result) return NotFound();
            return NoContent();
        }
    }

    [Route("api/trade-routes")]
    [ApiController]
    public class TradeRouteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TradeRouteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get trade routes", Description = "Returns trade routes, optionally filtered by world")]
        [SwaggerResponse(200, "List of trade routes", typeof(IEnumerable<TradeRouteDto>))]
        public async Task<ActionResult<IEnumerable<TradeRouteDto>>> GetAll([FromQuery] int? worldId = null)
        {
            return Ok(await _mediator.Send(new GetAllTradeRoutesQuery { WorldId = worldId }));
        }

        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get trade route by ID")]
        [SwaggerResponse(200, "Trade route found", typeof(TradeRouteDetailsDto))]
        [SwaggerResponse(404, "Trade route not found")]
        public async Task<ActionResult<TradeRouteDetailsDto>> GetById(int id)
        {
            var tradeRoute = await _mediator.Send(new GetTradeRouteByIdQuery { Id = id });
            if (tradeRoute == null) return NotFound();
            return Ok(tradeRoute);
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new trade route")]
        [SwaggerResponse(201, "Trade route created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<TradeRouteDto>> Create([FromBody] TradeRouteCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateTradeRouteCommand { TradeRouteCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update trade route by ID")]
        [SwaggerResponse(200, "Trade route updated", typeof(TradeRouteDto))]
        [SwaggerResponse(404, "Trade route not found")]
        public async Task<ActionResult<TradeRouteDto>> Update(int id, [FromBody] TradeRouteUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateTradeRouteCommand { Id = id, TradeRouteUpdateDto = dto });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete trade route by ID")]
        [SwaggerResponse(204, "Trade route deleted")]
        [SwaggerResponse(404, "Trade route not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteTradeRouteCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/trade-routes/{id}/locations/{locationId}
        [HttpPost("{id}/locations/{locationId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Link a location to the trade route")]
        [SwaggerResponse(204, "Location linked")]
        [SwaggerResponse(400, "Invalid target / already linked")]
        public async Task<IActionResult> AddLocation(int id, int locationId)
        {
            await _mediator.Send(new AddTradeRouteLocationCommand { TradeRouteId = id, LocationId = locationId });
            return NoContent();
        }

        // DELETE: /api/trade-routes/{id}/locations/{locationId}
        [HttpDelete("{id}/locations/{locationId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Unlink a location from the trade route")]
        [SwaggerResponse(204, "Location unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveLocation(int id, int locationId)
        {
            var result = await _mediator.Send(new RemoveTradeRouteLocationCommand { TradeRouteId = id, LocationId = locationId });
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: /api/trade-routes/{id}/resources/{resourceId}
        [HttpPost("{id}/resources/{resourceId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Link a natural resource to the trade route")]
        [SwaggerResponse(204, "Natural resource linked")]
        [SwaggerResponse(400, "Invalid target / already linked")]
        public async Task<IActionResult> AddResource(int id, int resourceId)
        {
            await _mediator.Send(new AddTradeRouteResourceCommand { TradeRouteId = id, NaturalResourceId = resourceId });
            return NoContent();
        }

        // DELETE: /api/trade-routes/{id}/resources/{resourceId}
        [HttpDelete("{id}/resources/{resourceId}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Unlink a natural resource from the trade route")]
        [SwaggerResponse(204, "Natural resource unlinked")]
        [SwaggerResponse(404, "Link not found")]
        public async Task<IActionResult> RemoveResource(int id, int resourceId)
        {
            var result = await _mediator.Send(new RemoveTradeRouteResourceCommand { TradeRouteId = id, NaturalResourceId = resourceId });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
