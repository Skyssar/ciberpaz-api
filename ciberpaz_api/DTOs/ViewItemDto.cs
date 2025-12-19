namespace ciberpaz_api.DTOs
{
    public class ViewItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Icon { get; set; }     // ruta / url
        public string? Route { get; set; }
        public string? WebUrl { get; set; }
        public bool Embed { get; set; }
        public int Order { get; set; }
    }

    public class ViewItemCreateDto
    {
        public string Title { get; set; } = null!;
        public IFormFile? Icon { get; set; }
        public string? Route { get; set; }
        public string? WebUrl { get; set; }
        public bool Embed { get; set; } = false;
        public int Order { get; set; }
    }

    public class ViewItemUpdateDto
    {
        public string Title { get; set; } = null!;
        public IFormFile? Icon { get; set; }
        public string? Route { get; set; }
        public string? WebUrl { get; set; }
        public bool Embed { get; set; }
        public int Order { get; set; }
    }


}
