using ChronicleKeeper.Core.CQRS.Military;
using ChronicleKeeper.Core.DTOs.Military;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChronicleKeeperAPI.Controllers
{
    [ApiController]
    [Route("api/military-doctrines")]
    public class MilitaryDoctrineController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MilitaryDoctrineController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<MilitaryDoctrineDto>>> GetAll([FromQuery] int? worldId = null) =>
            Ok(await _mediator.Send(new GetAllMilitaryDoctrinesQuery { WorldId = worldId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<MilitaryDoctrineDetailsDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetMilitaryDoctrineByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<MilitaryDoctrineDto>> Create([FromBody] MilitaryDoctrineCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateMilitaryDoctrineCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<MilitaryDoctrineDto>> Update(int id, [FromBody] MilitaryDoctrineUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateMilitaryDoctrineCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteMilitaryDoctrineCommand { Id = id }) ? NoContent() : NotFound();
    }

    [ApiController]
    [Route("api/military-organizations")]
    public class MilitaryOrganizationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MilitaryOrganizationController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<MilitaryOrganizationDto>>> GetAll([FromQuery] int? worldId = null) =>
            Ok(await _mediator.Send(new GetAllMilitaryOrganizationsQuery { WorldId = worldId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<MilitaryOrganizationDetailsDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetMilitaryOrganizationByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<MilitaryOrganizationDto>> Create([FromBody] MilitaryOrganizationCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateMilitaryOrganizationCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<MilitaryOrganizationDto>> Update(int id, [FromBody] MilitaryOrganizationUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateMilitaryOrganizationCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteMilitaryOrganizationCommand { Id = id }) ? NoContent() : NotFound();

        [HttpPost("{id}/countries/{countryId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> AddCountry(int id, int countryId)
        { await _mediator.Send(new AddOrganizationCountryCommand { OrganizationId = id, CountryId = countryId }); return NoContent(); }

        [HttpDelete("{id}/countries/{countryId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> RemoveCountry(int id, int countryId)
        { await _mediator.Send(new RemoveOrganizationCountryCommand { OrganizationId = id, CountryId = countryId }); return NoContent(); }

        [HttpPost("{id}/factions/{factionId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> AddFaction(int id, int factionId)
        { await _mediator.Send(new AddOrganizationFactionCommand { OrganizationId = id, FactionId = factionId }); return NoContent(); }

        [HttpDelete("{id}/factions/{factionId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> RemoveFaction(int id, int factionId)
        { await _mediator.Send(new RemoveOrganizationFactionCommand { OrganizationId = id, FactionId = factionId }); return NoContent(); }
    }

    [ApiController]
    [Route("api/armies")]
    public class ArmyController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ArmyController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<ArmyDto>>> GetAll([FromQuery] int? worldId = null) =>
            Ok(await _mediator.Send(new GetAllArmiesQuery { WorldId = worldId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<ArmyDetailsDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetArmyByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<ArmyDto>> Create([FromBody] ArmyCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateArmyCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<ArmyDto>> Update(int id, [FromBody] ArmyUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateArmyCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteArmyCommand { Id = id }) ? NoContent() : NotFound();

        [HttpPost("{id}/battles/{battleId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> AddBattle(int id, int battleId)
        { await _mediator.Send(new AddArmyBattleCommand { ArmyId = id, BattleId = battleId }); return NoContent(); }

        [HttpDelete("{id}/battles/{battleId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> RemoveBattle(int id, int battleId)
        { await _mediator.Send(new RemoveArmyBattleCommand { ArmyId = id, BattleId = battleId }); return NoContent(); }
    }

    [ApiController]
    [Route("api/military-units")]
    public class MilitaryUnitController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MilitaryUnitController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<MilitaryUnitDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] int? armyId = null) =>
            Ok(await _mediator.Send(new GetAllMilitaryUnitsQuery { WorldId = worldId, ArmyId = armyId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<MilitaryUnitDetailsDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetMilitaryUnitByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<MilitaryUnitDto>> Create([FromBody] MilitaryUnitCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateMilitaryUnitCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<MilitaryUnitDto>> Update(int id, [FromBody] MilitaryUnitUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateMilitaryUnitCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteMilitaryUnitCommand { Id = id }) ? NoContent() : NotFound();

        [HttpPost("{id}/equipment/{equipmentId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> AddEquipment(int id, int equipmentId)
        { await _mediator.Send(new AddUnitEquipmentCommand { UnitId = id, EquipmentId = equipmentId }); return NoContent(); }

        [HttpDelete("{id}/equipment/{equipmentId}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> RemoveEquipment(int id, int equipmentId)
        { await _mediator.Send(new RemoveUnitEquipmentCommand { UnitId = id, EquipmentId = equipmentId }); return NoContent(); }
    }

    [ApiController]
    [Route("api/military-ranks")]
    public class MilitaryRankController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MilitaryRankController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<MilitaryRankDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] int? unitId = null) =>
            Ok(await _mediator.Send(new GetAllMilitaryRanksQuery { WorldId = worldId, UnitId = unitId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<MilitaryRankDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetMilitaryRankByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<MilitaryRankDto>> Create([FromBody] MilitaryRankCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateMilitaryRankCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<MilitaryRankDto>> Update(int id, [FromBody] MilitaryRankUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateMilitaryRankCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteMilitaryRankCommand { Id = id }) ? NoContent() : NotFound();
    }

    [ApiController]
    [Route("api/battles")]
    public class BattleController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BattleController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<BattleDto>>> GetAll([FromQuery] int? worldId = null) =>
            Ok(await _mediator.Send(new GetAllBattlesQuery { WorldId = worldId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<BattleDetailsDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetBattleByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<BattleDto>> Create([FromBody] BattleCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateBattleCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<BattleDto>> Update(int id, [FromBody] BattleUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateBattleCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteBattleCommand { Id = id }) ? NoContent() : NotFound();
    }

    [ApiController]
    [Route("api/military-equipment")]
    public class MilitaryEquipmentController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MilitaryEquipmentController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<MilitaryEquipmentDto>>> GetAll([FromQuery] int? worldId = null) =>
            Ok(await _mediator.Send(new GetAllMilitaryEquipmentQuery { WorldId = worldId }));

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<MilitaryEquipmentDetailsDto>> GetById(int id)
        {
            var e = await _mediator.Send(new GetMilitaryEquipmentByIdQuery { Id = id });
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost, Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<MilitaryEquipmentDto>> Create([FromBody] MilitaryEquipmentCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(new CreateMilitaryEquipmentCommand { Dto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<ActionResult<MilitaryEquipmentDto>> Update(int id, [FromBody] MilitaryEquipmentUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _mediator.Send(new UpdateMilitaryEquipmentCommand { Id = id, Dto = dto }));
        }

        [HttpDelete("{id}"), Authorize(Roles = "Editor,Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id) =>
            await _mediator.Send(new DeleteMilitaryEquipmentCommand { Id = id }) ? NoContent() : NotFound();
    }
}
