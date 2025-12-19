namespace ciberpaz_api.DTOs
{
    public class AppViewCreateDto
    {
        public required string Title { get; set; }
        public IFormFile? Image { get; set; }
        public string? Route { get; set; }
    }

    public class AppViewUpdateDto
    {
        public string? Title { get; set; }
        public IFormFile? Image { get; set; }
        public string? Route { get; set; }
    }

    public class AppViewDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Image { get; set; }
        public string? Route { get; set; }

        public List<SectionDto>? Sections { get; set; }
        public List<ParagraphDto>? Paragraphs { get; set; }
    }

    public class AppViewListDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Image { get; set; }
        public string? Route { get; set; }

    }
}
