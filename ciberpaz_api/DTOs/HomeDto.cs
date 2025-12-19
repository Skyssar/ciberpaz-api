using ciberpaz_api.Models;
using System.ComponentModel.DataAnnotations;

namespace ciberpaz_api.DTOs
{
    public class HomeDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? GovIcon { get; set; }
        public string? AppIcon { get; set; }

        public List<ViewItemDto>? ViewItems { get; set; }
    }

    public class HomeCreateDto
    {
        public string Title { get; set; } = null!;

        public IFormFile? GovIcon { get; set; }
        public IFormFile? AppIcon { get; set; }
    }

    public class HomePatchDto
    {
        public string? Title { get; set; }
        public IFormFile? GovIcon { get; set; }
        public IFormFile? AppIcon { get; set; }
    }
}
