using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SportsLeague.Models
{
    public class TeamPlayer
    {
        public int Id { get; set; }

        public int TeamId { get; set; }
        public Team? Team { get; set; }

        public int PlayerId { get; set; }
        public Player? Player { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Required]
        [StringLength(50)]
        public required string ContractType { get; set; }
    }
}
