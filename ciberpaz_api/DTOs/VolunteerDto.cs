using System.ComponentModel.DataAnnotations;

namespace ciberpaz_api.DTOs
{
    public class VolunteerDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public int? Phone { get; set; }
    }

    public class VolunteerUpdateCreateDto
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public int? Phone { get; set; }
    }
}
