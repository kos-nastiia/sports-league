using System.ComponentModel.DataAnnotations;

namespace SportsLeague.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string City { get; set; }

        public DateTime FoundedDate { get; set; }

        public string CoachName { get; set; }

        public string Stadium { get; set; }

        public ICollection<TeamPlayer> TeamPlayers { get; set; }
    }
}
