using System.ComponentModel.DataAnnotations;

namespace EricTest.Models
{
    public class Recipient
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; } = "email";
        [Required]
        public bool Hashed { get; set; } = false; // or "true" if using hashing
        [Required]
        public string Identity { get; set; } // The recipient's email address 
        public string PlainTextIdentity
        {
            get => Identity;
            set => Identity = value;
        }

        public string Salt { get; set; } // Optional, used if Hashed is "true"
    }
}
