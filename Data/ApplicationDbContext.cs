using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportsLeague.Models;

namespace SportsLeague.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext<IdentityUser>(options)
    {
        public required DbSet<Team> Teams { get; set; }
        public required DbSet<Player> Players { get; set; }
        public required DbSet<TeamPlayer> TeamPlayers { get; set; }
        public required DbSet<ChatMessage> ChatMessages { get; set; }
    }
}
