using System.ComponentModel.DataAnnotations;

namespace EricTest.Models
{
    public class CreateAchievementViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string? ImageUrl { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? ImageFile { get; set; }

        public Criteria Criteria { get; set; } = new Criteria();

        [Required]
        public string IssuerId { get; set; }

        public string[] Tags { get; set; } = Array.Empty<string>();
        public Alignment[] Alignments { get; set; } = Array.Empty<Alignment>();
    }
}
