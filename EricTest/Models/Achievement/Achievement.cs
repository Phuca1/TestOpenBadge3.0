using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EricTest.Models
{
    public class Achievement
    {
        [JsonPropertyName("@context")]
        public string[] Context { get; } = new[]
        {
            "https://w3id.org/openbadges/v3",
            "https://w3id.org/security/suites/ed25519-2020/v1"
        };

        [Required]
        [JsonPropertyName("id")]
        public string Id { get; set; }  // DID or URL format

        [JsonPropertyName("type")]
        public string Type { get; } = "Achievement";

        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [Required]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("image")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("criteria")]
        public Criteria Criteria { get; set; }

        [Required]
        [JsonPropertyName("issuer")]
        public string IssuerId { get; set; }  // Reference to IssuerProfile DID

        [JsonPropertyName("tags")]
        public string[] Tags { get; set; } = Array.Empty<string>();

        [JsonPropertyName("alignment")]
        public Alignment[] Alignments { get; set; } = Array.Empty<Alignment>();
    }

    public class Criteria
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("narrative")]
        public string? Narrative { get; set; }
    }

    public class Alignment
    {
        [JsonPropertyName("targetUrl")]
        public string TargetUrl { get; set; }

        [JsonPropertyName("targetName")]
        public string TargetName { get; set; }

        [JsonPropertyName("targetDescription")]
        public string? TargetDescription { get; set; }
    }
}
