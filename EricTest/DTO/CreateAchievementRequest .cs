using EricTest.Models;

namespace EricTest.DTO
{
    public class CreateAchievementRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public Criteria? Criteria { get; set; }
        public string IssuerId { get; set; }
        public string[] Tags { get; set; } = Array.Empty<string>();
        public Alignment[] Alignments { get; set; } = Array.Empty<Alignment>();
        public IFormFile? ImageFile { get; set; }
    }
}
