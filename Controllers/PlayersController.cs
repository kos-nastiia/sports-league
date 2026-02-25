using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsLeague.Data;
using SportsLeague.Models;

namespace SportsLeague.Controllers
{
    public class PlayersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlayersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Players
        public async Task<IActionResult> Index()
        {
            var players = await _context.Players
                .Include(p => p.TeamPlayers)
                .ThenInclude(tp => tp.Team)
                .ToListAsync();

            return View(players);
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var player = await _context.Players
                .Include(p => p.TeamPlayers)
                .ThenInclude(tp => tp.Team)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (player == null) return NotFound();

            return View(player);
        }

        // GET: Players/Create
        public IActionResult Create()
        {
            ViewBag.Teams = _context.Teams.ToList();
            return View();
        }

        // POST: Players/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    [Bind("Id,FirstName,LastName,BirthDate,Position,Number")] Player player,
    int? teamId)
        {
            if (ModelState.IsValid)
            {
                _context.Add(player);
                await _context.SaveChangesAsync();

                if (teamId.HasValue)
                {
                    var teamPlayer = new TeamPlayer
                    {
                        PlayerId = player.Id,
                        TeamId = teamId.Value,
                        StartDate = DateTime.Now,
                        ContractType = "Standard"
                    };

                    _context.TeamPlayers.Add(teamPlayer);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Teams = _context.Teams.ToList();
            return View(player);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var player = await _context.Players.FindAsync(id);
            if (player == null) return NotFound();

            ViewBag.Teams = _context.Teams.ToList();
            var current = _context.TeamPlayers.FirstOrDefault(tp => tp.PlayerId == id && tp.EndDate == null);
            ViewBag.CurrentTeamId = current?.TeamId;

            return View(player);
        }


        // POST: Players/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,BirthDate,Position,Number")] Player player, int? teamId)
        {
            if (id != player.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Players.Any(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }

                // Handle team assignment/change
                var existing = _context.TeamPlayers.FirstOrDefault(tp => tp.PlayerId == player.Id && tp.EndDate == null);

                if (teamId.HasValue)
                {
                    if (existing == null)
                    {
                        var tp = new TeamPlayer
                        {
                            PlayerId = player.Id,
                            TeamId = teamId.Value,
                            StartDate = DateTime.Now,
                            ContractType = "Standard"
                        };
                        _context.TeamPlayers.Add(tp);
                        await _context.SaveChangesAsync();
                    }
                    else if (existing.TeamId != teamId.Value)
                    {
                        existing.EndDate = DateTime.Now;
                        _context.TeamPlayers.Update(existing);

                        var tp = new TeamPlayer
                        {
                            PlayerId = player.Id,
                            TeamId = teamId.Value,
                            StartDate = DateTime.Now,
                            ContractType = "Standard"
                        };
                        _context.TeamPlayers.Add(tp);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    // remove current team (set end date)
                    if (existing != null)
                    {
                        existing.EndDate = DateTime.Now;
                        _context.TeamPlayers.Update(existing);
                        await _context.SaveChangesAsync();
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Teams = _context.Teams.ToList();
            return View(player);
        }
    }
}
