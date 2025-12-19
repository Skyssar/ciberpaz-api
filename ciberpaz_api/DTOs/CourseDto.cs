
namespace ciberpaz_api.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Image { get; set; }

        public string? Description { get; set; }

        public string? Link { get; set; }
    }

    public class CourseUpdateCreateDto
    {
        public string Title { get; set; } = null!;

        // imagen subida
        public IFormFile? Image { get; set; }

        public string? Description { get; set; }

        public string? Link { get; set; }
    }

}
