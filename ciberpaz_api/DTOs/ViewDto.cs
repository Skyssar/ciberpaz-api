namespace ciberpaz_api.DTOs
{
    public class ViewCreateDto
    {
        public required string Title { get; set; }
        public IFormFile? Image { get; set; }
    }

    public class ViewUpdateDto
    {
        public string? Title { get; set; }
        public IFormFile? Image { get; set; }
    }

    public class ViewDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Image { get; set; }

        public List<SectionDto>? Sections { get; set; }
        public List<ParagraphDto>? Paragraphs { get; set; }
    }

    public class ViewListDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Image { get; set; }
    }
}
