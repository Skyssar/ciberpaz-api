using System.ComponentModel.DataAnnotations;

namespace ciberpaz_api.Models
{
    public class AppView
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }
        public string? Image { get; set; }
        public string? Route { get; set; }

        // Relación uno a muchos
        public ICollection<Paragraph>? Paragraphs { get; set; }
        public ICollection<Section>? Sections { get; set; }
    }
}
