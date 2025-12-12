using System.ComponentModel.DataAnnotations;

namespace ciberpaz_api.Models
{
    public class View
    {
        public int Id { get; set; }
        
        [MaxLength(100)]
        public required string Title { get; set; }
        public string? Image { get; set; }

        // Relación uno a muchos
        public ICollection<Section>? Sections { get; set; }
        public ICollection<Paragraph>? Paragraphs { get; set; }
    }
}
