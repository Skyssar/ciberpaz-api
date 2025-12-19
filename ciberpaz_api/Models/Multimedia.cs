namespace ciberpaz_api.Models
{
    public class Multimedia
    {
        public int Id { get; set; }

        public MultimediaType Type { get; set; }

        public string? Title { get; set; }

        public string? Icon { get; set; }

        public string? Link { get; set; }

    }
}
