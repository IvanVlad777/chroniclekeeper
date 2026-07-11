using ChronicleKeeper.Core.CQRS.JobRanks.Commands;
using ChronicleKeeper.Core.CQRS.JobRanks.Queries;
using ChronicleKeeper.Core.DTOs.JobRank;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChronicleKeeperAPI.Controllers
{
    [Route("api/job-ranks")]
    [ApiController]
    public class JobRankController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<JobRankController> _logger;

        public JobRankController(IMediator mediator, ILogger<JobRankController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: /api/job-ranks?worldId=1&professionId=2
        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get job ranks", Description = "Returns job ranks, optionally filtered by world and/or profession")]
        [SwaggerResponse(200, "List of job ranks", typeof(IEnumerable<JobRankDto>))]
        public async Task<ActionResult<IEnumerable<JobRankDto>>> GetAll([FromQuery] int? worldId = null, [FromQuery] int? professionId = null)
        {
            var jobRanks = await _mediator.Send(new GetAllJobRanksQuery { WorldId = worldId, ProfessionId = professionId });
            return Ok(jobRanks);
        }

        // GET: /api/job-ranks/{id}
        [HttpGet("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Get job rank by ID")]
        [SwaggerResponse(200, "Job rank found", typeof(JobRankDto))]
        [SwaggerResponse(404, "Job rank not found")]
        public async Task<ActionResult<JobRankDto>> GetById(int id)
        {
            var jobRank = await _mediator.Send(new GetJobRankByIdQuery { Id = id });
            if (jobRank == null) return NotFound();
            return Ok(jobRank);
        }

        // POST: /api/job-ranks
        [HttpPost]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Create new job rank", Description = "Job rank's world is derived from its profession")]
        [SwaggerResponse(201, "Job rank created")]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<JobRankDto>> Create([FromBody] JobRankCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("API: Creating job rank: {Name}", dto.Name);
            var result = await _mediator.Send(new CreateJobRankCommand { JobRankCreateDto = dto });
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: /api/job-ranks/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Update job rank by ID", Description = "A job rank's profession cannot be changed")]
        [SwaggerResponse(200, "Job rank updated", typeof(JobRankDto))]
        [SwaggerResponse(404, "Job rank not found")]
        public async Task<ActionResult<JobRankDto>> Update(int id, [FromBody] JobRankUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateJobRankCommand { Id = id, JobRankUpdateDto = dto });
            return Ok(result);
        }

        // DELETE: /api/job-ranks/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Editor,Admin,SuperAdmin")]
        [SwaggerOperation(Summary = "Delete job rank by ID")]
        [SwaggerResponse(204, "Job rank deleted")]
        [SwaggerResponse(404, "Job rank not found")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteJobRankCommand { Id = id });
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
