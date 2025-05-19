using EricTest.DTO;
using EricTest.Models;
using NSec.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EricTest.Services
{
    public class CredentialIssuerService
    {
        private static List<OpenBadgeCredential> _openBadgeCredentials = new List<OpenBadgeCredential>();
        private readonly AchievementService _achievementService;
        private readonly IssuerProfileService _issuerProfileService;
        private readonly IAuthService _authService;

        public CredentialIssuerService(AchievementService achievementService, IAuthService authService, IssuerProfileService issuerProfileService)
        {
            _achievementService = achievementService;
            _authService = authService;
            _issuerProfileService = issuerProfileService;
        }

        public OpenBadgeCredential IssueBadge(IssueBadgeRequest request)
        {
            var recipient = _authService.GetUserByEmail(request.RecipientEmail);
            if (recipient == null)
            {
                throw new Exception("Recipient not found.");
            }

            var achievement = _achievementService.GetAchievement(request.AchievementId);
            if(achievement == null)
            {
                throw new Exception("Achievement not found.");
            }
            var newId = Guid.NewGuid();

            var issuerProfile = _issuerProfileService.GetProfileWithPk("did:web:openbadges.local:issuers:main");

            var credential = new OpenBadgeCredential
            {
                Id = $"{GetBaseUrl}/credentials/{newId}",
                Name = achievement.Name,
                Description = achievement.Description,
                Image = new Image
                {
                    Id = achievement.ImageUrl,
                    Caption = achievement.Name,
                },
                Issuer = new OpenBadgeIssuerInfo
                {
                    Id = issuerProfile.Id,
                    Name = issuerProfile.Name,
                    Url = issuerProfile.Url,
                    Image = issuerProfile.Image,
                    Email = issuerProfile.Email,
                },
                IssuanceDate = DateTime.UtcNow,
                ExpirationDate = request.Expires,
                CredentialSubject = new CredentialSubject
                {
                    Id = recipient.Email,
                    Achievement = achievement
                },
            };
            credential.Proof = GenerateProof(credential, issuerProfile);

            _openBadgeCredentials.Add(credential);
            return credential;
        }

        private Proof GenerateProof(OpenBadgeCredential credential, IssuerProfile issuer)
        {
            var algorithm = SignatureAlgorithm.Ed25519;
            var key = Key.Import(algorithm, Convert.FromBase64String(issuer.PrivateKey), KeyBlobFormat.RawPrivateKey);
            var jsonToSign = JsonSerializer.Serialize(credential, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
            var normalizedJson = CanonicalizeJson(jsonToSign);
            var dataToSign = Encoding.UTF8.GetBytes(normalizedJson);
            var signature = algorithm.Sign(key, dataToSign);

            return new Proof
            {
                Type = "Ed25519Signature2020",
                Created = DateTime.UtcNow,
                VerificationMethod = issuer.VerificationMethod[0].Id,
                ProofPurpose = "assertionMethod",
                ProofValue = Convert.ToBase64String(signature),
            };
        }

        private static string CanonicalizeJson(string json)
        {
            // For production, use a proper JSON-LD canonicalization library
            // This simplified version works for basic cases:
            using var doc = JsonDocument.Parse(json);
            using var stream = new MemoryStream();
            using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
            {
                Indented = false,
                SkipValidation = true
            }))
            {
                doc.WriteTo(writer);
            }
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        private string GetBaseUrl() => "https://openbadges.local";
    }
}
