using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EricTest.Models;
public class IssuerProfile
{
    [JsonPropertyName("@context")]
    public string[] Context { get; } = new[]
    {
        "https://w3id.org/openbadges/v3",
        "https://w3id.org/security/suites/ed25519-2020/v1"
    };

    [Required]
    [JsonPropertyName("id")]
    public string Id { get; set; }  // DID format: did:web:example.com

    [JsonPropertyName("type")]
    public string Type { get; } = "Profile";

    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("image")]
    public Image Image { get; set; }
    [Required]
    [JsonPropertyName("verification")]
    public VerificationMethod[] VerificationMethod { get; set; }

    [JsonPropertyName("revocationList")]
    public string? RevocationListUrl { get; set; }

    [JsonIgnore]
    public string? PrivateKey { get; set; } // Only stored server-side
}

public class VerificationMethod
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = "Ed25519VerificationKey2020";

    [JsonPropertyName("controller")]
    public string Controller { get; set; }

    [JsonPropertyName("publicKeyMultibase")]
    public string PublicKeyMultibase { get; set; }
}