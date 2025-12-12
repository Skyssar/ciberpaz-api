namespace ciberpaz_api.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public bool Main { get; set; } = false;
        public string Icon { get; set; } = string.Empty;  // URL o ruta relativa
    }
}
