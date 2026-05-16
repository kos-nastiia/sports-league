using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsLeague.Models;
using SportsLeague.Repositories;

namespace SportsLeague.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersApiController : ControllerBase
    {
        private readonly IRepository<Player> _repository;

        public PlayersApiController(IRepository<Player> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            var players = await _repository.GetAllAsync();
            return Ok(players);
        }

        [HttpGet("with-teams")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayersWithTeams()
        {
            var players = await _repository.GetAllAsync(query => query
                .Include(p => p.TeamPlayers)
                    .ThenInclude(tp => tp.Team));
            return Ok(players);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _repository.GetByIdAsync(id);
            if (player is null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        [HttpGet("{id}/with-teams")]
        public async Task<ActionResult<Player>> GetPlayerWithTeams(int id)
        {
            var player = await _repository.GetByIdAsync(id, query => query
                .Include(p => p.TeamPlayers)
                    .ThenInclude(tp => tp.Team));
            if (player is null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        [HttpPost]
        public async Task<ActionResult<Player>> CreatePlayer([FromBody] Player player)
        {
            if (player is null)
            {
                return BadRequest();
            }

            await _repository.AddAsync(player);
            return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, player);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer(int id, [FromBody] Player player)
        {
            if (player is null || player.Id != id)
            {
                return BadRequest();
            }

            var existingPlayer = await _repository.GetByIdAsync(id);
            if (existingPlayer is null)
            {
                return NotFound();
            }

            await _repository.UpdateAsync(player);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var existingPlayer = await _repository.GetByIdAsync(id);
            if (existingPlayer is null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
