using System.ComponentModel.DataAnnotations;

namespace SportsLeague.Models
{
    public class Player
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Position { get; set; }

        public int Number { get; set; }

        public int TeamId { get; set; } // зовнішній ключ
        public Team Team { get; set; } // навігаційне поле

        public ICollection<TeamPlayer> TeamPlayers { get; set; }
    }
}
