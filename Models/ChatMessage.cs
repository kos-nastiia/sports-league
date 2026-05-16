using System;
using System.ComponentModel.DataAnnotations;

namespace SportsLeague.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }

        [Required]
        public int TeamId { get; set; }

        [Required]
        [StringLength(256)]
        public required string SenderName { get; set; }

        [Required]
        [StringLength(2000)]
        public required string MessageText { get; set; }

        public string? FilePath { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
