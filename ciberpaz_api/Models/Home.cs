using System.ComponentModel.DataAnnotations;

namespace ciberpaz_api.Models
{
    public class Home
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }

        public string? GovIcon { get; set; }
        public string? AppIcon { get; set; }

        // Relación 1 a muchos
        public ICollection<ViewItem> ViewItems { get; set; } = [];
    }
}
