namespace ciberpaz_api.DTOs
{
    public class SectionCreateDto
    {
        public string? Title { get; set; }
        public required string Content { get; set; }
        public IFormFile? Image { get; set; }
        public string? Link { get; set; }

        public required int ViewId { get; set; }
    }

    public class SectionUpdateDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public IFormFile? Image { get; set; }
        public string? Link { get; set; }

        public required int ViewId { get; set; }
    }

    public class SectionDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public required string Content { get; set; }
        public string? Image { get; set; }
        public string? Link { get; set; }


    }
}
