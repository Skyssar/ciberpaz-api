using System.ComponentModel.DataAnnotations;

namespace ciberpaz_api.Models
{
    public class Section
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string? Title { get; set; }

        [Required]
        public required string Content { get; set; }
        public string? Image { get; set; }

        public string? Link { get; set; }

        // Llave foránea
        public required int AppViewId { get; set; }

        // Navigation property
        public AppView? AppView { get; set; }
    }
}
