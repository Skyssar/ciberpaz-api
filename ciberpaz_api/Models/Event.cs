using System.ComponentModel.DataAnnotations;

namespace ciberpaz_api.Models
{
    public class Event
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string? Title { get; set; }

        public EventType Type { get; set; } = EventType.other;
        public string? Image { get; set; }

        [Required]
        public DateTimeOffset Date { get; set; }
        public string? Link { get; set; }
    }
}
