using ChronicleKeeper.Core.CQRS.BankingSystems.Commands;
using ChronicleKeeper.Core.CQRS.BankingSystems.Queries;
using ChronicleKeeper.Core.CQRS.Currencies.Commands;
using ChronicleKeeper.Core.CQRS.Currencies.Queries;
using ChronicleKeeper.Core.CQRS.EconomicSystems.Commands;
using ChronicleKeeper.Core.CQRS.EconomicSystems.Queries;
using ChronicleKeeper.Core.CQRS.TaxationSystems.Commands;
using ChronicleKeeper.Core.CQRS.TaxationSystems.Queries;
using ChronicleKeeper.Core.DTOs.BankingSystem;
using ChronicleKeeper.Core.DTOs.Currency;
using ChronicleKeeper.Core.DTOs.EconomicSystem;
using ChronicleKeeper.Core.DTOs.TaxationSystem;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/currencies")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CurrencyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get currencies", Description = "Returns currencies, optionally filtered by world")]
        [SwaggerResponse(200, "List of currencies", typeof(IEnumerable<CurrencyDto>))]
        public async Task<ActionResult<IEnumerable<CurrencyDto>>> GetAll([FromQuery] int? worldId = null)
        {
            return Ok(await _mediator.Send(new GetAllCurrenciesQuery { WorldId = worldId }));
        }

        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get currency by ID")]
        [SwaggerResponse(200, "Currency found", typeof(CurrencyDetailsDto))]
        [SwaggerResponse(404, "Currency not found")]
        public async Task<ActionResult<CurrencyDetailsDto>> GetById(int id)
        {
            var currency = await _mediator.Send(new GetCurrencyByIdQuery { Id = id });
            if (currency == null) return NotFound();
            return Ok(currency);
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new currency")]
        [SwaggerResponse(201, "Currency created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<CurrencyDto>> Create([FromBody] CurrencyCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateCurrencyCommand { CurrencyCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update currency by ID")]
        [SwaggerResponse(200, "Currency updated", typeof(CurrencyDto))]
        [SwaggerResponse(404, "Currency not found")]
        public async Task<ActionResult<CurrencyDto>> Update(int id, [FromBody] CurrencyUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateCurrencyCommand { Id = id, CurrencyUpdateDto = dto });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete currency by ID")]
        [SwaggerResponse(204, "Currency deleted")]
        [SwaggerResponse(404, "Currency not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteCurrencyCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }

    [Route("api/banking-systems")]
    [ApiController]
    public class BankingSystemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BankingSystemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get banking systems", Description = "Returns banking systems, optionally filtered by world")]
        [SwaggerResponse(200, "List of banking systems", typeof(IEnumerable<BankingSystemDto>))]
        public async Task<ActionResult<IEnumerable<BankingSystemDto>>> GetAll([FromQuery] int? worldId = null)
        {
            return Ok(await _mediator.Send(new GetAllBankingSystemsQuery { WorldId = worldId }));
        }

        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get banking system by ID")]
        [SwaggerResponse(200, "Banking system found", typeof(BankingSystemDetailsDto))]
        [SwaggerResponse(404, "Banking system not found")]
        public async Task<ActionResult<BankingSystemDetailsDto>> GetById(int id)
        {
            var bankingSystem = await _mediator.Send(new GetBankingSystemByIdQuery { Id = id });
            if (bankingSystem == null) return NotFound();
            return Ok(bankingSystem);
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new banking system")]
        [SwaggerResponse(201, "Banking system created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<BankingSystemDto>> Create([FromBody] BankingSystemCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateBankingSystemCommand { BankingSystemCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update banking system by ID")]
        [SwaggerResponse(200, "Banking system updated", typeof(BankingSystemDto))]
        [SwaggerResponse(404, "Banking system not found")]
        public async Task<ActionResult<BankingSystemDto>> Update(int id, [FromBody] BankingSystemUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateBankingSystemCommand { Id = id, BankingSystemUpdateDto = dto });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete banking system by ID")]
        [SwaggerResponse(204, "Banking system deleted")]
        [SwaggerResponse(404, "Banking system not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteBankingSystemCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }

    [Route("api/taxation-systems")]
    [ApiController]
    public class TaxationSystemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaxationSystemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get taxation systems", Description = "Returns taxation systems, optionally filtered by world")]
        [SwaggerResponse(200, "List of taxation systems", typeof(IEnumerable<TaxationSystemDto>))]
        public async Task<ActionResult<IEnumerable<TaxationSystemDto>>> GetAll([FromQuery] int? worldId = null)
        {
            return Ok(await _mediator.Send(new GetAllTaxationSystemsQuery { WorldId = worldId }));
        }

        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get taxation system by ID")]
        [SwaggerResponse(200, "Taxation system found", typeof(TaxationSystemDetailsDto))]
        [SwaggerResponse(404, "Taxation system not found")]
        public async Task<ActionResult<TaxationSystemDetailsDto>> GetById(int id)
        {
            var taxationSystem = await _mediator.Send(new GetTaxationSystemByIdQuery { Id = id });
            if (taxationSystem == null) return NotFound();
            return Ok(taxationSystem);
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new taxation system")]
        [SwaggerResponse(201, "Taxation system created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<TaxationSystemDto>> Create([FromBody] TaxationSystemCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateTaxationSystemCommand { TaxationSystemCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update taxation system by ID")]
        [SwaggerResponse(200, "Taxation system updated", typeof(TaxationSystemDto))]
        [SwaggerResponse(404, "Taxation system not found")]
        public async Task<ActionResult<TaxationSystemDto>> Update(int id, [FromBody] TaxationSystemUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateTaxationSystemCommand { Id = id, TaxationSystemUpdateDto = dto });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete taxation system by ID")]
        [SwaggerResponse(204, "Taxation system deleted")]
        [SwaggerResponse(404, "Taxation system not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteTaxationSystemCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }

    [Route("api/economic-systems")]
    [ApiController]
    public class EconomicSystemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EconomicSystemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get economic systems", Description = "Returns economic systems, optionally filtered by world")]
        [SwaggerResponse(200, "List of economic systems", typeof(IEnumerable<EconomicSystemDto>))]
        public async Task<ActionResult<IEnumerable<EconomicSystemDto>>> GetAll([FromQuery] int? worldId = null)
        {
            return Ok(await _mediator.Send(new GetAllEconomicSystemsQuery { WorldId = worldId }));
        }

        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get economic system by ID")]
        [SwaggerResponse(200, "Economic system found", typeof(EconomicSystemDetailsDto))]
        [SwaggerResponse(404, "Economic system not found")]
        public async Task<ActionResult<EconomicSystemDetailsDto>> GetById(int id)
        {
            var economicSystem = await _mediator.Send(new GetEconomicSystemByIdQuery { Id = id });
            if (economicSystem == null) return NotFound();
            return Ok(economicSystem);
        }

        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new economic system")]
        [SwaggerResponse(201, "Economic system created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<EconomicSystemDto>> Create([FromBody] EconomicSystemCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new CreateEconomicSystemCommand { EconomicSystemCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update economic system by ID")]
        [SwaggerResponse(200, "Economic system updated", typeof(EconomicSystemDto))]
        [SwaggerResponse(404, "Economic system not found")]
        public async Task<ActionResult<EconomicSystemDto>> Update(int id, [FromBody] EconomicSystemUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateEconomicSystemCommand { Id = id, EconomicSystemUpdateDto = dto });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete economic system by ID")]
        [SwaggerResponse(204, "Economic system deleted")]
        [SwaggerResponse(404, "Economic system not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteEconomicSystemCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
