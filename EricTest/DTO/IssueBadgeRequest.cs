using EricTest.Models;
using System.ComponentModel.DataAnnotations;

namespace EricTest.DTO
{
    public class IssueBadgeRequest
    {
        [Required]
        public string RecipientEmail { get; set; }
        public string AchievementId { get; set; }
        public DateTime Expires { get; set; }
        //public bool HashedIdentifier { get; set; } = false;
    }
}
