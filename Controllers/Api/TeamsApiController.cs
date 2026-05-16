using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsLeague.Models;
using SportsLeague.Repositories;

namespace SportsLeague.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsApiController : ControllerBase
    {
        private readonly IRepository<Team> _repository;

        public TeamsApiController(IRepository<Team> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
        {
            var teams = await _repository.GetAllAsync();
            return Ok(teams);
        }

        [HttpGet("with-players")]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeamsWithPlayers()
        {
            var teams = await _repository.GetAllAsync(query => query
                .Include(t => t.TeamPlayers)
                    .ThenInclude(tp => tp.Player));
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            var team = await _repository.GetByIdAsync(id);
            if (team is null)
            {
                return NotFound();
            }

            return Ok(team);
        }

        [HttpGet("{id}/with-players")]
        public async Task<ActionResult<Team>> GetTeamWithPlayers(int id)
        {
            var team = await _repository.GetByIdAsync(id, query => query
                .Include(t => t.TeamPlayers)
                    .ThenInclude(tp => tp.Player));
            if (team is null)
            {
                return NotFound();
            }

            return Ok(team);
        }

        [HttpPost]
        public async Task<ActionResult<Team>> CreateTeam([FromBody] Team team)
        {
            if (team is null)
            {
                return BadRequest();
            }

            await _repository.AddAsync(team);
            return CreatedAtAction(nameof(GetTeam), new { id = team.Id }, team);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, [FromBody] Team team)
        {
            if (team is null || team.Id != id)
            {
                return BadRequest();
            }

            var existingTeam = await _repository.GetByIdAsync(id);
            if (existingTeam is null)
            {
                return NotFound();
            }

            await _repository.UpdateAsync(team);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var existingTeam = await _repository.GetByIdAsync(id);
            if (existingTeam is null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
