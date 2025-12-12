namespace ciberpaz_api.Models
{
    public class Paragraph
    {
        public int Id { get; set; }

        public ParagraphType Type { get; set; }

        public required string Content { get; set; }

        // Llave foránea
        public required int ViewId { get; set; }

    }
}
