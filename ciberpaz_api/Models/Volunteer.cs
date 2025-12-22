using System.ComponentModel.DataAnnotations;

namespace ciberpaz_api.Models
{
    public class Volunteer
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public required string Email { get; set; }
        public String? Phone { get; set; }
    }
}
