using ciberpaz_api.Models;

namespace ciberpaz_api.DTOs
{
    public class EventDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public EventType Type { get; set; }
        public string? Image { get; set; }
        public DateTimeOffset Date { get; set; }
        public string? Link { get; set; }
    }

    public class EventsByTypeDto
    {
        public EventType Type { get; set; }
        public List<EventDto> Events { get; set; } = [];
    }

    public class EventCreateDto
    {
        public string? Title { get; set; }
        public EventType Type { get; set; } = EventType.other;
        public IFormFile? Image { get; set; }
        public DateTimeOffset Date { get; set; }
        public string? Link { get; set; }
    }

    public class EventUpdateDto
    {
        public string? Title { get; set; }
        public EventType Type { get; set; } 
        public IFormFile? Image { get; set; }
        public DateTimeOffset? Date { get; set; }
        public string? Link { get; set; }
    }
}
