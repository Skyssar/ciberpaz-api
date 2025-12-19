using System.ComponentModel.DataAnnotations;

namespace ciberpaz_api.Models
{
    public class ViewItem
    {
        public int Id { get; set; }

        // FK (siempre apunta al Home con Id = 1)
        [Required]
        public int HomeId { get; set; } = 1;

        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }
        public string? Icon { get; set; }

        [MaxLength(50)]
        public string? Route { get; set; }
        public string? WebUrl { get; set; }

        public bool Embed { get; set; } = false;
        public int Order { get; set; }

        // Navegación
        public Home Home { get; set; } = null!;
    }
}
