using EricTest.DTO;
using EricTest.Models;
using EricTest.Services;
using SimpleBase;
using System.Text.Json;

namespace EricTest.Services
{
    public class IssuerProfileService
    {
        private readonly KeyService _keyService;
        private readonly IWebHostEnvironment _env;
        private static IssuerProfile _issuerProfile;

        public IssuerProfileService(KeyService keyService, IWebHostEnvironment env)
        {
            _keyService = keyService;
            _env = env;
            EnsureProfilesDirectoryExists();
        }

        public IssuerProfile CreateProfile(CreateIssuerProfileRequest request)
        {
            var did = $"did:web:{request.Domain}:issuers:main";

            // Generate a new key pair
            var (publicKey, privateKey) = _keyService.GenerateEd25519KeyPair();
            var publicKeyMultibase = ConvertKeyToMultibase(publicKey);

            var verificationMethod = new VerificationMethod
            {
                Id = $"{did}#key-1",
                Type = "Ed25519VerificationKey2020",
                Controller = did,
                PublicKeyMultibase = publicKeyMultibase
            };
            // Create the issuer profile
            var profile = new IssuerProfile
            {
                Id = did,
                Name = request.Name,
                Url = request.Url,
                Email = request.Email,
                Description = request.Description,
                VerificationMethod = new[] { verificationMethod },
                RevocationListUrl = $"{request.Url}/revocations/{Guid.NewGuid()}",
                PrivateKey = privateKey 
            };
            _issuerProfile = profile;
            SaveProfile(profile);
            SaveDidDocument(profile);

            return profile;
        }

        public IssuerProfile GetProfile(string did)
        {
            var safeDid = did.Replace(":", "_");
            var path = Path.Combine(
                _env.WebRootPath,
                "profiles",
                $"{safeDid}.json");
            if (!File.Exists(path))
                return null;
            var profileJson = File.ReadAllText(path);
            return JsonSerializer.Deserialize<IssuerProfile>(profileJson);
        }

        public IssuerProfile GetProfileWithPk(string did)
        {
            return _issuerProfile;
        }

        private string ConvertKeyToMultibase(string base64Key)
        {
            // Convert the public key to Multibase format
            var bytes = Convert.FromBase64String(base64Key);
            return "z" + Base58.Bitcoin.Encode(bytes);

        }

        private void SaveProfile(IssuerProfile profile)
        {
            var profilePath = Path.Combine(
                _env.WebRootPath,
                "profiles",
                $"{profile.Id.Replace(":", "_")}.json");

            var options = new JsonSerializerOptions { WriteIndented = true };

            File.WriteAllText(profilePath, JsonSerializer.Serialize(profile, options));
        }

        private void SaveDidDocument(IssuerProfile profile)
        {
            var didDoc = new
            {
                id = profile.Id,
                verificationMethod = profile.VerificationMethod.Select(vm => new
                {
                    id = vm.Id,
                    type = vm.Type,
                    controller = vm.Controller,
                    publicKeyMultibase = vm.PublicKeyMultibase
                }),
                authentication = new[] { $"{profile.Id}#key-1" }
            };

            var didPath = Path.Combine(_env.WebRootPath, ".well-known", "did.json");

            Directory.CreateDirectory(Path.GetDirectoryName(didPath) ?? string.Empty);
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(didPath, JsonSerializer.Serialize(didDoc, options));
        }

        private void EnsureProfilesDirectoryExists()
        {
            var path = Path.Combine(_env.WebRootPath, "profiles");
            Directory.CreateDirectory(path);
        }
    }
}
