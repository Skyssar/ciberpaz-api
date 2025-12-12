using System.ComponentModel.DataAnnotations;

namespace ciberpaz_api.Models
{
    public class Section
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string? Title { get; set; }
        public required string Content { get; set; }
        public string? Image { get; set; }

        public string? Link { get; set; }

        // Llave foránea
        public required int ViewId { get; set; }

        // Navigation property
        public View? View { get; set; }
    }
}
