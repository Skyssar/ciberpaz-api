using System.ComponentModel.DataAnnotations;

namespace ciberpaz_api.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Title { get; set; }

        public string? Image { get; set; }

        public string? Description { get; set; }
        public string? Link { get; set; }
    }
}
