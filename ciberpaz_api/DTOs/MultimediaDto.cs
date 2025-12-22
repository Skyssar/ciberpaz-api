using ciberpaz_api.Models;

namespace ciberpaz_api.DTOs
{
    public class MultimediaDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public MultimediaType? Type { get; set; }
        public string? Icon { get; set; }
        public string? Link { get; set; }
    }

    public class MultimediaCreateDto
    {
        public string Title { get; set; } = null!;
        public MultimediaType Type { get; set; } = MultimediaType.social;
        public IFormFile? Icon { get; set; }
        public string? Link { get; set; }
    }

    public class MultimediaUpdateDto
    {
        public string? Title { get; set; }
        public MultimediaType Type { get; set; }
        public IFormFile? Icon { get; set; }
        public string? Link { get; set; }
    }

    public class MultimediaByTypeDto
    {
        public MultimediaType Type { get; set; }
        public List<MultimediaDto> Multimedia { get; set; } = [];
    }
}
