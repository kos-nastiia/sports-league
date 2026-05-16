using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections.Generic;

namespace SportsLeague.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [StringLength(100)]
        public required string City { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Founded Date")]
        public DateTime? FoundedDate { get; set; }

        [StringLength(100)]
        public string? CoachName { get; set; }

        [StringLength(100)]
        public string? Stadium { get; set; }

        public ICollection<TeamPlayer> TeamPlayers { get; set; } = new List<TeamPlayer>();
    }
}
