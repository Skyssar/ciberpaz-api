namespace ciberpaz_api.DTOs
{
    public class MenuItemCreateDto
    {
        public required string Title { get; set; }
        public IFormFile? Icon { get; set; }
    }

    public class MenuItemUpdateDto
    {
        public string? Title { get; set; }
        public IFormFile? Icon { get; set; }
    }
}
