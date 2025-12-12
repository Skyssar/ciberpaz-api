using ciberpaz_api.Models;

namespace ciberpaz_api.DTOs
{
    public class ParagraphDto
    {
        public int Id { get; set; }
        public ParagraphType Type { get; set; }
        public required string Content { get; set; }
    }
}
