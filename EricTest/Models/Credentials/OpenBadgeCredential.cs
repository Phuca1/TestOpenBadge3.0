using System.Text.Json.Serialization;

namespace EricTest.Models
{
    public class OpenBadgeCredential
    {
        [JsonPropertyName("@context")]
        public string[] Context { get; } = new[]
        {
            "https://www.w3.org/2018/credentials/v1",
            "https://purl.imsglobal.org/spec/ob/v3p0/context-3.0.0.json"
        };

        [JsonPropertyName("type")]
        public string[] Type { get; } = new[] { "VerifiableCredential", "OpenBadgeCredential" };

        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("image")]
        public Image Image { get; set; }

        [JsonPropertyName("issuer")]
        public OpenBadgeIssuerInfo Issuer { get; set; } // Should be a DID

        [JsonPropertyName("validFrom")]
        public DateTime IssuanceDate { get; set; }

        [JsonPropertyName("validUntil")]
        public DateTime? ExpirationDate { get; set; }

        [JsonPropertyName("credentialSubject")]
        public CredentialSubject CredentialSubject { get; set; }

        [JsonPropertyName("proof")]
        public Proof Proof { get; set; }
    }

    public class OpenBadgeIssuerInfo
    {

        [JsonPropertyName("id")]
        public string Id { get; set; } // Issuer identifier, can be DID or URL
        [JsonPropertyName("type")]
        public string[] Type { get; } = new[] { "Profile" };
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("image")]
        public Image Image { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

    public class CredentialSubject
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }  // Can be DID, email, or URI

        [JsonPropertyName("type")]
        public string[] Type { get; } = new[] { "AchievementSubject" };

        [JsonPropertyName("achievement")]
        public Achievement Achievement { get; set; }

    }

    public class Proof
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "Ed25519Signature2020";

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("verificationMethod")]
        public string VerificationMethod { get; set; }

        [JsonPropertyName("proofPurpose")]
        public string ProofPurpose { get; set; } = "assertionMethod";

        [JsonPropertyName("proofValue")]
        public string ProofValue { get; set; }
    }
}
